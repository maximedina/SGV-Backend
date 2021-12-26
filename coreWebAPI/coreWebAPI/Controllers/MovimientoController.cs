using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class MovimientoController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Movimiento _movimiento;
        private TipoMovimiento _tipo;
        public MovimientoController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _movimiento = new Movimiento(coreDbContext);
            _tipo = new TipoMovimiento(coreDbContext);
        }
        [HttpPost("registrar")]
        //[SecurityExclusion]
        public ActionResult Add([FromBody] Movimiento movimiento)
        {
            try
            {
                _movimiento.Add(movimiento);
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
        [HttpGet("list-tipo")]
        //[SecurityExclusion]
        public ActionResult<List<TipoMovimiento>> list_tpomov()
        {
            try
            {
                return _tipo.listar();
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
