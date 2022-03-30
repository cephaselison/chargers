using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class InstallationsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [EnableCors("MyPolicy")]
        public IEnumerable<Installation> Get()
        {
            return Installation.GetInstances();
        }
    }
}
