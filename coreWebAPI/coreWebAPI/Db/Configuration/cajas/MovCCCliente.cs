using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.cajas
{
    public class MovCCCliente : IEntityTypeConfiguration<MCCCliente>
    {
        public void Configure(EntityTypeBuilder<MCCCliente> builder)
        {
            builder.ToTable("MovCCCliente");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.idMovimientoCaja).HasColumnName("idMovimientoCaja");
            builder.Property(e => e.clienteId).HasColumnName("clienteId");
            builder.Property(e => e.Importe).HasColumnName("Importe");
            builder.Property(e => e.TipoMovimiento).HasColumnName("TipoMovimiento");
        }
    }
}
