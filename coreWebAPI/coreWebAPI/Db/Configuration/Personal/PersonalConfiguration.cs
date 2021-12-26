using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.WebAPI.Models.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Db.Configuration.Personal
{
    public class PersonalConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Cajas");
            builder.Property(e => e.id).HasColumnName("id");
            builder.Property(e => e.legajo).HasColumnName("legajo");
            builder.Property(e => e.matricula).HasColumnName("matricula");
            builder.Property(e => e.observaciones).HasColumnName("observaciones");
            builder.Property(e => e.inactivo).HasColumnName("inactivo");
            builder.Property(e => e.userId).HasColumnName("userId");
        }
    }
}
