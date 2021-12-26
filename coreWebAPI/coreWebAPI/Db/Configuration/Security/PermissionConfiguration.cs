using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Security
{
    class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");
            builder.HasKey(obj => obj.PermissionId);
            builder.Property(obj => obj.PermissionId).ValueGeneratedOnAdd();
        }
    }
}
