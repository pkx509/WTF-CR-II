using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration.Configuration
{
    public class ProductGroupLevel1Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductGroupLevel1>
    {
        public ProductGroupLevel1Configuration()
            : this("dbo")
        {
        }

        public ProductGroupLevel1Configuration(string schema)
        {
            ToTable(schema + ".sys_product_group_level1");
            HasKey(x => x.ProductGroupLevel1ID);

            Property(x => x.ProductGroupLevel1ID).IsRequired().HasColumnName("ProductGroupLevel1ID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.ProductOwnerID).IsRequired().HasColumnName("ProductOwnerID").HasColumnType("uniqueidentifier");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.ProductOwner).WithMany(x => x.ProductGroupLevel1Collection).HasForeignKey(x => x.ProductOwnerID).WillCascadeOnDelete(false);

        }
    }
}
