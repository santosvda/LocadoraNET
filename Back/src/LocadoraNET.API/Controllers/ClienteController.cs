﻿using System.Collections.Generic;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraNET.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly LocadoraNetContext _context;

        public ClienteController(LocadoraNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            return _context.Clientes;
        }
    }
}
