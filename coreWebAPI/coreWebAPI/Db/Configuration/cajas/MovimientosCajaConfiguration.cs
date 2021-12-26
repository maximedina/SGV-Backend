using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.cajas
{
    public class MovimientosCajaConfiguration : IEntityTypeConfiguration<Movimiento>
    {
        public void Configure(EntityTypeBuilder<Movimiento> builder)
        {
            builder.ToTable("MovimientosCaja");
            builder.Property(e => e.idMovimiento).HasColumnName("idMovimiento");
            builder.Property(e => e.idCaja).HasColumnName("idCaja");
            builder.Property(e => e.FecMovimiento).HasColumnName("FecMovimiento");
            builder.Property(e => e.TipoMovimiento).HasColumnName("TipoMovimiento");
            builder.Property(e => e.Importe).HasColumnName("Importe");
            builder.Property(e => e.idMotivo).HasColumnName("idMotivo");
            builder.Property(e => e.Comentario).HasColumnName("Comentario");
            builder.Property(e => e.TipoPago).HasColumnName("TipoPago");
            builder.Ignore(x => x.idCliente);
            builder.Ignore(x => x.idProveedor);
        }
    }
}
