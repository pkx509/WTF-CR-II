using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductTemplateUomDetailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductTemplateUomDetail>
    {
        public ProductTemplateUomDetailConfiguration()
            : this("dbo")
        {
        }

        public ProductTemplateUomDetailConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_template_uom_detail");
            HasKey(x => x.Product_UOM_Template_Detail_ID);

            Property(x => x.Product_UOM_Template_Detail_ID).HasColumnName(@"Product_UOM_Template_Detail_ID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Product_UOM_Template_ID).HasColumnName(@"Product_UOM_Template_ID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Product_UOM_Template_Detail_Name).HasColumnName(@"Product_UOM_Template_Detail_Name").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Product_UOM_Template_Detail_Short_Name).HasColumnName(@"Product_UOM_Template_Detail_Short_Name").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Product_UOM_Template_Detail_Quantity).HasColumnName(@"Product_UOM_Template_Detail_Quantity").HasColumnType("float").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_SKU).HasColumnName(@"Product_UOM_Template_Detail_SKU").HasColumnType("int").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_Status).HasColumnName(@"Product_UOM_Template_Detail_Status").HasColumnType("int").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_Package_Width).HasColumnName(@"Product_UOM_Template_Detail_Package_Width").HasColumnType("float").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_Package_Length).HasColumnName(@"Product_UOM_Template_Detail_Package_Length").HasColumnType("float").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_Package_Height).HasColumnName(@"Product_UOM_Template_Detail_Package_Height").HasColumnType("float").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_Package_Weight).HasColumnName(@"Product_UOM_Template_Detail_Package_Weight").HasColumnType("float").IsOptional();
            Property(x => x.Product_UOM_Template_Detail_Barcode).HasColumnName(@"Product_UOM_Template_Detail_Barcode").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Product_UOM_Template_Detail_Win_Code).HasColumnName(@"Product_UOM_Template_Detail_Win_Code").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.Product_UOM_Template_Detail_Weight).HasColumnName(@"Product_UOM_Template_Detail_Weight").HasColumnType("float").IsOptional();

            HasRequired(x => x.ProductTemplateUom).WithMany(x => x.ProductTemplateUomDetailCollection).HasForeignKey(x => x.Product_UOM_Template_ID).WillCascadeOnDelete(false);

        }
    }
}
