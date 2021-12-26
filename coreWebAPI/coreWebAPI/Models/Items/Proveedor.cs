using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MOM.Core.Models.Localizacion;
using System;

namespace MOM.Core.Models.Items
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal CuentaCorriente { get; set; }
        public string Contacto { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public Ciudad Ciudad { get; set; }
        public string Observaciones { get; set; }
        public Boolean Inactivo { get; set; }


        private CoreDbContext _coreDbContext;

        public Proveedor() { }

        public Proveedor(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Proveedor proveedor)
        {
            if (Validate(proveedor.Nombre, proveedor.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + proveedor.Nombre);
            }
            else
            {
                proveedor.Inactivo = false;
                _coreDbContext.Entry(proveedor).State = EntityState.Added;
                _coreDbContext.Proveedores.Add(proveedor);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Proveedor proveedor)
        {
            if (Validate(proveedor.Nombre, proveedor.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + proveedor.Nombre);
            }
            else
            {
                _coreDbContext.Update(proveedor);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var proveedor = _coreDbContext.Proveedores.Where(hq => hq.Id == id).FirstOrDefault();
            if (proveedor != null)
            {
                proveedor.Inactivo = true;
                _coreDbContext.Proveedores.Update(proveedor);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Proveedor> List(bool incluirInactivos, string nombre = "", string email = "", int idCiudad = 0)
        {
            var ciudad = _coreDbContext.Ciudades.Where(p => p.Id == idCiudad).FirstOrDefault();

            var proveedores = _coreDbContext.Proveedores
            .Include(r => r.Ciudad)
            .Where(r => (r.Nombre.Contains(nombre) || nombre == null)
                        && (r.Email.Contains(email) || email == null)
                        && (r.Ciudad == ciudad || ciudad == null)
                        && (r.Inactivo == incluirInactivos || incluirInactivos)).ToList();

            return proveedores;
        }

        public List<Proveedor> listar()
        {
            var proveedores = _coreDbContext.Proveedores.Include(r => r.Ciudad).ToList();
            return proveedores;
        }



        public bool Validate(string nombre, int id) => _coreDbContext.Proveedores.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

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