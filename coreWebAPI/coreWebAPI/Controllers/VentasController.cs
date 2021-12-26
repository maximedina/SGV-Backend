using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.WebAPI.Models.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Venta _venta;
        public VentasController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _venta = new Venta(coreDbContext);
        }
        [HttpPost("registrar")]
        //[SecurityExclusion]
        public ActionResult registrar(Venta venta)
        {
            try
            {
                _venta.registrar(venta);
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

        [HttpGet("consultar")]
        //[SecurityExclusion]
        public ActionResult<List<Venta>> consultar(String fecIni,String fecFin)
        {
            return _venta.buscar(fecIni,fecFin);
        }

        [HttpGet("Get/{id}")]
        //[SecurityExclusion]
        public ActionResult<Venta> Get(Int64 id)
        {
            return _venta.Get(id);
        }
    }
}
