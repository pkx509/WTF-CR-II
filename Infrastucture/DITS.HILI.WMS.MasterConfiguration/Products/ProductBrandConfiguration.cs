using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductBrandConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductBrand>
    {
        public ProductBrandConfiguration()
            : this("dbo")
        {
        }

        public ProductBrandConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_brand");
            HasKey(x => x.ProductBrandID);

            Property(x => x.ProductBrandID).IsRequired().HasColumnName("ProductBrandID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(150);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

        }
    }
}
