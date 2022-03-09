using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class ChargeController
    {
        /// <summary>
        /// Called when a charger is connected.
        /// </summary>
        /// <returns>
        /// The initial charge current allocated to the charger.
        /// </returns>
        public double RequestCharge(Charger charger)
        {
            if(charger.Installation == null || charger.Installation.Chargers == null) {
                return 0;
            }

            var active = charger.Installation
                .Chargers
                .Where(_ => _.ConnectedVehicle != null);

            return Math.Min(charger.Installation.CircuitBreakerCurrent, Charger.MAX_CURRENT);
        }

        /// <summary>
        /// Called when a charger's allocated or charge current is changed.
        /// </summary>
        public void OnChange(Charger charger, string changedProperty)
        {

        }
    }
}