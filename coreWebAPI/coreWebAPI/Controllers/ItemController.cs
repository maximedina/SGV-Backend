using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MOM.Core.Db;
using System;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.Models.Items;

namespace MOM.Core.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class ItemController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Tipo _tipo;
        private Grupo _grupo;
        private Rubro _rubro;
        private Categoria _categoria;
        private Item _item;


        public ItemController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _tipo = new Tipo(coreDbContext);
            _grupo = new Grupo(coreDbContext);
            _rubro = new Rubro(coreDbContext);
            _categoria = new Categoria(coreDbContext);
            _item = new Item(coreDbContext);
        }

        #region Item

        [HttpPost("Add")]
        public ActionResult ItemAdd([FromBody] Item item)
        {
            try
            {
                _item.Add(item);
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

        [HttpPost("Update")]
        public ActionResult ItemUpdate([FromBody] Item item)
        {
            try
            {
                _item.Update(item);
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

        [HttpPost("Delete")]
        public ActionResult ItemDelete([FromBody] int id)
        {
            try
            {
                _item.Delete(id);
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

        [HttpGet("Get")]
        public List<Item> ListarItems(
                                bool incluirInactivos,
                                string codigo,
                                string nombre,
                                string descripcion,
                                int idCategoria,
                                int idProveedor,
                                int idTipo)
        {
            return _item.List(incluirInactivos, codigo, nombre, descripcion, idCategoria, idProveedor, idTipo);
        }

        [HttpGet("Practicas")]
        public List<Item> ListarPracticas()
        {
            return _item.ListPracticas();
        }

        [HttpGet("buscar")]
        //[SecurityExclusion]
        public List<Item> Buscar(bool? incluirInactivos,
            string nombre,
            int? Categoria,
            int? Proveedor,
            int? Grupo,
            int? Rubro,
            int? Tipo)
        {
            return _item.Buscar(incluirInactivos,nombre,Categoria,Proveedor,Grupo,Rubro,Tipo);
        }

        #endregion

        #region Tipo

        [HttpPost("Tipo/Add")]
        public ActionResult TipoAdd([FromBody] Tipo tipo)
        {
            try
            {
                _tipo.Add(tipo);
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

        [HttpPost("Tipo/Update")]
        public ActionResult TipoUpdate([FromBody] Tipo tipo)
        {
            try
            {
                _tipo.Update(tipo);
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

        [HttpPost("Tipo/Delete")]
        public ActionResult TipoDelete([FromBody] int id)
        {
            try
            {
                _tipo.Delete(id);
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

        [HttpGet("Tipo/Get")]
        public List<Tipo> ListarTipos(bool incluirInactivos, string nombre, string descripcion)
        {
            return _tipo.List(incluirInactivos, nombre, descripcion);
        }

        #endregion

        #region Grupo

        [HttpPost("Grupo/Add")]
        public ActionResult GrupoAdd([FromBody] Grupo grupo)
        {
            try
            {
                _grupo.Add(grupo);
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

        [HttpPost("Grupo/Update")]
        public ActionResult GrupoUpdate([FromBody] Grupo grupo)
        {
            try
            {
                _grupo.Update(grupo);
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

        [HttpPost("Grupo/Delete")]
        public ActionResult GrupoDelete([FromBody] int id)
        {
            try
            {
                _grupo.Delete(id);
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

        [HttpGet("Grupo/Get")]
        public List<Grupo> ListarGrupos(bool incluirInactivos, string nombre, string descripcion)
        {
            return _grupo.List(incluirInactivos, nombre, descripcion);
        }

        #endregion

        #region Rubro

        [HttpGet("Rubro/Get")]
        public List<Rubro> ListarRubros(bool incluirInactivos, string nombre, string descripcion, int idGrupo)
        {
            return _rubro.List(incluirInactivos, nombre, descripcion, idGrupo);
        }


        //[HttpGet("Rubro/Get")]
        //public ActionResult<List<Rubro>> GetAllRubros()
        //{
        //    try
        //    {
        //        return _rubro.ListAll();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet("Rubro/GetByValue")]
        //public ActionResult<List<Rubro>> GetByValue(bool incluirInactivos, string nombre, string descripcion, int idGrupo)
        //{
        //    try
        //    {
        //        return _rubro.ListByValue(incluirInactivos, nombre, descripcion, idGrupo);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPost("Rubro/Add")]
        public ActionResult AddRubro([FromBody] Rubro rubro)
        {
            try
            {
                _rubro.Add(rubro);
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

        [HttpPost("Rubro/Update")]
        public ActionResult UpdateRubro([FromBody] Rubro rubro)
        {
            try
            {
                _rubro.Update(rubro);
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

        [HttpPost("Rubro/Delete")]
        public ActionResult DeleteRubro([FromBody] int id)
        {
            try
            {
                _rubro.Delete(id);
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

        #region Categoria

        [HttpGet("Categoria/Get")]
        public List<Categoria> ListarCategorias(bool incluirInactivos, string nombre, string descripcion, int idRubro)
        {
            return _categoria.List(incluirInactivos, nombre, descripcion, idRubro);
        }


        //[HttpGet("Rubro/Get")]
        //public ActionResult<List<Rubro>> GetAllRubros()
        //{
        //    try
        //    {
        //        return _rubro.ListAll();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet("Rubro/GetByValue")]
        //public ActionResult<List<Rubro>> GetByValue(bool incluirInactivos, string nombre, string descripcion, int idGrupo)
        //{
        //    try
        //    {
        //        return _rubro.ListByValue(incluirInactivos, nombre, descripcion, idGrupo);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPost("Categoria/Add")]
        public ActionResult AddCategoria([FromBody] Categoria categoria)
        {
            try
            {
                _categoria.Add(categoria);
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

        [HttpPost("Categoria/Update")]
        public ActionResult UpdateCategoria([FromBody] Categoria categoria)
        {
            try
            {
                _categoria.Update(categoria);
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

        [HttpPost("Categoria/Delete")]
        public ActionResult DeleteCategoria([FromBody] int id)
        {
            try
            {
                _categoria.Delete(id);
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