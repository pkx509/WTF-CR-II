using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductShapeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductShape>
    {
        public ProductShapeConfiguration()
            : this("dbo")
        {
        }

        public ProductShapeConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_shape");
            HasKey(x => x.ProductShapeID);

            Property(x => x.ProductShapeID).IsRequired().HasColumnName("ProductShapeID").HasColumnType("uniqueidentifier")
                               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

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
