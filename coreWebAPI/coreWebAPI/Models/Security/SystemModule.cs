using MOM.Core.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Models.Security
{
    public class SystemModule
    {
        public int SystemModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string EndPoint { get; set; }

        private CoreDbContext _dbContext;

        public SystemModule(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SystemModule> ListAll()
        {
            return _dbContext.SystemModules.ToList();
        }

        public SystemModule GetByPk(int id)
        {
            return _dbContext.SystemModules.Where(s => s.SystemModuleId == id).FirstOrDefault();
        }
    }
}
