using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Security;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using MOM.Core.WebAPI.Util;

namespace MOM.Core.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class SecurityController : ControllerBase
    {
        private User _user;
        private Permission _permission;
        private Profile _profile;
        private SystemModule _systemModule;
        private OperationLog _operationLog;
        private readonly SecurityService security;

        public SecurityController(CoreDbContext dbContext, IServiceProvider services)
        {
            _user = new User(dbContext);
            _permission = new Permission(dbContext);
            _profile = new Profile(dbContext);
            _systemModule = new SystemModule(dbContext);
            _operationLog = new OperationLog(dbContext);

            security = services.GetService<SecurityService>();
        }

        #region User  

        [HttpPost("User/AuthenticateOnFrame")]
        [SecurityExclusion]
        public ActionResult<Session> AuthenticateOnFrame([FromForm] string login, [FromForm] string ipAddress, [FromForm] string checkSum, [FromForm] string deviceType)
        {
            try
            {
                if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(checkSum))
                {
                    var session = security.LoginOnFrame(login, ipAddress, checkSum, deviceType);
                    if (session != null && session.Token != null)
                    {
                        return session;
                    }

                    return Unauthorized();
                }
                else
                    return BadRequest(string.Format("Error iniciando Single Sign On. Inicie sesión nuevamente en el Portal MOM."));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("User/Authenticate")]
        [SecurityExclusion]
        public ActionResult<Session> Authenticate([FromForm] string login, [FromForm] string password, [FromForm] string deviceType)
        {
            try
            {
                if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                {
                    var session = security.Login(login, password, deviceType);
                    

                    if (session != null && session.Token != null)
                    {
                        return session;
                    }
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("User/Logout/{token}")]
        [SecurityExclusion]
        public ActionResult Logout(string token)
        {
            try
            {
                security.Logout(token);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("User/Get")]
        public ActionResult<List<User>> GetAllUsers()
        {
            try
            {
                return _user.ListAll();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("User/GetByValue")]
        public ActionResult<List<User>> GetByValue(bool includeDeleted, string login, string name, int profileId)
        {
            try
            {
                return _user.ListByValue(includeDeleted, login, name, profileId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("User/Add")]
        public ActionResult AddUser([FromBody] User user)
        {
            try
            {
                _user.Add(user);
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

        [HttpPost("User/Update")]
        public ActionResult UpdateUser([FromBody] User user)
        {
            try
            {
                _user.Update(user);
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

        [HttpPost("User/changePassword")]
        public ActionResult ChangeUserPassword([FromBody] User user, string passwordActual, string passwordNueva)
        {
            try
            {
                _user.ChangePassword(user, passwordActual, passwordNueva);
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

        [HttpPost("User/Delete")]
        public ActionResult DeleteUser([FromBody] int userId)
        {
            try
            {
                _user.Delete(userId);
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

        [HttpGet("User/buscar")]
        //[SecurityExclusion]
        public ActionResult<List<User>> Buscar(int? codigo,string nombre,string dni)
        {
            try
            {
                return _user.Buscar(codigo, nombre, dni);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region Profile

        [HttpGet("Profile/Get")]
        public ActionResult<List<Profile>> GetAllProfiles()
        {
            try
            {
                return _profile.ListAll();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Profile/GetByValues")]
        public ActionResult<List<Profile>> GetProfileByValue(string name, string description)
        {
            try
            {
                return _profile.ListByValues(name, description);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("Profile/AssociatePermission")]
        public ActionResult AssociatePermission([ModelBinder(BinderType = typeof(JsonModelBinder))] Profile profile, [ModelBinder(BinderType = typeof(JsonModelBinder))] Permission permission)
        {
            try
            {
                _profile.AssociatePermission(profile, permission);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Profile/DisassociatePermission")]
        public ActionResult DisassociatePermission([ModelBinder(BinderType = typeof(JsonModelBinder))] Profile profile, [ModelBinder(BinderType = typeof(JsonModelBinder))] Permission permission)
        {
            try
            {
                _profile.DisassociatePermission(profile, permission);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Permission

        [HttpGet("Permission/PerModule")]
        public ActionResult<List<Permission>> GetPermissionBySystemModule([FromBody] SystemModule systemModule)
        {
            try
            {
                return _permission.ListByModule(systemModule);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Permission/PerProfile")]
        public ActionResult<List<Permission>> GetPermissionByProfile([FromBody] Profile profile)
        {
            try
            {
                return _permission.ListByProfile(profile);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Permission/AvailablePermissions")]
        public ActionResult<List<Permission>> GetAvailablePermissionPerProfile(int profileId, int moduleId)
        {
            try
            {
                return _permission.ListAvailablePermissions(profileId, moduleId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region SystemModule

        [HttpGet("SystemModule/PerPermissionId")]
        public ActionResult<List<SystemModule>> GetModuleByPermission(int permissionId)
        {
            try
            {
                return _permission.GetByPermissionId(permissionId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("SystemModule/Get")]
        public ActionResult<List<SystemModule>> GetSystemModule()
        {
            try
            {
                return _systemModule.ListAll();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region OperationLog
        //TODO: Remover as chamadas a este método no Angular. Não existe um serviço web que gera log. Isso é responsabilidade do business
        [HttpPost("Log/Add")]
        public ActionResult AddLog([FromBody] OperationLog log)
        {
            try
            {
                _operationLog.Log(log.User.UserId, log.Permission.Name, log.Description);
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

        #endregion
    }
}
