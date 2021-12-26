using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Security
{
    class SystemModuleConfiguration : IEntityTypeConfiguration<SystemModule>
    {
        public void Configure(EntityTypeBuilder<SystemModule> builder)
        {
            builder.ToTable("SystemModule");
            builder.HasKey(obj => obj.SystemModuleId);
            builder.Property(obj => obj.IsEnabled).HasDefaultValue(true);
        }
    }
}
