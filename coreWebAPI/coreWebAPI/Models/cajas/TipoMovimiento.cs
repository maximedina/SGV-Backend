using MOM.Core.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.cajas
{
    public class TipoMovimiento
    {
        [Key]
        public Int16 idMotivo { get; set; }
        public string Motivo { get; set; }
        private CoreDbContext _coreDbContext;
        public TipoMovimiento(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        public List<TipoMovimiento> listar()
        {
            return _coreDbContext.MotivoMovimiento.ToList();
        }
    }
}
