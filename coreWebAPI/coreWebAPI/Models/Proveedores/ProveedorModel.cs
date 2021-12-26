using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.Models.Localizacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Proveedores
{
    public class ProveedorModel
    {
		private CoreDbContext _coreDbContext;
		[Key]
		public int id { get; set; }
		public string nombre { get; set; }
		public decimal cuentaCorriente { get; set; }
		public string contacto { get; set; }
		public string domicilio { get; set; }
		public string telefono { get; set; }
		public string email { get; set; }
		public int ciudadId { get; set; }
		public string observaciones { get; set; }
		public Boolean inactivo { get; set; }
		public Ciudad Ciudad { get; set; }
		public ProveedorModel(CoreDbContext coreDbContext)
		{
			_coreDbContext = coreDbContext;
		}
		
	}
}
