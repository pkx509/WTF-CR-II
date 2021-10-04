using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration.Configuration
{
    public class ProductGroupLevel3Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductGroupLevel3>
    {
        public ProductGroupLevel3Configuration()
            : this("dbo")
        {
        }

        public ProductGroupLevel3Configuration(string schema)
        {
            ToTable(schema + ".sys_product_group_level3");
            HasKey(x => x.ProductGroupLevel3ID);

            Property(x => x.ProductGroupLevel3ID).IsRequired().HasColumnName("ProductGroupLevel3ID").HasColumnType("uniqueidentifier")
                               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProductGroupLevel2ID).IsRequired().HasColumnName("ProductGroupLevel2ID").HasColumnType("uniqueidentifier");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.ProductGroupLevel2).WithMany(x => x.ProductGroupLevel3Collection).HasForeignKey(x => x.ProductGroupLevel2ID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductOwner).WithMany(x => x.ProductGroupLevel3Collection).HasForeignKey(x => x.ProductOwnerID);
        }
    }
}
