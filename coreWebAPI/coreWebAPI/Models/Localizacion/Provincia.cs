using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Localizacion
{
    public class Provincia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool? Inactivo { get; set; }

        private CoreDbContext _coreDbContext;

        public Provincia() { }

        public Provincia(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Provincia provincia)
        {
            if (Validate(provincia.Nombre, provincia.Id))
            {
                throw new ValidationException("Ya existe un provincia con el nombre " + provincia.Nombre);
            }
            else
            {
                provincia.Inactivo = false;
                _coreDbContext.Entry(provincia).State = EntityState.Added;
                _coreDbContext.Provincias.Add(provincia);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Provincia provincia)
        {
            if (Validate(provincia.Nombre, provincia.Id))
            {
                throw new ValidationException("Ya existe un provincia con el nombre " + provincia.Nombre);
            }
            else
            {
                _coreDbContext.Update(provincia);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var provincia = _coreDbContext.Provincias.Where(hq => hq.Id == id).FirstOrDefault();
            if (provincia != null)
            {
                provincia.Inactivo = true;
                _coreDbContext.Provincias.Update(provincia);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Provincia> List(bool incluirInactivos, string nombre = "") =>
            _coreDbContext.Provincias
                .Where(pr => (pr.Nombre.Contains(nombre) || nombre == null)
                            && (pr.Inactivo == incluirInactivos || incluirInactivos))
            .ToList();

        public bool Validate(string nombre, int id) => _coreDbContext.Provincias.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}
