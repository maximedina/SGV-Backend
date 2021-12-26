using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Security;
using MOM.Core.WebAPI.Controllers.Interceptor;
using System;
using System.Collections.Generic;

namespace MOM.Core.WebAPI.Controllers
{
    [ApiController]
    [Route("api/SystemSetting")]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class SystemSettingController : ControllerBase
    {
        private SystemSetting _systemSetting;
        private SystemModule _systemModule;

        public SystemSettingController(CoreDbContext coreDbContext)
        {
            _systemSetting = new SystemSetting(coreDbContext);
            _systemModule = new SystemModule(coreDbContext);
        }
        
        [HttpGet("Get")]
        public ActionResult<List<SystemSetting>> Get(string keyName, int moduleId, bool includeDeleted)
        {
            return _systemSetting.List(keyName, moduleId, includeDeleted);
        }

        [HttpGet("GetSingleKey")]
        public ActionResult<SystemSetting> GetSingleKey(string keyName)
        {
            return _systemSetting.GetByName(keyName);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] SystemSetting systemSetting)
        {
            try
            {
                _systemSetting.Add(systemSetting);
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
        public ActionResult Update([FromBody] SystemSetting systemSetting)
        {
            try
            {
                _systemSetting.Update(systemSetting);
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
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                _systemSetting.Delete(id);
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

        [HttpGet("GetModules")]
        public ActionResult<List<SystemModule>> GetModules()
        {
            return _systemModule.ListAll();
        }

		[HttpGet("GetReports")]
		public ActionResult<List<SystemSetting>> GetReports()
		{
			try
			{
				return _systemSetting.ListReports();
			}
			catch (Exception exception)
			{
				return BadRequest(exception.Message);
			}
			
		}
	}
}
