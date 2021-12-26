using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MOM.Core.Db;
using System;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.Models.Items;
using MOM.Core.Models.Localizacion;

namespace MOM.Core.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class LocalizacionController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Provincia _provincia;
        private Ciudad _ciudad;

        public LocalizacionController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _provincia = new Provincia(coreDbContext);
            _ciudad = new Ciudad(coreDbContext);
        }

        #region Provincia

        [HttpPost("Provincia/Add")]
        public ActionResult ProvinciaAdd([FromBody] Provincia provincia)
        {
            try
            {
                _provincia.Add(provincia);
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

        [HttpPost("Provincia/Update")]
        public ActionResult ProvinciaUpdate([FromBody] Provincia provincia)
        {
            try
            {
                _provincia.Update(provincia);
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

        [HttpPost("Provincia/Delete")]
        public ActionResult ProvinciaDelete([FromBody] int id)
        {
            try
            {
                _provincia.Delete(id);
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

        [HttpGet("Provincia/Get")]
        public List<Provincia> TipoGetByValue(bool incluirInactivos, string nombre, string descripcion)
        {
            return _provincia.List(incluirInactivos, nombre);
        }

        #endregion

        #region Ciudad

        //[HttpGet("Ciudad/Get")]
        //public List<Ciudad> List(bool incluirInactivos, string nombre, int idProvincia)
        //{
        //    return _ciudad.List(incluirInactivos, nombre, idProvincia);
        //}

        [HttpGet("Ciudad/Get")]
        public ActionResult<List<Ciudad>> GetAllCiudades()
        {
            try
            {
                return _ciudad.ListAll();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Ciudad/GetByValue")]
        public ActionResult<List<Ciudad>> GetByValue(bool incluirInactivos, string nombre, int idProvincia)
        {
            try
            {
                return _ciudad.ListByValue(incluirInactivos, nombre, idProvincia);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Ciudad/Add")]
        public ActionResult AddCiudad([FromBody] Ciudad ciudad)
        {
            try
            {
                _ciudad.Add(ciudad);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
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

        [HttpPost("Ciudad/Update")]
        public ActionResult UpdateCiudad([FromBody] Ciudad ciudad)
        {
            try
            {
                _ciudad.Update(ciudad);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
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

        [HttpPost("Ciudad/Delete")]
        public ActionResult DeleteCiudad([FromBody] int id)
        {
            try
            {
                _ciudad.Delete(id);
                return Ok();
            }
            catch (DbUpdateException d)
            {
                return BadRequest(d.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}