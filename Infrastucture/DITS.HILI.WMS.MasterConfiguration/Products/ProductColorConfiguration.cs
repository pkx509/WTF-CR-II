using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration.Configuration
{
    public class ProductColourConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductColor>
    {
        public ProductColourConfiguration()
            : this("dbo")
        {
        }

        public ProductColourConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_color");
            HasKey(x => new { x.ProductID, x.Sequence });

            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.Product).WithMany(x => x.ColorCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);

        }
    }
}
