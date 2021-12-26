using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;

namespace MOM.Core.Db.Configuration.Security
{
    class SessionLogConfiguration : IEntityTypeConfiguration<SessionLog>
    {
        public void Configure(EntityTypeBuilder<SessionLog> builder)
        {
            builder.HasKey(obj => obj.Id);
            builder.Property(obj => obj.Id).ValueGeneratedOnAdd();
        }
    }
}
