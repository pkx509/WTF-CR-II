using DITS.HILI.WMS.MasterModel.Utility;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductStatusMapDocumentConfiguration : EntityTypeConfiguration<ProductStatusMapDocument>
    {
        public ProductStatusMapDocumentConfiguration()
            : this("dbo")
        {

        }

        public ProductStatusMapDocumentConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.ProductStatusID, t.DocumentTypeID });

            // Properties
            // Table & Column Mappings
            ToTable(schema + ".sys_product_status_map_document");
            Property(t => t.ProductStatusID).HasColumnName("ProductStatusID");
            Property(t => t.DocumentTypeID).HasColumnName("DocumentTypeID");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.IsDefault).HasColumnName("IsDefault");

            // Relationships
            //this.HasRequired(t => t.DocumentType)
            //    .WithMany(t => t.ProductStatusMapDocumentCollection)
            //    .HasForeignKey(d => d.DocumentTypeID);
            //this.HasRequired(t => t.ProductStatus)
            //    .WithMany(t => t.ProductStatusMapDocumentCollection)
            //    .HasForeignKey(d => d.ProductStatusID);

        }
    }
}
