using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.cajas
{
    public class MCCCliente
    {
        [Key]
        public int id { get; set; }
        public int idMovimientoCaja { get; set; }
        public int clienteId { get; set; }
        public decimal Importe { get; set; }
        public String TipoMovimiento { get; set; }
    }
}
