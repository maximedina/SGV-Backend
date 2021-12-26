using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.cajas
{
    public class MovCCProveedor : IEntityTypeConfiguration<MCCProveedor>
    {
        public void Configure(EntityTypeBuilder<MCCProveedor> builder)
        {
            builder.ToTable("MovCCProveedor");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.idMovimientoCaja).HasColumnName("idMovimientoCaja");
            builder.Property(e => e.proveedorId).HasColumnName("proveedorId");
            builder.Property(e => e.Importe).HasColumnName("Importe");
        }
    }
}
