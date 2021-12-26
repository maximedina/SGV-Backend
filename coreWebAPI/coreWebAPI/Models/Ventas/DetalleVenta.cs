using MOM.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Ventas
{
    public class DetalleVenta
    {
        public Int64 id { get; set; }
        public Int64 ventaId { get; set; }
        public int itemId { get; set; }
        public double cantidad { get; set; }
        public double precio { get; set; }
        public double descuento { get; set; }
        public double importe { get; set; }
        public Item item { get; set; }

    }
}
