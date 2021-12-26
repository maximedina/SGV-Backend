using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.Models.Pacientes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Practicas
{
    public class Desparacitacion
    {
        [Key]
        public Int64 idDesparacitacion { get; set; }
        public Int64 idPractica { get; set; }
        public int itemId { get; set; }
        public int PacienteId { get; set; }
        public DateTime Fecha { get; set; }
        [ForeignKey("itemId")]
        public Item item { get; set; }
        [ForeignKey("idPractica")]
        public RegistroPractica practica { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente paciente { get; set; }
        private CoreDbContext _coreDbContext;
        public Desparacitacion(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
    }
}
