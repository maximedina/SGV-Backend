using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Models.Security
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SystemModule SystemModule { get; set; }
        [JsonIgnore]
        public List<ProfilePermission> ProfilePermissions { get; set; } = new List<ProfilePermission>();

        private CoreDbContext _dbContext;

        public Permission(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Permission> ListByModule(SystemModule module) => _dbContext.Permissions.Include(p => p.SystemModule).Where(p => p.SystemModule == module).ToList();

        public List<Permission> ListByProfile(Profile profile) => _dbContext.ProfilePermissions
                                                                .Include(r => r.Permission)
                                                                .Where(r => r.Profile == profile)
                                                                .Select(r => r.Permission)
                                                                .ToList();

        public List<Permission> ListAvailablePermissions(int profileId, int moduleId)
        {
            var permission = _dbContext.Permissions.Where(obj => obj.SystemModule.SystemModuleId == moduleId).ToList();
            var profilePermission = _dbContext.ProfilePermissions
                .Include(p => p.Permission)
                .Where(pp => pp.Profile.ProfileId == profileId)
                .Select(p => p.Permission).ToList();

            foreach (var perm in profilePermission)
            {
                permission.Remove(perm);
            }

            return permission;
        }

        public List<SystemModule> GetByPermissionId(int permissionId) => _dbContext.Permissions
                                                .Include(p => p.SystemModule)
                                                .Where(p => p.PermissionId == permissionId)
                                                .Select(p => p.SystemModule).ToList();
    }
}
