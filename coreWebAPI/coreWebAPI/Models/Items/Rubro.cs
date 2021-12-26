using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Items
{
    public class Rubro
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Inactivo { get; set; }
        public int grupoId { get; set; }
        public Grupo Grupo { get; set; }

        public List<Categoria> Categorias { get; set; }

        private CoreDbContext _coreDbContext;

        public Rubro() { }

        public Rubro(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Rubro rubro)
        {
            if (Validate(rubro.Nombre, rubro.Id))
            {
                throw new ValidationException("Ya existe un rubro con el nombre " + rubro.Nombre);
            }
            else
            {
                rubro.Inactivo = false;
                _coreDbContext.Entry(rubro).State = EntityState.Added;
                _coreDbContext.Rubros.Add(rubro);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Rubro rubro)
        {
            if (Validate(rubro.Nombre, rubro.Id))
            {
                throw new ValidationException("Ya existe un rubro con el nombre " + rubro.Nombre);
            }
            else
            {
                _coreDbContext.Update(rubro);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var rubro = _coreDbContext.Rubros.Where(hq => hq.Id == id).FirstOrDefault();
            if (rubro != null)
            {
                rubro.Inactivo = true;
                _coreDbContext.Rubros.Update(rubro);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Rubro> List(bool incluirInactivos, string nombre = "", string descripcion = "", int idGrupo = 0)
            {
            var grupo = _coreDbContext.Grupos.Where(p => p.Id == idGrupo).FirstOrDefault();

            var rubros = _coreDbContext.Rubros
            .Include(r => r.Grupo)
            .Where(r => (r.Nombre.Contains(nombre) || nombre == null)
                        && (r.Descripcion.Contains(descripcion) || descripcion == null)
                        && (r.Grupo == grupo || grupo == null)
                        && (r.Inactivo == incluirInactivos || incluirInactivos)).ToList();

            return rubros;
        }



        public bool Validate(string nombre, int id) => _coreDbContext.Rubros.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}




//public List<Rubro> List(bool incluirInactivos, string nombre = "", string descripcion = "", int idGrupo = 0)
//            {
//    var grupo = _dbContext.Grupos.Where(p => p.Id == idGrupo).FirstOrDefault();
//        _coreDbContext.Rubros
//        .Where(hs => (hs.Nombre.Contains(nombre) || nombre == null)
//                    && (hs.Descripcion.Contains(descripcion) || descripcion == null)
//                    && (hs.Inactivo == incluirInactivos || incluirInactivos))
//    .ToList();
//    }
//        public List<Rubro> ListByValue(bool incluirInactivos, string nombre= "", string descripcion = "", int idGrupo = 0)
//        {
//            var grupo = _dbContext.Grupos.Where(p => p.Id == idGrupo).FirstOrDefault();

//            var rubros = _dbContext.Rubros  
//            .Include(u => u.Grupo)
//            .Where(u => (u.Nombre.Contains(nombre) || nombre == null)
//                        && (u.Descripcion.Contains(descripcion) || descripcion == null)
//                        && (u.Grupo == grupo || grupo == null)
//                        && u.Inactivo == incluirInactivos || incluirInactivos).ToList();

//            return rubros;
//        }

//        public bool Validate(string nombre, int id) => _dbContext.Rubros.Where(x => x.Nombre.Equals(nombre) && x.Id != id).Any();
//    }
//}