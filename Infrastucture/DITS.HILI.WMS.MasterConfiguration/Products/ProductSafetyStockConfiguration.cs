using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.Configuration.Products
{
    public class ProductSafetyStockConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductSafetyStock>
    {
        public ProductSafetyStockConfiguration()
            : this("dbo")
        {
        }

        public ProductSafetyStockConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_safety_stock");
            HasKey(x => new { x.ProductID, x.Sequence });

            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Quantity).IsRequired().HasColumnName("Quantity").HasColumnType("float");
            Property(x => x.ProductOwnerID).IsOptional().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.Product).WithMany(x => x.SafetyStockCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            HasOptional(x => x.ProductOwner).WithMany(x => x.ProductSafetyStockCollection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);

        }
    }
}
