using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.cajas
{
    public class MovCajaVta
    {
        public int id { get; set; }
        public int idMovimiento { get; set; }
        public Int64 idVenta { get; set; }
    }
}
