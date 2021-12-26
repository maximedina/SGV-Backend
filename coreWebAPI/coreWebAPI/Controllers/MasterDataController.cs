using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MOM.Core.Db;
using MOM.Core.Models.Shared;
using MOM.Core.WebAPI.Controllers.Interceptor;
using System;
using System.Collections.Generic;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class MasterDataController : ControllerBase
    {
        private DataTypes _dataType;
        private Uom _uom;

		private IConfiguration _configuration;


		public MasterDataController(CoreDbContext dbContext, IConfiguration Configuration)
        {
            _dataType = new DataTypes(dbContext);
            _uom = new Uom(dbContext);

			_configuration = Configuration;
        }

        #region DataType
        [HttpGet("DataType/Get")]
        public ActionResult<List<DataTypes>> GetDataType()
        {
			try
			{
				return _dataType.List(_configuration);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message); 
			}
        }

		[HttpGet("GetEntities")]
		public ActionResult GetEntities(string entity)
		{
			try
			{
				return Ok(_dataType.ListEntities(entity, _configuration));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		#endregion

		#region Uom
		[HttpGet("Uom/Get")]
        public ActionResult<List<Uom>> GetUom()
        {
			try
			{
				return _uom.List();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }
        #endregion
    }
}
