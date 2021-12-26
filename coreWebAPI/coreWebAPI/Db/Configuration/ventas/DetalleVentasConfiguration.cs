using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.ventas
{
    public class DetalleVentasConfiguration : IEntityTypeConfiguration<DetalleVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleVenta> builder)
        {
            builder.ToTable("DetalleVentas");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.ventaId).HasColumnName("ventaId");
            builder.Property(e => e.itemId).HasColumnName("itemId");
            builder.Property(e => e.cantidad).HasColumnName("cantidad");
            builder.Property(e => e.descuento).HasColumnName("descuento");
            builder.Property(e => e.precio).HasColumnName("precio");
            builder.Property(e => e.importe).HasColumnName("importe");
        }
    }
}
