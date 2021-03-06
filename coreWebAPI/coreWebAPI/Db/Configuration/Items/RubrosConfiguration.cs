using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Items;

namespace MOM.Core.Db.Configuration.Items
{
    class RubrosConfiguration : IEntityTypeConfiguration<Rubro>
    {
        public void Configure(EntityTypeBuilder<Rubro> builder)
        {
            builder.ToTable("Rubros");

            builder.HasIndex(e => e.Nombre)
                    .IsUnique();
            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
            builder.Property(e => e.Descripcion)
                    .HasMaxLength(350)
                    .HasColumnName("descripcion");
            builder.Property(e => e.Inactivo)
                    .HasColumnName("inactivo")
                    .HasDefaultValue(false);
            builder.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .HasColumnName("nombre");
            builder.Property(e => e.grupoId)
                    .HasColumnName("grupoId");

        }
    }
}
