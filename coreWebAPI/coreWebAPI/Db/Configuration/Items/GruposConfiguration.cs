using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Items;

namespace MOM.Core.Db.Configuration.Items
{
    class GruposConfiguration : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("Grupos");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            builder.HasIndex(e => e.Nombre).IsUnique();
            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("nombre");
            builder.Property(e => e.Descripcion).HasColumnName("descripcion");
            builder.Property(e => e.Inactivo)
                .HasColumnName("inactivo")
                .HasDefaultValue(false);
        }
    }
}