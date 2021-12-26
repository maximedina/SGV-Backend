using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Security
{
    class ProfilePermissionConfiguration : IEntityTypeConfiguration<ProfilePermission>
    {
        public void Configure(EntityTypeBuilder<ProfilePermission> builder)
        {
            builder.ToTable("ProfilePermission");
            builder.HasKey(obj => obj.ProfilePermissionId);
            builder.Property(obj => obj.ProfilePermissionId).ValueGeneratedOnAdd();
        }
    }
}
