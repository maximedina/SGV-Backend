using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Practicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Practicas
{
    public class DesparacitacionConfiguration : IEntityTypeConfiguration<Desparacitacion>
    {
        public void Configure(EntityTypeBuilder<Desparacitacion> builder)
        {
            /*builder.ToTable("Desparacitaciones");
            builder.Property(e => e.idDesparacitacion).HasColumnName("idDesparacitacion");
            builder.Property(e => e.idPractica).HasColumnName("idPractica");
            builder.Property(e => e.itemId).HasColumnName("itemId");
            builder.Property(e => e.PacienteId).HasColumnName("PacienteId");
            builder.Property(e => e.Fecha).HasColumnName("Fecha");*/
        }
    }
}
