using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
            : this("dbo")
        {
        }

        public ProductConfiguration(string schema)
        {
            ToTable(schema + ".sys_product");
            HasKey(x => x.ProductID);
            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(150)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(t => t.Remark).IsOptional().HasColumnName("Remark");
            Property(x => x.Description).IsOptional().HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.Age).HasColumnName("Age").HasColumnType("float");
            Property(x => x.SafetyStockQTY);
            Property(x => x.ProductGroupLevel3ID).IsOptional().HasColumnName("ProductGroupLevel3ID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductShapeID).IsOptional().HasColumnName("ProductShapeID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductBrandID).IsOptional().HasColumnName("ProductBrandID").HasColumnType("uniqueidentifier");
            Property(x => x.ProductModel).IsOptional().HasColumnName("ProductModel").HasColumnType("nvarchar").HasMaxLength(250);

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasOptional(x => x.ProductGroupLevel3).WithMany(x => x.ProductCollection).HasForeignKey(x => x.ProductGroupLevel3ID).WillCascadeOnDelete(false);
            //HasOptional(x => x.ProductShape).WithMany(x => x.ProductCollection).HasForeignKey(x => x.ProductShapeID).WillCascadeOnDelete(false);
            //HasOptional(x => x.ProductBrand).WithMany(x => x.ProductCollection).HasForeignKey(x => x.ProductBrandID).WillCascadeOnDelete(false);

        }
    }
}
