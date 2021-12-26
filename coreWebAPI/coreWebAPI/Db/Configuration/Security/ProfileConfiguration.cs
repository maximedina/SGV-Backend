using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;

namespace MOM.Core.Db.Configuration.Security
{
    class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profile");
            builder.HasKey(obj => obj.ProfileId);
            builder.Property(obj => obj.ProfileId).ValueGeneratedOnAdd();
        }
    }
}
