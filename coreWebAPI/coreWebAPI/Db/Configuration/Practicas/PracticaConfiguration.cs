using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Practicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Practicas
{
    public class PracticaConfiguration : IEntityTypeConfiguration<Practica>
    {
        public void Configure(EntityTypeBuilder<Practica> builder)
        {
            builder.ToTable("Practicas");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.fechaHora).HasColumnName("fechaHora");
            builder.Property(e => e.ventaId).HasColumnName("ventaId");
            builder.Property(e => e.turnoId).HasColumnName("turnoId");
            builder.Property(e => e.pacienteId).HasColumnName("pacienteId");
            builder.Property(e => e.userId).HasColumnName("userId");
            builder.Property(e => e.itemId).HasColumnName("itemId");
            builder.Property(e => e.proximaPractica).HasColumnName("proximaPractica");
            builder.Property(e => e.texto).HasColumnName("texto");
            //builder.Ignore(e => e.turno);
            //builder.Ignore(e => e.turnoid);

        }
    }
}
