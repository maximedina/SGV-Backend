using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Practicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Practicas
{
    public class VacunaConfiguration : IEntityTypeConfiguration<Vacuna>
    {
        public void Configure(EntityTypeBuilder<Vacuna> builder)
        {
            /*builder.ToTable("Vacunas");
            builder.Property(e => e.idVacuna).HasColumnName("idVacuna");
            builder.Property(e => e.idPractica).HasColumnName("idPractica");
            builder.Property(e => e.itemId).HasColumnName("itemId");
            builder.Property(e => e.PacienteId).HasColumnName("PacienteId");
            builder.Property(e => e.Fecha).HasColumnName("Fecha");*/
        }
    }
}
