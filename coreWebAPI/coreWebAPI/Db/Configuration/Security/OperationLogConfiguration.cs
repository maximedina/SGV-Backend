using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Security
{
    class OperationLogConfiguration : IEntityTypeConfiguration<OperationLog>
    {
        public void Configure(EntityTypeBuilder<OperationLog> builder)
        {
            builder.ToTable("OperationLog");
            builder.HasKey(obj => obj.OperationLogId);
            builder.Property(obj => obj.OperationLogId).ValueGeneratedOnAdd();
            builder.Property(obj => obj.Description).HasMaxLength(500);
            builder.HasOne(obj => obj.User);
            builder.HasOne(obj => obj.Permission);
        }
    }
}
