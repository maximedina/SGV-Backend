using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.ventas
{
    public class VentasConfiguration : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("Ventas");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.userId).HasColumnName("userId");
            builder.Property(e => e.fechaHora).HasColumnName("fechaHora").HasDefaultValue(DateTime.Now);
            builder.Property(e => e.total).HasColumnName("total");
            builder.Property(e => e.descuento).HasColumnName("descuento");
            builder.Property(e => e.ticketTarjeta).HasColumnName("ticketTarjeta");
            builder.Property(e => e.tipoPagoId).HasColumnName("tipoPagoId");
            builder.Property(e => e.Estatus).HasColumnName("Estatus");
            builder.Ignore(x => x.idCaja);
        }
    }
}
