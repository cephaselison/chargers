using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargersController : ControllerBase
    {
        private static Installation GetInstallation(Guid installationId)
        {
            return Installation
                .GetInstances()
                .Single(i => i.Id == installationId);
        }

        private static Charger GetCharger(Guid chargerId)
        {
            return Installation.GetInstances()
                .SelectMany(i => i.Chargers)
                .Single(c => c.Id == chargerId);
        }

        [HttpGet("{installationId}")]
        [EnableCors("MyPolicy")]
        public IEnumerable<Charger> Get(Guid installationId)
        {
            var installation = GetInstallation(installationId);

            return installation != null
                ? installation.Chargers
                : null;
        }

        [HttpPost("{id}/connect")]
        [EnableCors("MyPolicy")]
        public Charger Connect(Guid id)
        {
            var charger = GetCharger(id);
            charger.ConnectVehicle();
            return charger;
        }

        [HttpPost("{id}/disconnect")]
        [EnableCors("MyPolicy")]
        public Charger Disconnect(Guid id)
        {
            var charger = GetCharger(id);
            charger.DisconnectVehicle();
            return charger;
        }
    }
}
