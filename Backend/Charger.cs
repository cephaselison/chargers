using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public class Charger
    {
        public const double MIN_CURRENT = 10;
        public const double MAX_CURRENT = 32;

        public Charger(Guid id, string name)
        {
            this.Id = id;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        [JsonIgnore]
        public Installation Installation { get; set; }

        public Guid? InstallationId
        {
            get
            {
                if(this.Installation == null) return null;
                return this.Installation.Id;
            }
        }

        public Vehicle ConnectedVehicle { get; set; }

        private double _allocatedCurrent;
        public double AllocatedCurrent
        {
            get { return _allocatedCurrent; }
            set
            {
                if(_allocatedCurrent == value)
                {
                    return;
                }

                if(value < 0)
                {
                    throw new InvalidOperationException($"Cannot allocate less than 0 ({value})");
                }
                else if(value > MAX_CURRENT)
                {
                    throw new InvalidOperationException($"Cannot allocate more than MaxCurrent ({value}/{MAX_CURRENT})");
                }
                if(value > 0 && value < MIN_CURRENT)
                {
                    throw new InvalidOperationException($"Cannot allocate less than MinCurrent ({value}/{MIN_CURRENT})");
                }

                _allocatedCurrent = value;
                Queue.GetInstance().NotifyChange(this, nameof(this.AllocatedCurrent));
            }
        }

        public void ConnectVehicle()
        {
            if(this.ConnectedVehicle != null)
            {
                throw new InvalidOperationException("Charger is already connected");
            }

            this.AllocatedCurrent = this.Installation.RequestCharge(this);

            var vehicle = new Vehicle();

            this.ConnectedVehicle = vehicle;
            this.ConnectedVehicle.ConnectAndStartCharging(this);
        }

        public void DisconnectVehicle()
        {
            if(this.ConnectedVehicle == null)
            {
                throw new InvalidOperationException("Charger is already disconnected");
            }

            this.ConnectedVehicle.Disconnect();
            this.ConnectedVehicle = null;
            this.AllocatedCurrent = 0;
        }
    }
}