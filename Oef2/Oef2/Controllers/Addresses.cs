using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Oef2.Models;
using Oef2.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Oef2.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class Addresses : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<Adres> Get()
        {
            return Ok(Database.Addresses);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] List<Models.Adres> addresses)
        {
            Database.Addresses.Clear();
            Database.Addresses = addresses;
        }

    }
}
