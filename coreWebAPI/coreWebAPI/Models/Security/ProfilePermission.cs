using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Models.Security
{
    public class ProfilePermission
    {
        public int ProfilePermissionId { get; set; }
        public Profile Profile { get; set; }
        public Permission Permission { get; set; }

        private CoreDbContext _coreDbContext;
        
        public ProfilePermission(CoreDbContext dbContext)
        {
            _coreDbContext = dbContext;
        }

        public void Add(ProfilePermission profilePermission)
        {
            _coreDbContext.Entry(profilePermission).State = EntityState.Added;
            _coreDbContext.ProfilePermissions.Add(profilePermission);
            _coreDbContext.SaveChanges();
        }

        public void Remove(ProfilePermission profilePermission)
        {
            _coreDbContext.ProfilePermissions.Remove(profilePermission);
            _coreDbContext.SaveChanges();
        }
    }
}
