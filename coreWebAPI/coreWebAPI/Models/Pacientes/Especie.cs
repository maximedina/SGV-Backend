using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Pacientes
{
    public class Especie
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Inactivo { get; set; }

        private CoreDbContext _coreDbContext;

        public Especie() { }

        public Especie(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Especie especie)
        {
            if (Validate(especie.Nombre, especie.Id))
            {
                throw new ValidationException("Ya existe una especie con el nombre " + especie.Nombre);
            }
            else
            {
                especie.Inactivo = false;
                _coreDbContext.Entry(especie).State = EntityState.Added;
                _coreDbContext.Especies.Add(especie);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Especie especie)
        {
            if (Validate(especie.Nombre, especie.Id))
            {
                throw new ValidationException("Ya existe una especie con el nombre "+especie.Nombre);
            }
            else
            {
                _coreDbContext.Update(especie);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var especie = _coreDbContext.Especies.Where(hq => hq.Id == id).FirstOrDefault();
            if (especie != null)
            {
                especie.Inactivo = true;
                _coreDbContext.Especies.Update(especie);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Especie> List(bool incluirInactivos, string nombre = "", string descripcion = "") =>
            _coreDbContext.Especies
                .Where(hs => (hs.Nombre.Contains(nombre) || nombre == null)
                            && (hs.Descripcion.Contains(descripcion) || descripcion == null)   
                            && (hs.Inactivo == incluirInactivos || incluirInactivos))
            .ToList();

        public bool Validate(string nombre, int id) => _coreDbContext.Especies.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}