using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MOM.Core.WebAPI.Models.Proveedores;

namespace MOM.Core.Models.Items
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public double PrecioCosto { get; set; }
        public double PorcentajePrecio { get; set; }
        public double Iva { get; set; }
        public string Presentacion { get; set; }
        public double Stock { get; set; }
        public double StockMinimo { get; set; }
        public bool? Inactivo { get; set; }
        public Categoria Categoria { get; set; }
        public Proveedor Proveedor { get; set; }
        public Tipo Tipo { get; set; }
        public int categoriaId { get; set; }
        public int proveedorId { get; set; }
        public int tipoId { get; set; }
        //[NotMapped]
        public string Identificador => $"{Codigo} - {Nombre}";
        public double PrecioVenta {
            get
            {
                return PrecioCosto * (1 + (PorcentajePrecio / 100)) * (1 + (Iva / 100));
            }   
        }

        private CoreDbContext _coreDbContext;

        public Item() {

        }

        public Item(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Item item)
        {
            if (Validate(item.Nombre, item.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + item.Nombre);
            }
            else
            {
                item.Inactivo = false;
                _coreDbContext.Entry(item).State = EntityState.Added;
                _coreDbContext.Items.Add(item);
                _coreDbContext.SaveChanges();
            }
        }

        public void Update(Item item)
        {
            if (Validate(item.Nombre, item.Id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + item.Nombre);
            }
            else
            {
                _coreDbContext.Update(item);
                _coreDbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var item = _coreDbContext.Items.Where(hq => hq.Id == id).FirstOrDefault();
            if (item != null)
            {
                item.Inactivo = true;
                _coreDbContext.Items.Update(item);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Item> List(
            bool incluirInactivos,
            string codigo = "",
            string nombre = "",
            string descripcion = "",
            int idCategoria = 0,
            int idProveedor = 0,
            int idTipo=0
            )
       {
            var categoria = _coreDbContext.Categorias.Where(p => p.Id == idCategoria).FirstOrDefault();
            var proveedor = _coreDbContext.Proveedores.Where(p => p.Id == idProveedor).FirstOrDefault();
            var tipo = _coreDbContext.Tipos.Where(p => p.Id == idTipo).FirstOrDefault();

            var items = _coreDbContext.Items
            .Include(r => r.Categoria)
            .Include(r => r.Proveedor)
            .Include(r => r.Tipo)
            .Where(r => (r.Nombre.Contains(nombre) || nombre == null)
                        && (r.Codigo.Contains(codigo) || codigo == null)
                        && (r.Descripcion.Contains(descripcion) || descripcion == null)
                        && (r.Categoria == categoria || categoria == null)
                        && (r.Proveedor == proveedor || proveedor == null)
                        && (r.Tipo == tipo || tipo == null)
                        && (r.Inactivo == incluirInactivos || incluirInactivos)).ToList();

            return items;
        }

        public List<Item> Buscar(
            bool? incluirInactivos,
            string nombre = "",
            int? idCategoria = 0,
            int? idProveedor = 0,
            int? idGrupo = 0,
            int? idRubro = 0,
            int? Tipo = 0
            )
        {

            var categoria = _coreDbContext.Categorias.Where(p => p.Id == idCategoria).FirstOrDefault();
            var proveedor = _coreDbContext.Proveedores.Where(p => p.Id == idProveedor).FirstOrDefault();
            var grupos = _coreDbContext.Grupos.Where(p => p.Id == idGrupo).FirstOrDefault();
            
            
            List<Item> items = _coreDbContext.Items.Include(i=>i.Proveedor).Include(i=>i.Categoria).ThenInclude(c=>c.Rubro).ToList();
            if (incluirInactivos.HasValue)
            {
                if (!incluirInactivos.Value)
                {
                    items = items.Where(i => i.Inactivo == false).ToList();
                }
            }
            if(nombre != null)
            {
                items = items.Where(i => i.Nombre.Contains(nombre)).ToList();
            }
            if(idCategoria.HasValue)
            {
                items = items.Where(i => i.categoriaId == idCategoria).ToList();
            }
            if (idProveedor.HasValue)
            {
                items = items.Where(i => i.proveedorId == idProveedor).ToList();
            }
            if (idGrupo.HasValue)
            {
                items = items.Where(i => i.Categoria.Rubro.grupoId == idGrupo).ToList();
            }
            if (idRubro.HasValue)
            {
                items = items.Where(i => i.Categoria.Rubro.Id == idRubro).ToList();
            }
            if (Tipo.HasValue)
            {
                items = items.Where(i => i.tipoId == Tipo).ToList();
            }
            return items;
        }

        public List<Item> ListPracticas()
        {

            List<Item> items = _coreDbContext.Items.Where(i=>i.tipoId == 1 ).Include(i => i.Proveedor).Include(i => i.Categoria).ThenInclude(c => c.Rubro).ToList();
            return items;
        }

        public bool Validate(string nombre, int id) => _coreDbContext.Items.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

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