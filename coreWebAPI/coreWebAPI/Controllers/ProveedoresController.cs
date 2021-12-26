using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.WebAPI.Models.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        private Proveedor _proveedor;
        public ProveedoresController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _proveedor = new Proveedor(coreDbContext);
        }
        [HttpGet("listar")]
        //[SecurityExclusion]
        public ActionResult<List<Proveedor>> List()
        {
            try
            {
                return _proveedor.listar();
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
