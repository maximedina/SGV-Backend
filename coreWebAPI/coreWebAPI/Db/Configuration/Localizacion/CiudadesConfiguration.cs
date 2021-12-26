using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Localizacion;

namespace MOM.Core.Db.Configuration.Localizacion
{
    class CiudadesConfiguration : IEntityTypeConfiguration<Ciudad>
    {
        public void Configure(EntityTypeBuilder<Ciudad> builder)
        {
            builder.ToTable("Ciudades");

            builder.HasIndex(e => e.Nombre)
                    .IsUnique();
            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
            builder.Property(e => e.Inactivo)
                    .HasColumnName("inactivo")
                    .HasDefaultValue(false);
            builder.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .HasColumnName("nombre");
        }
    }
}