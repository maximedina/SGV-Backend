using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MOM.Core.Models.Security
{
    public class SystemSetting
    {
        public int SystemSettingId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        [ForeignKey("SystemModuleId")]
        public SystemModule Module { get; set; }
        public bool IsDeleted { get; private set; }

        private CoreDbContext _dbContext;

        public SystemSetting() { }

        public SystemSetting(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(SystemSetting systemSetting)
        {
            if (Validate(systemSetting.Key, systemSetting.Value, systemSetting.Module, systemSetting.SystemSettingId))
            {
                throw new Exception("Valores ya registrados para estos parámetros!");
            }

            systemSetting.IsDeleted = false;
            _dbContext.Entry(systemSetting).State = EntityState.Added;
            _dbContext.Add(systemSetting);
            _dbContext.SaveChanges();
        }

        public void Update(SystemSetting systemSetting)
        {
            if (Validate(systemSetting.Key, systemSetting.Value, systemSetting.Module, systemSetting.SystemSettingId))
            {
                throw new Exception("Valores ya registrados para estos parámetros!");
            }

            _dbContext.Entry(systemSetting).State = EntityState.Added;
            _dbContext.Update(systemSetting);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var systemSetting = _dbContext.SystemSettings.FirstOrDefault(u => u.SystemSettingId == id);
            if (systemSetting != null)
            {
                systemSetting.IsDeleted = true;
                _dbContext.SystemSettings.Update(systemSetting);
                _dbContext.SaveChanges();
            }
        }

        public List<SystemSetting> List(string keyName = "", int moduleId = 0, bool includeDeleted = false) =>
            _dbContext.SystemSettings
                .Include(obj => obj.Module)
                .Where(obj => ((obj.Key.Contains(keyName) || keyName == null)
                            && (moduleId == 0 || obj.Module.SystemModuleId == moduleId)
                            && (obj.IsDeleted == includeDeleted || includeDeleted)))
                .ToList();

        public SystemSetting GetByName(string keyName)
        {
            SystemSetting key = _dbContext.SystemSettings.Where(obj => obj.Key.Equals(keyName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if(key == null)
            {
                throw new Exception("Clave [" + keyName + "] no fue encontrada en la tabla de configuraciones.");
            }
            return key;
        }

        public bool Validate(string key, string value, SystemModule systemModule, long id)
            => _dbContext.SystemSettings.Where(x => x.Key.Equals(key) && x.Value.Equals(value) && x.Module == systemModule && x.SystemSettingId != id).Any();

		public List<SystemSetting> ListReports()
		{
			return _dbContext.SystemSettings.Where(x => x.Key.Contains("REPORT")).ToList();
		}
	}
}
