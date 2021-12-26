using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Turnos;
using MOM.Core.WebAPI.Controllers.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnosController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Turno _turno;
        public TurnosController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _turno = new Turno(coreDbContext);
        }
        [HttpPost("crear")]
        //[SecurityExclusion]
        public ActionResult crear(Turno turno)
        {
            try
            {
                //turno.end = turno.end.AddHours(-6);
                //turno.start = turno.start.AddHours(-6);
                if (turno.id > 0)
                {
                    _turno.Update(turno);
                }
                else
                {
                    _turno.Add(turno);
                }
                return Ok();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("eliminar")]
        //[SecurityExclusion]
        public ActionResult eliminar(Turno turno)
        {
            try
            {
                _turno.Delete(turno);
                return Ok();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("Get/{id}")]
        //[SecurityExclusion]
        public ActionResult<Turno> Get(Int64 id)
        {
            return _turno.Get(id);
        }
        [HttpGet("listar")]
        //[SecurityExclusion]
        public ActionResult<List<Turno>> consultar()
        {
            return _turno.listar();
        }
        [HttpGet("paciente/{id}")]
        //[SecurityExclusion]
        public ActionResult<List<Turno>> porPaciente(int id)
        {
            return _turno.porPaciente(id);
        }
    }
}
