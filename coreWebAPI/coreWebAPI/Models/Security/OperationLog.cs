using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Models.Security
{
    public class OperationLog
    {
        public int OperationLogId { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public User User { get; set; }
        public Permission Permission { get; set; }
        public DateTime OperationDateTime { get; set; }
        public string Description { get; set; }

        private CoreDbContext _coreDbContext;

        public OperationLog() { }

        public OperationLog(CoreDbContext myDbContext)
        {
            _coreDbContext = myDbContext;
        }

        public void Log(int userId, string permissionName, string logText)
        {
            User logUser = _coreDbContext.Users.Where(obj => obj.UserId == userId).FirstOrDefault();
            Permission logPermission = _coreDbContext.Permissions.Where(obj => obj.Name.Equals(permissionName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if(logUser == null)
            {
                throw new Exception("Usuario especificado no existe en la base de datos.");
            }
            else if(logPermission == null)
            {
                throw new Exception("Permiso especificado no existe en la base de datos.");
            }

            User = logUser;
            Permission = logPermission;
            Description = logText;
            OperationDateTime = DateTime.Now;

            _coreDbContext.Entry(this).State = EntityState.Added;
            _coreDbContext.SaveChanges();
        }

    }
}