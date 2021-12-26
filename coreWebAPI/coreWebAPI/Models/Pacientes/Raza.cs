using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Pacientes
{
    public class Raza
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Inactivo { get; set; }
        public Especie Especie { get; set; }

        private CoreDbContext _coreDbContext;

        public Raza() { }

        public Raza(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Raza raza)
        {
            if (Validate(raza.Nombre, raza.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + raza.Nombre);
            }
            else
            {
                raza.Inactivo = false;
                _coreDbContext.Entry(raza).State = EntityState.Added;
                _coreDbContext.Razas.Add(raza);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Raza raza)
        {
            if (Validate(raza.Nombre, raza.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + raza.Nombre);
            }
            else
            {
                _coreDbContext.Update(raza);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var raza = _coreDbContext.Razas.Where(hq => hq.Id == id).FirstOrDefault();
            if (raza != null)
            {
                raza.Inactivo = true;
                _coreDbContext.Razas.Update(raza);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Raza> List(bool incluirInactivos, string nombre = "", string descripcion = "", int idEspecie = 0)
        {
            var especie = _coreDbContext.Especies.Where(p => p.Id == idEspecie).FirstOrDefault();

            var razas = _coreDbContext.Razas
            .Include(r => r.Especie)
            .Where(r => (r.Nombre.Contains(nombre) || nombre == null)
                        && (r.Descripcion.Contains(descripcion) || descripcion == null)
                        && (r.Especie == especie || especie == null)
                        && (r.Inactivo == incluirInactivos || incluirInactivos)).ToList();

            return razas;
        }

        public bool Validate(string nombre, int id) => _coreDbContext.Razas.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}




//public List<Especie> List(bool incluirInactivos, string nombre = "", string descripcion = "", int idGrupo = 0)
//            {
//    var grupo = _dbContext.Grupos.Where(p => p.Id == idGrupo).FirstOrDefault();
//        _coreDbContext.Especies
//        .Where(hs => (hs.Nombre.Contains(nombre) || nombre == null)
//                    && (hs.Descripcion.Contains(descripcion) || descripcion == null)
//                    && (hs.Inactivo == incluirInactivos || incluirInactivos))
//    .ToList();
//    }
//        public List<Especie> ListByValue(bool incluirInactivos, string nombre= "", string descripcion = "", int idGrupo = 0)
//        {
//            var grupo = _dbContext.Grupos.Where(p => p.Id == idGrupo).FirstOrDefault();

//            var especies = _dbContext.Especies  
//            .Include(u => u.Grupo)
//            .Where(u => (u.Nombre.Contains(nombre) || nombre == null)
//                        && (u.Descripcion.Contains(descripcion) || descripcion == null)
//                        && (u.Grupo == grupo || grupo == null)
//                        && u.Inactivo == incluirInactivos || incluirInactivos).ToList();

//            return especies;
//        }

//        public bool Validate(string nombre, int id) => _dbContext.Especies.Where(x => x.Nombre.Equals(nombre) && x.Id != id).Any();
//    }
//}