using DITS.HILI.WMS.MasterModel.Products;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductCodeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductCodes>
    {
        public ProductCodeConfiguration()
            : this("dbo")
        {
        }

        public ProductCodeConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_code");
            HasKey(x => new { x.ProductID, x.Code });

            Property(x => x.ProductID).IsRequired().HasColumnName("ProductID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Code).IsRequired().HasColumnName("ProductCode").HasColumnType("nvarchar").HasMaxLength(20)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsDefault).IsRequired().HasColumnName("IsDefault").HasColumnType("bit");
            Property(x => x.CodeType).IsRequired().HasColumnName("CodeType").HasColumnType("int");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");

            HasRequired(x => x.Product).WithMany(x => x.CodeCollection).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
        }
    }
}
