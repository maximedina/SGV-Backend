using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Localizacion
{
    public class Ciudad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool? Inactivo { get; set; }
        public Provincia Provincia { get; set; }

        private CoreDbContext _coreDbContext;

        public Ciudad() { }

        public Ciudad(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Ciudad ciudad)
        {
            if (Validate(ciudad.Nombre, ciudad.Id))
            {
                throw new ValidationException("Ya existe una ciudad con el nombre " + ciudad.Nombre);
            }
            else
            {
                ciudad.Inactivo = false;
                _coreDbContext.Entry(ciudad).State = EntityState.Added;
                _coreDbContext.Ciudades.Add(ciudad);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Ciudad ciudad)
        {
            if (Validate(ciudad.Nombre, ciudad.Id))
            {
                throw new ValidationException("Ya existe una ciudad con el nombre " + ciudad.Nombre);
            }
            else
            {
                _coreDbContext.Update(ciudad);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var ciudad = _coreDbContext.Ciudades.Where(hq => hq.Id == id).FirstOrDefault();
            if (ciudad != null)
            {
                ciudad.Inactivo = true;
                _coreDbContext.Ciudades.Update(ciudad);
                _coreDbContext.SaveChanges();
            }
        }

        //public List<Ciudad> List(bool incluirInactivos, string nombre = "", int idProvincia = 0)
        //{
        //    var provincia = _coreDbContext.Provincias.Where(p => p.Id == idProvincia).FirstOrDefault();

        //    var rubros = _coreDbContext.Ciudades
        //    .Include(r => r.Provincia)
        //    .Where(r => (r.Nombre.Contains(nombre) || nombre == null)
        //                && (r.Provincia == provincia || provincia == null)
        //                && (r.Inactivo == incluirInactivos || incluirInactivos)).ToList();

        //    return rubros;
        //}



        public List<Ciudad> ListAll() => _coreDbContext.Ciudades
                                .Include(u => u.Provincia)
                                .Where(u => (bool)!u.Inactivo).OrderBy(u => u.Nombre)
                                .ToList();

        public List<Ciudad> ListByValue(bool incluirInactivos, string nombre= "", int idProvincia = 0)
        {
            var provincia = _coreDbContext.Provincias.Where(p => p.Id == idProvincia).FirstOrDefault();

            var ciudades = _coreDbContext.Ciudades
            .Include(u => u.Provincia)
            .Where(u => (u.Nombre.Contains(nombre) || nombre == null)
                        && (u.Provincia == provincia || provincia == null)
                        && u.Inactivo == incluirInactivos || incluirInactivos).ToList();

            return ciudades;
        }






        public bool Validate(string nombre, int id) => _coreDbContext.Ciudades.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

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