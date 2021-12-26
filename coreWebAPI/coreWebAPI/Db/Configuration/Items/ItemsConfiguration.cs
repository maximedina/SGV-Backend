using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MOM.Core.Models.Items;

namespace MOM.Core.Db.Configuration.Items
{
    class ItemsConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            builder.Property(e => e.Inactivo)
                .HasColumnName("inactivo")
                .HasDefaultValue(false);
            builder.Property(e => e.Iva).HasColumnName("iva");
            builder.Property(e => e.Nombre)
                .HasMaxLength(250)
                .HasColumnName("nombre");
            builder.Property(e => e.Codigo)
    .HasMaxLength(100)
    .HasColumnName("codigo");
            builder.Property(e => e.PorcentajePrecio).HasColumnName("porcentajePrecio");
            builder.Property(e => e.PrecioCosto).HasColumnName("precioCosto");
            builder.Property(e => e.categoriaId).HasColumnName("categoriaId"); 
                builder.Property(e => e.proveedorId).HasColumnName("proveedorId"); 
            builder.Property(e => e.Presentacion)
                .HasMaxLength(150)
                .HasColumnName("presentacion");
            builder.Property(e => e.Stock).HasColumnName("stock");
            builder.Property(e => e.StockMinimo).HasColumnName("stockMinimo");
            builder.Property(e => e.tipoId).HasColumnName("tipoId");
            builder.Ignore(x => x.PrecioVenta);
            builder.Ignore(x => x.Identificador);
        }
    }
}
