using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Security;
namespace MOM.Core.Db.Configuration.Security
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(obj => obj.UserId);
            builder.Property(obj => obj.UserId).ValueGeneratedOnAdd();
            builder.HasIndex(obj => obj.Login).IsUnique();
            builder.Property(obj => obj.Login).HasMaxLength(200);
            builder.Property(obj => obj.Name).HasMaxLength(200);
            builder.Property(obj => obj.IsDeleted).HasDefaultValue(false);
            builder.Property(obj => obj.cuentaCorriente).HasColumnName("cuentaCorriente").HasDefaultValue(0);
            builder.Property(obj => obj.dni).HasColumnName("dni").HasDefaultValue(0);
            builder.Ignore(x => x.Identificador);
            builder.Ignore(x => x.Domicilio);
            //builder.Property(obj => obj.Identificador).HasComputedColumnSql("[Login] + ' - ' + [Name]");
            //builder.Property(obj => obj.Domicilio).HasComputedColumnSql("[Calle] + ' ' + [Altura]");
        }
    }
}
