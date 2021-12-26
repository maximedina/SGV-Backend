using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Items;

namespace MOM.Core.Db.Configuration.Items
{
    class ProveedoresConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("Proveedores");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Contacto)
                .HasMaxLength(250)
                .HasColumnName("contacto");

            builder.Property(e => e.Telefono)
                .HasMaxLength(25)
                .HasColumnName("telefono");


            builder.Property(e => e.CuentaCorriente)
                .HasColumnName("cuentaCorriente");

            builder.Property(e => e.Domicilio)
                .HasMaxLength(250)
                .HasColumnName("domicilio");

            builder.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");

            builder.Property(e => e.Inactivo)
                .HasColumnName("inactivo")
                .HasDefaultValue(false);

            builder.Property(e => e.Nombre)
                .HasMaxLength(250)
                .HasColumnName("nombre");

            builder.Property(e => e.Observaciones).HasColumnName("observaciones");

        }
    }
}
