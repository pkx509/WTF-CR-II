using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.Configuration.Products
{
    public class ProductReplacementConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductReplacement>
    {
        public ProductReplacementConfiguration()
            : this("dbo")
        {
        }

        public ProductReplacementConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_replacement");
            HasKey(x => new { x.ProductID, x.ReplaceProductID, x.Sequence });

            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ReplaceProductID).IsRequired().HasColumnName("ReplaceProductID").HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Quantity).IsRequired().HasColumnName("Quantity").HasColumnType("float");
            Property(x => x.ProductOwnerID).IsOptional().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.Product).WithMany(x => x.ProductParentReplacementCollection).HasForeignKey(x => x.ProductID);
            HasRequired(x => x.Product).WithMany(x => x.ProductChildReplacementCollection).HasForeignKey(x => x.ReplaceProductID);
            HasOptional(x => x.ProductOwner).WithMany(x => x.ProductReplacementCollection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);

        }
    }
}
