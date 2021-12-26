using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOM.Core.Db;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.WebAPI.Models.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Persona _persona;
        public PersonalController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _persona = new Persona(coreDbContext);
        }
        [HttpGet("listar")]
        //[SecurityExclusion]
        public ActionResult<List<Persona>> consultar()
        {
            return _persona.listar();
        }
    }
}
