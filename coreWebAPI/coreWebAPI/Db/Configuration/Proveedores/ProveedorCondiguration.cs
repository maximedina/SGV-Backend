using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Proveedores
{
    public class ProveedorCondiguration : IEntityTypeConfiguration<ProveedorModel>
    {
        public void Configure(EntityTypeBuilder<ProveedorModel> builder)
        {
            builder.ToTable("Proveedores");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.nombre).HasMaxLength(250).HasColumnName("nombre");
            builder.Property(e => e.cuentaCorriente).HasColumnName("cuentaCorriente");
            builder.Property(e => e.contacto).HasMaxLength(250).HasColumnName("contacto");
            builder.Property(e => e.domicilio).HasMaxLength(250).HasColumnName("domicilio");
            builder.Property(e => e.telefono).HasMaxLength(25).HasColumnName("telefono");
            builder.Property(e => e.email).HasMaxLength(200).HasColumnName("email");
            builder.Property(e => e.ciudadId).HasColumnName("ciudadId");
            builder.Property(e => e.observaciones).HasMaxLength(250).HasColumnName("observaciones");
            builder.Property(e => e.observaciones).HasColumnName("inactivo");
        }
    }
}
