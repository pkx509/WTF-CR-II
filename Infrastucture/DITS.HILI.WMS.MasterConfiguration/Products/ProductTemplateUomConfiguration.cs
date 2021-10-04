using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductTemplateUomConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductTemplateUom>
    {
        public ProductTemplateUomConfiguration()
            : this("dbo")
        {
        }

        public ProductTemplateUomConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_template_uom");
            HasKey(x => x.Product_UOM_Template_ID);

            Property(x => x.Product_UOM_Template_ID).HasColumnName(@"Product_UOM_Template_ID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Product_UOM_Template_Name).HasColumnName(@"Product_UOM_Template_Name").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

        }
    }
}
