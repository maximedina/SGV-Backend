using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Personal
{
    public class Persona
    {
        [Key]
        public int id { get; set; }
        public int legajo { get; set; }
        public string matricula { get; set; }
        public string observaciones { get; set; }
        public Boolean inactivo { get; set; }
        public int userId { get; set; }

        public User Usuario { get; set; }

        private CoreDbContext _coreDbContext;

        public Persona(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        public List<Persona> listar()
        {
            return _coreDbContext.Personal.Include(p=>p.Usuario).ToList();
        }
    }
}
