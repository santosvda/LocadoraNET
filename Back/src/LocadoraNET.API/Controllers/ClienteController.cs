using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LocadoraNET.API.Models;

namespace LocadoraNET.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {

        public ClienteController()
        {
        }

        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            return new Cliente[] {
                new Cliente() {ClienteId = 1}
            };
        }
    }
}
