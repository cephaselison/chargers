using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend
{
    public class Installation
    {
        public Installation(string name, double circuitBreakerCurrent, IEnumerable<Charger> chargers)
        {
            this.Id = Guid.Parse("715bf20a-f68f-41d1-8294-f3246705bcd0");
            this.Name = name;
            this.CircuitBreakerCurrent = circuitBreakerCurrent;
            this.Chargers = chargers ?? throw new ArgumentNullException(nameof(chargers));

            _controller = new ChargeController();
            Queue.GetInstance().Subscribe(this.OnChange);
        }

        private ChargeController _controller;

        public IEnumerable<Charger> Chargers { get; private set; }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public double CircuitBreakerCurrent { get; private set; }

        public bool CircuitBreakerTripped { get; private set; }

        public Installation Initialize()
        {
            if(this.Chargers != null)
            {
                foreach(var charger in this.Chargers)
                {
                    charger.Installation = this;
                }
            }
            return this;
        }

        /// <summary>
        /// This method will be called when a vehicle is requesting charge.
        /// You must evaluate other charger is the installation, and determine
        /// how much current is available for the charger.
        /// </summary>
        public double RequestCharge(Charger charger)
        {
            return _controller.RequestCharge(charger);
        }

        private void OnChange(Charger charger, string changedProperty)
        {
            this.AssertCircuitBreaker();
            _controller.OnChange(charger, changedProperty);
        }

        private void AssertCircuitBreaker()
        {
            var chargeCurrents = this.Chargers
                .Where(_ => _.ConnectedVehicle != null)
                .Sum(_ => _.ConnectedVehicle.ChargeCurrent);

            if(chargeCurrents > this.CircuitBreakerCurrent)
            {
                Console.WriteLine("##########################################");
                Console.WriteLine($"##### CIRCUIT BREAKER TRIPPED!");
                Console.WriteLine("##########################################");
                
                this.CircuitBreakerTripped = true;
                this.CircuitBreakerCurrent = 0;
            }
        }

        #region static

        private static IEnumerable<Installation> _installations = new [] {
            new Installation(
                "Installation 1",
                32,
                new []
                {
                    new Charger(Guid.Parse("f1a2d0ef-490f-4903-ab83-69d5c5a47d73"), "Charger 1"),
                    new Charger(Guid.Parse("04c820c7-a583-49af-8a18-a187959eb0dd"), "Charger 2"),
                    new Charger(Guid.Parse("77c45095-b0dc-45d7-b51b-430e79d7fbf2"), "Charger 3"),
                    new Charger(Guid.Parse("72a8a53d-b275-4b2b-bdc5-80a478d06606"), "Charger 4"),
                    new Charger(Guid.Parse("dc8293fd-3538-4ae0-a665-460dddf656f9"), "Charger 5")
                })
                .Initialize()
        };

        public static IEnumerable<Installation> GetInstances() {
            return _installations;
        }

        #endregion
    }
}