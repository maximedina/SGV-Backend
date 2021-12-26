using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Security
{
    class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSetting");
            builder.HasKey(obj => obj.SystemSettingId);
            builder.Property(obj => obj.SystemSettingId).ValueGeneratedOnAdd();
            builder.Property(obj => obj.Key).IsRequired().HasMaxLength(200);
            builder.Property(obj => obj.Value).IsRequired().HasMaxLength(200);
            builder.Property(obj => obj.IsDeleted).HasDefaultValue(false);

            builder.HasIndex(obj => obj.Key);
        }
    }
}
