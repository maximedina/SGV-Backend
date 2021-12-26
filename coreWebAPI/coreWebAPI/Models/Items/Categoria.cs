using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Items
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Inactivo { get; set; }
        public Rubro Rubro { get; set; }

        private CoreDbContext _coreDbContext;

        public Categoria() { }

        public Categoria(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Categoria categoria)
        {
            if (Validate(categoria.Nombre, categoria.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + categoria.Nombre);
            }
            else
            {
                categoria.Inactivo = false;
                _coreDbContext.Entry(categoria).State = EntityState.Added;
                _coreDbContext.Categorias.Add(categoria);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Categoria categoria)
        {
            if (Validate(categoria.Nombre, categoria.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + categoria.Nombre);
            }
            else
            {
                _coreDbContext.Update(categoria);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var categoria = _coreDbContext.Categorias.Where(hq => hq.Id == id).FirstOrDefault();
            if (categoria != null)
            {
                categoria.Inactivo = true;
                _coreDbContext.Categorias.Update(categoria);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Categoria> List(bool incluirInactivos, string nombre = "", string descripcion = "", int idRubro = 0)
        {
            var rubro = _coreDbContext.Rubros.Where(p => p.Id == idRubro).FirstOrDefault();

            var categorias = _coreDbContext.Categorias
            .Include(r => r.Rubro)
            .Where(r => (r.Nombre.Contains(nombre) || nombre == null)
                        && (r.Descripcion.Contains(descripcion) || descripcion == null)
                        && (r.Rubro == rubro || rubro == null)
                        && (r.Inactivo == incluirInactivos || incluirInactivos)).ToList();

            return categorias;
        }

        public bool Validate(string nombre, int id) => _coreDbContext.Categorias.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

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