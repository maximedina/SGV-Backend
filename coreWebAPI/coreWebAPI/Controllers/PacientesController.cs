using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOM.Core.Db;
using MOM.Core.Models.Pacientes;
using MOM.Core.WebAPI.Controllers.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Paciente _paciente;
        public PacientesController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _paciente = new Paciente(coreDbContext);
        }
        [HttpGet("listar")]
        //[SecurityExclusion]
        public ActionResult<List<Paciente>> consultar(String fecIni, String fecFin)
        {
            return _paciente.Listar();
        }
    }
}
