using MOM.Core.Db;
using MOM.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Practicas
{
    public class DetallePractica
    {
        
        [Key]
        public Int64 idDetalle { get; set; }
        public Int64 idPractica { get; set; }
        public int itemId { get; set; }
        public double cantidad { get; set; }
        public double precio { get; set; }
        public double descuento { get; set; }
        public double importe { get; set; }
        [ForeignKey("itemId")]
        public Item Item { get; set; }
        [ForeignKey("idPractica")]
        public RegistroPractica practica { get; set; }
        private CoreDbContext _coreDbContext;
        public DetallePractica(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
    }
}
