using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Shared
{
    class UomConfiguration : IEntityTypeConfiguration<Uom>
    {
        public void Configure(EntityTypeBuilder<Uom> builder)
        {
            builder.ToTable("Uom");
            builder.HasKey(obj => obj.UomId);
            builder.HasIndex(obj => obj.Name).IsUnique();
        }
    }
}
