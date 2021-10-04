using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.Configuration.Products
{
    public class ProductUnitConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductUnit>
    {
        public ProductUnitConfiguration()
            : this("dbo")
        {
        }

        public ProductUnitConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_uom");
            HasKey(x => x.ProductUnitID);

            Property(x => x.ProductUnitID).IsRequired().HasColumnName("ProductUnitID").HasColumnType("uniqueidentifier")
                               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Code).IsRequired().HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(20)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.Barcode).IsRequired().HasColumnName("Barcode").HasColumnType("nvarchar").HasMaxLength(150);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.Quantity).IsRequired().HasColumnName("Quantity").HasColumnType("decimal");
            Property(x => x.PalletQTY);
            Property(x => x.IsBaseUOM).IsRequired().HasColumnName("IsBaseUOM").HasColumnType("bit");
            Property(x => x.Width).IsRequired().HasColumnName("Width").HasColumnType("float");
            Property(x => x.Height).IsRequired().HasColumnName("Height").HasColumnType("float");
            Property(x => x.Length).IsRequired().HasColumnName("Length").HasColumnType("float");
            Property(x => x.Cubicmeters).IsRequired().HasColumnName("Cubicmeters").HasColumnType("float");
            Property(x => x.ProductWeight).IsRequired().HasColumnName("ProductWeight").HasColumnType("float");
            Property(x => x.PackageWeight).IsRequired().HasColumnName("PackageWeight").HasColumnType("float");
            Property(x => x.URLImage).HasColumnName("URLImage").IsRequired().HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.ConversionMark).HasColumnName(@"ConversionMark").HasColumnType("decimal").IsOptional().HasPrecision(1, 0);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.Product).WithMany(x => x.UnitCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(true);
        }
    }
}
