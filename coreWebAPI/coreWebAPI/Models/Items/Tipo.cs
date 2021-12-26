using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Items
{
    public class Tipo
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Inactivo { get; set; }

        private CoreDbContext _coreDbContext;

        public Tipo() { }

        public Tipo(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Tipo tipo)
        {
            if (Validate(tipo.Nombre, tipo.Id))
            {
                throw new ValidationException("Ya existe un tipo con el nombre " + tipo.Nombre);
            }
            else
            {
                tipo.Inactivo = false;
                _coreDbContext.Entry(tipo).State = EntityState.Added;
                _coreDbContext.Tipos.Add(tipo);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Tipo tipo)
        {
            if (Validate(tipo.Nombre, tipo.Id))
            {
                throw new ValidationException("Ya existe un tipo con el nombre "+tipo.Nombre);
            }
            else
            {
                _coreDbContext.Update(tipo);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var tipo = _coreDbContext.Tipos.Where(hq => hq.Id == id).FirstOrDefault();
            if (tipo != null)
            {
                tipo.Inactivo = true;
                _coreDbContext.Tipos.Update(tipo);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Tipo> List(bool incluirInactivos, string nombre = "", string descripcion = "") =>
            _coreDbContext.Tipos
                .Where(hs => (hs.Nombre.Contains(nombre) || nombre == null)
                            && (hs.Descripcion.Contains(descripcion) || descripcion == null)   
                            && (hs.Inactivo == incluirInactivos || incluirInactivos))
            .ToList();

        public bool Validate(string nombre, int id) => _coreDbContext.Tipos.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}
