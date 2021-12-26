using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Pacientes;
using System;
namespace MOM.Core.Db.Configuration.Pacientes
{
	class PacientesConfiguration : IEntityTypeConfiguration<Paciente>
	{
		public void Configure(EntityTypeBuilder<Paciente> builder)
		{
			builder.ToTable("Pacientes");

			builder.Property(e => e.id).HasColumnName("id");
			builder.Property(e => e.razaId).HasColumnName("razaId");
			builder.Property(e => e.userId).HasColumnName("userId");

			builder.Property(e => e.observaciones)
				.HasMaxLength(500)
				.HasColumnName("observaciones");
			builder.Property(e => e.inactivo)
				.HasColumnName("inactivo")
				.HasDefaultValue(false);
			builder.Property(e => e.fallecido)
	.HasColumnName("fallecido")
	.HasDefaultValue(false);
			builder.Property(e => e.nombre)
				.HasMaxLength(250)
				.HasColumnName("nombre");
			builder.Property(e => e.fechaNacimiento)
	  .HasColumnType("datetime")
	  .HasColumnName("fechaNacimiento");
			builder.Property(e => e.fechaFallecimiento)
	.HasColumnType("datetime")
	.HasColumnName("fechaFallecimiento");
			builder.Ignore(x => x.Edad);
			//builder.Ignore(x => x.Identificador);
		}
	}
}
