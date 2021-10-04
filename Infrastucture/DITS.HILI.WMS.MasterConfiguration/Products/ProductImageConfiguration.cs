using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MasterConfiguration.Configuration
{
    public class ProductImageConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductImages>
    {
        public ProductImageConfiguration()
            : this("dbo")
        {
        }

        public ProductImageConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_images");
            HasKey(x => new { x.ProductID, x.Sequence }); 

            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").IsRequired().HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.Product).WithMany(x => x.ImageCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);

        }
    }
}
