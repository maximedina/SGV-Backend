using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Practicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Practicas
{
    public class RegistroPracticaConfiguration : IEntityTypeConfiguration<RegistroPractica>
    {
        public void Configure(EntityTypeBuilder<RegistroPractica> builder)
        {
            /*builder.ToTable("RegistroPracticas");
            builder.Property(e => e.idPractica).HasColumnName("idPractica");
            builder.Property(e => e.userId).HasColumnName("UserId");
            builder.Property(e => e.clienteId).HasColumnName("ClienteId");
            builder.Property(e => e.pacienteId).HasColumnName("PacienteId");
            builder.Property(e => e.fechaRegistro).HasColumnName("FechaRegistro");
            builder.Property(e => e.comentarios).HasColumnName("Comentarios");
            builder.Property(e => e.total).HasColumnName("Total");
            builder.Property(e => e.descuento).HasColumnName("Descuento");
            builder.Property(e => e.estatus).HasColumnName("Estatus");*/
            //builder.Ignore(e => e.turno);
            //builder.Ignore(e => e.turnoid);
            
        }
    }
}
