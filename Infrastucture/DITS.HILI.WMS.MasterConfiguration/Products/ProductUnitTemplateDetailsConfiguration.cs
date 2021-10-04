using DITS.HILI.WMS.Master.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Configuration.Products
{
    public class ProductUnitTemplateDetailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductUnitTemplateDetails>
    {
        public ProductUnitTemplateDetailConfiguration()
            : this("dbo")
        {
        }

        public ProductUnitTemplateDetailConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_unit_template_detail");
            HasKey(x => x.DetailID);
            Property(x => x.DetailID).IsRequired().HasColumnName("DetailID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProductUOMTemplateID).IsRequired().HasColumnName("ProductUOMTemplateID").HasColumnType("uniqueidentifier");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Quantity).IsRequired().HasColumnName("Quantity").HasColumnType("float");
            Property(x => x.IsBaseUOM).IsRequired().HasColumnName("IsBaseUOM").HasColumnType("bit");
            Property(x => x.Width).IsRequired().HasColumnName("Width").HasColumnType("float");
            Property(x => x.Height).IsRequired().HasColumnName("Height").HasColumnType("float");
            Property(x => x.Lenght).IsRequired().HasColumnName("Lenght").HasColumnType("float");
            Property(x => x.Cubicmeters).IsRequired().HasColumnName("Cubicmeters").HasColumnType("float");
            Property(x => x.ProductWeight).IsRequired().HasColumnName("ProductWeight").HasColumnType("float");
            Property(x => x.PackageWeight).IsRequired().HasColumnName("PackageWeight").HasColumnType("float");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

        } 
    }
}
