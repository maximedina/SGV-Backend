using Newtonsoft.Json;
using MOM.Core.Db;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MOM.Core.Models.Security
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        private CoreDbContext _dbContext;

        public Profile() { }

        public Profile(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Profile> ListAll()
        {
            return _dbContext.Profiles.ToList();
        }

        public List<Profile> ListByValues(string name = "", string description = "") =>
            _dbContext.Profiles.Where(p => (p.Name.Contains(name) || name == null)
                                           && (p.Description.Contains(description) || description == null)).ToList();

        public Profile GetByPk(int id)
        {
            return _dbContext.Profiles.FirstOrDefault(p => p.ProfileId == id);
        }

        public void AssociatePermission(Profile profile, Permission permission)
        {
            var profilePermission = new ProfilePermission(_dbContext);
            profilePermission.Profile = profile;
            profilePermission.Permission = permission;

            profilePermission.Add(profilePermission);
        }

        public void DisassociatePermission(Profile profile, Permission permission)
        {
            var pp = new ProfilePermission(_dbContext);
            var profilePermission = _dbContext.ProfilePermissions
                .Include(obj => obj.Profile)
                .Include(obj => obj.Permission)
                .Where(obj => obj.Profile == profile && obj.Permission == permission)
                .FirstOrDefault();
            if (profilePermission != null)
            {
                pp.Remove(profilePermission);
            }
        }
    }
}
