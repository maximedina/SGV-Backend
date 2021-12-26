using Microsoft.AspNetCore.Mvc;
using MOM.Core.Db;
using MOM.Core.Models.Security;
using MOM.Core.WebAPI.Controllers.Interceptor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class UploadImageController : Controller
    {
        private CoreDbContext _dbContext;

        public UploadImageController(CoreDbContext coreDbContext)
        {
            _dbContext = coreDbContext;

        }

        [HttpPost("UploadImage")]
        public ActionResult UploadImage()
        {
            try
            {
                var systemSetting = new SystemSetting(_dbContext);
                var path = systemSetting.GetByName("UPLOADS_FOLDER_NAME").Value;
                var pathToSave = "Resources" + "\\" + path;
                var file = Request.Form.Files[0];



                if (file.Length > 0)
                {
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
