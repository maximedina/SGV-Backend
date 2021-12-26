using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Practicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Practicas
{
    public class DetallePracticaConfiguration : IEntityTypeConfiguration<DetallePractica>
    {
        public void Configure(EntityTypeBuilder<DetallePractica> builder)
        {
            /*builder.ToTable("DetallePractica");
            builder.Property(e => e.idDetalle).HasColumnName("idDetalle");
            builder.Property(e => e.idPractica).HasColumnName("idPractica");
            builder.Property(e => e.itemId).HasColumnName("itemId");
            builder.Property(e => e.cantidad).HasColumnName("cantidad");
            builder.Property(e => e.precio).HasColumnName("precio");
            builder.Property(e => e.descuento).HasColumnName("descuento");
            builder.Property(e => e.importe).HasColumnName("importe");*/
        }
    }
}
