using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.WebAPI.Models.Practicas;
using MOM.Core.WebAPI.Models.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticasController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private RegistroPractica _regpractica;
        private Practica _practica;
        public PracticasController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _regpractica = new RegistroPractica(coreDbContext);
            _practica = new Practica(coreDbContext);
        }
        [HttpPost("crear")]
        //[SecurityExclusion]
        public ActionResult<Venta> crear(RegistroPractica practica)
        {
            try
            {
                return _regpractica.Add(practica);
                //return Ok();
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
        /*[HttpGet("listar")]
        //[SecurityExclusion]
        public ActionResult<List<RegistroPractica>> listar()
        {
            return _regpractica.listar();
        }*/

        [HttpGet("Get/{id}")]
        //[SecurityExclusion]
        public ActionResult<List<Practica>> Get(int id)
        {
            //return _regpractica.PracticasPaciente(id);
            return _practica.PracticasPaciente(id);
        }

        [HttpPost("Add")]
        public ActionResult<Venta> Add(Practica practica)
        {
            try
            {
                return _practica.add(practica);
                //return Ok();
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
    }
}
