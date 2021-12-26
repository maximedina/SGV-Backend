using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Pacientes;

namespace MOM.Core.Db.Configuration.Pacientes
{
    class RazasConfiguration : IEntityTypeConfiguration<Raza>
    {
        public void Configure(EntityTypeBuilder<Raza> builder)
        {
            builder.ToTable("Raza");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("nombre");
            builder.Property(e => e.Descripcion).HasColumnName("descripcion");
            builder.Property(e => e.Inactivo)
                .HasColumnName("inactivo")
                .HasDefaultValue(false);

        }
    }
}
