using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOM.Core.Models.Items
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Inactivo { get; set; }
        public List<Rubro> Rubros { get; set; }

        private CoreDbContext _coreDbContext;

        public Grupo() { }

        public Grupo(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Grupo grupo)
        {
            if (Validate(grupo.Nombre, grupo.Id))
            {
                throw new ValidationException("Ya existe un grupo con el nombre " + grupo.Nombre);
            }
            else
            {
                grupo.Inactivo = false;
                _coreDbContext.Entry(grupo).State = EntityState.Added;
                _coreDbContext.Grupos.Add(grupo);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Grupo grupo)
        {
            if (Validate(grupo.Nombre, grupo.Id))
            {
                throw new ValidationException("Ya existe un grupo con el nombre "+grupo.Nombre);
            }
            else
            {
                _coreDbContext.Update(grupo);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var grupo = _coreDbContext.Grupos.Where(hq => hq.Id == id).FirstOrDefault();
            if (grupo != null)
            {
                grupo.Inactivo = true;
                _coreDbContext.Grupos.Update(grupo);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Grupo> List(bool incluirInactivos, string nombre = "", string descripcion = "") =>
            _coreDbContext.Grupos
                .Where(hs => (hs.Nombre.Contains(nombre) || nombre == null)
                            && (hs.Descripcion.Contains(descripcion) || descripcion == null)   
                            && (hs.Inactivo == incluirInactivos || incluirInactivos))
            .Include(g=>g.Rubros).ThenInclude(r=>r.Categorias)
            .ToList();

        public bool Validate(string nombre, int id) => _coreDbContext.Grupos.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}