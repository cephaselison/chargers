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
        
            var activeCount = active.Count(); 
            
            // Update other chargers
            if (activeCount > 1) {
                var dividedCurrent = GetDividedCurrent(charger.Installation.CircuitBreakerCurrent, Charger.MAX_CURRENT, activeCount);
                
                charger.Installation.Chargers
                .Where(x => x.Id != charger.Id) // Unnecessary but
                .ToList()
                .ForEach(x => x.AllocatedCurrent = dividedCurrent);

                return dividedCurrent; 
            }

            return Math.Min(charger.Installation.CircuitBreakerCurrent, Charger.MAX_CURRENT);
        }

         /// <summary>
        /// X = CircuitBreakerCurrent
        /// Y = Max Current
        /// Z = Count

        /// STEPS
        /// 1. X / Z  but check if lower than max.
        /// 2. Y / Z max of 1. 
        /// 4. Check if 2. is lower than Min.Current otherwise ignore and diversify current to other? (Not sure if that's a usecase)
        /// 5. Check if Charger is fully charged, otherwise lower current there and put more on other
        /// </summary>
        private static double GetDividedCurrent(double circuitBreakerCurrent, double maxCurrent, int activeCount) {
            var initDividedCurrent = circuitBreakerCurrent / activeCount;

            var minMaxCurrent = maxCurrent / activeCount;

            return Math.Min(initDividedCurrent, minMaxCurrent);
        }

        /// <summary>
        /// Called when a charger's allocated or charge current is changed.
        /// </summary>
        public void OnChange(Charger charger, string changedProperty)
        {

        }
    }
}