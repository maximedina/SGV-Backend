using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Turnos;

namespace MOM.Core.Db.Configuration.Turnos
{
    class TurnosConfiguration : IEntityTypeConfiguration<Turno>
    {
        public void Configure(EntityTypeBuilder<Turno> builder)
        {
            builder.ToTable("Turnos");

            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.idPaciente).HasColumnName("idPaciente");
            builder.Property(e => e.idPractica).HasColumnName("idPractica");
            builder.Property(e => e.idProfesional).HasColumnName("idProfesional");
            builder.Property(e => e.observaciones).HasColumnName("observaciones");

            builder.Property(e => e.Tomado)
                .HasColumnName("tomado")
                .HasDefaultValue(false);

            builder.Property(e => e.Inactivo)
    .HasColumnName("inactivo")
    .HasDefaultValue(false);
            builder.Property(e => e.start)
    .HasColumnType("datetime")
    .HasColumnName("inicio");
            builder.Property(e => e.end)
    .HasColumnType("datetime")
    .HasColumnName("fin");
            builder.Ignore(e => e.UserId);
        }
    }
}
