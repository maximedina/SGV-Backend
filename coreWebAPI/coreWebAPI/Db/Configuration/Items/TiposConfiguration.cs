using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Items;

namespace MOM.Core.Db.Configuration.Items
{
    class TiposConfiguration : IEntityTypeConfiguration<Tipo>
    {
        public void Configure(EntityTypeBuilder<Tipo> builder)
        {
            builder.ToTable("Tipos");
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
