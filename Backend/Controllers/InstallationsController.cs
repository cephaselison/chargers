using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallationsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Installation> Get()
        {
            return Installation.GetInstances();
        }
    }
}
