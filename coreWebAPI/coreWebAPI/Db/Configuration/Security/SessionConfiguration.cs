using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;

namespace MOM.Core.Db.Configuration.Security
{
    class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(obj => obj.Id);
            builder.Property(obj => obj.Id).ValueGeneratedOnAdd();
        }
    }
}
