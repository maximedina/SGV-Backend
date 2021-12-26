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
    [ApiController]
    [Route("api/[Controller]")]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class CajasController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Caja _caja;

        public CajasController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _caja = new Caja(coreDbContext);
        }
        [HttpPost("abrir")]
        //[SecurityExclusion]
        public ActionResult Add([FromBody] Caja caja)
        {
            try
            {
                _caja.Add(caja);
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

        [HttpPost("cerrar")]
        //[SecurityExclusion]
        public ActionResult Cerrar([FromBody] Caja caja)
        {
            try
            {
                _caja.cerrar(caja);
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
        [HttpGet("buscar/{userId}")]
        //[SecurityExclusion]
        public ActionResult<Caja> Get(int userId)
        {
            try
            {
                return _caja.consultar(userId);
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
        //[HttpPost("Update")]
        //public ActionResult ItemUpdate([FromBody] Item item)
        //{
        //    try
        //    {
        //        _item.Update(item);
        //        return Ok();
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost("Delete")]
        //public ActionResult ItemDelete([FromBody] int id)
        //{
        //    try
        //    {
        //        _item.Delete(id);
        //        return Ok();
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet("Get")]
        //public List<Item> ListarItems(
        //                        bool incluirInactivos,
        //                        string codigo,
        //                        string nombre,
        //                        string descripcion,
        //                        int idCategoria,
        //                        int idProveedor,
        //                        int idTipo)
        //{
        //    return _item.List(incluirInactivos, codigo, nombre, descripcion, idCategoria, idProveedor, idTipo);
        //}
    }
}
