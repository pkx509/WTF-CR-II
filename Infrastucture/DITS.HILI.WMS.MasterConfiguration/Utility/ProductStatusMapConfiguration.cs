using DITS.HILI.WMS.MasterModel.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ProductStatusMapConfiguration : EntityTypeConfiguration<ProductStatusMap>
    {
        public ProductStatusMapConfiguration()
            : this("dbo")
        {

        }

        public ProductStatusMapConfiguration(string schema)
        {
            ToTable(schema + ".sys_product_status_map");
            HasKey(x => new { x.ProductStatusID, x.ProductSubStatusID });
            Property(x => x.ProductStatusID).IsRequired().HasColumnName("ProductStatusID").HasColumnType("uniqueidentifier")
                                           .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ProductSubStatusID).IsRequired().HasColumnName("ProductSubStatusID").HasColumnType("uniqueidentifier")
                                           .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.IsDefault).IsRequired().HasColumnName("IsDefault").HasColumnType("bit");
            HasRequired(x => x.ProductStatus).WithMany(x => x.ProductStatusMapCollection).HasForeignKey(x => x.ProductStatusID).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductSubStatus).WithMany(x => x.ProductStatusMapCollection).HasForeignKey(x => x.ProductSubStatusID).WillCascadeOnDelete(false);

        }
    }
}
