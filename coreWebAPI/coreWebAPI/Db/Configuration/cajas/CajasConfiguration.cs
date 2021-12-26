using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.cajas
{
    public class CajasConfiguration : IEntityTypeConfiguration<Caja>
    {
        public void Configure(EntityTypeBuilder<Caja> builder)
        {
            builder.ToTable("Cajas");
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.userId).HasColumnName("userId");
            builder.Property(e => e.Fecha_Apertura).HasColumnName("FecApertura").HasDefaultValue(DateTime.Now);
            builder.Property(e => e.Fecha_Cierre).HasColumnName("FecCierre");
            builder.Property(e => e.SaldoInicial).HasColumnName("SaldoInicial");
            builder.Property(e => e.Ingresos).HasColumnName("Ingresos");
            builder.Property(e => e.Egresos).HasColumnName("Egresos");
            builder.Property(e => e.Estatus).HasColumnName("Estatus").HasDefaultValue(true);
            builder.Property(e => e.Comentario).HasMaxLength(250).HasColumnName("Comentario");
        }
    }
}
