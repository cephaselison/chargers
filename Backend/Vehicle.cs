using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace Backend
{
    public class Vehicle
    {
        private CancellationTokenSource _cancellationTokenSource;

        public Vehicle()
        {
            Queue.GetInstance().Subscribe((charger, prop) => {
                if(prop == nameof(charger.AllocatedCurrent) && charger == _charger)
                {
                    this.ChargeCurrent = charger.AllocatedCurrent;
                }
            });
        }

        public void Disconnect()
        {
            if(_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }

            this.ChargeCurrent = 0;
            _charger = null;
            Console.WriteLine("Vehicle is disconnected");
        }

        public void ConnectAndStartCharging(Charger charger)
        {
            if (_cancellationTokenSource != null)
            {
                throw new Exception("Vehicle is already charging");
            }

            _charger = charger;
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(
                async () =>
                {
                    await this.ChargeLoop(_cancellationTokenSource);

                    _cancellationTokenSource = null;
                    Console.WriteLine("Vehicle charging is stopped");
                });
        }

        private async Task ChargeLoop(CancellationTokenSource cancellationTokenSource)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();

            this.ChargeCurrent = this.ResolveChargeCurrent();
            
            while ((this.ChargeCurrent = this.ResolveChargeCurrent()) > 0 && !cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine($"Vehicle charging with {this.ChargeCurrent}A (total: {(double) this.BatteryChargeWh / 1000}kWh)");
                var hourSinceLast = stopWatch.Elapsed.TotalSeconds / 15;
                stopWatch.Restart();

                this.BatteryChargeWh = Math.Min(
                    this.BatteryCapacityWh,
                    this.BatteryChargeWh + (long) (this.ChargeCurrent * 230 * hourSinceLast)
                );

                await Task.Delay(TimeSpan.FromSeconds(1), _cancellationTokenSource.Token);
            }

            if (this.BatteryChargeWh >= this.BatteryCapacityWh)
            {
                Console.WriteLine("Vehicle fully charged");
            }
            else
            {
                Console.WriteLine("Vehicle charging aborted");
            }

            this.ChargeCurrent = 0;
        }

        private double ResolveChargeCurrent()
        {
            if(_charger.Installation.CircuitBreakerTripped)
            {
                return 0;
            }

            if(this.BatteryChargeWh >= this.BatteryCapacityWh)
            {
                return 0;
            }
            
            var divisor = this.IsRampDown(this.BatteryChargeWh) ? 4 : 1;
            return Math.Min(
                this.AvailableCurrent,
                Charger.MAX_CURRENT / divisor
                );
        }
        
        /// <summary>
        /// When battery is nearing capacity maximum charge rate will drop.
        /// </summary>
        private bool IsRampDown(long chargeWh)
        {
            return chargeWh >= this.BatteryCapacityWh / 2;
        }

        private Charger _charger;

        public long BatteryCapacityWh { get { return 14720; } }

        public bool Charging { get { return this.ChargeCurrent > 0; } }

        private double _chargeCurrent = 0;

        public double ChargeCurrent
        {
            get
            {
                return _chargeCurrent;
            }

            private set
            {
                var changed = _chargeCurrent != value;
                
                _chargeCurrent = value;
                
                if (changed)
                {
                    Queue.GetInstance().NotifyChange(_charger, nameof(this.ChargeCurrent));
                }
            }
        }

        public double AvailableCurrent
        {
            get { return _charger != null ? _charger.AllocatedCurrent : 0; }
        }

        public long BatteryChargeWh { get; set; }
    }
}