using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.cajas
{
    public class MovCajaVtaConfiguration : IEntityTypeConfiguration<MovCajaVta>
    {
        public void Configure(EntityTypeBuilder<MovCajaVta> builder)
        {
            builder.ToTable("MovCaja_venta");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.idMovimiento).HasColumnName("idMovimiento");
            builder.Property(e => e.idVenta).HasColumnName("idVenta");
        }
    }
}
