using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOM.Core.Db.Configuration.Shared
{
    class DataTypeConfiguration : IEntityTypeConfiguration<DataTypes>
    {
        public void Configure(EntityTypeBuilder<DataTypes> builder)
        {
            builder.ToTable("DataType");
            builder.HasKey(obj => obj.DataTypeId);

            builder.Property(obj => obj.DataTypeId).ValueGeneratedOnAdd();
            builder.HasIndex(obj => obj.Name).IsUnique();
        }
    }
}
