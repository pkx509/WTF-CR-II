using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{

    public class AdjustDetailConfiguration : EntityTypeConfiguration<AdjustDetail>
    {
        public AdjustDetailConfiguration() : this("dbo")
        {

        }
        public AdjustDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.AdjustDetailID);

            // Properties
            Property(t => t.ProductLot)
                .HasMaxLength(50);

            Property(t => t.LocationID);

            Property(t => t.Barcode)
                .HasMaxLength(50);

            Property(t => t.PalletCode)
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("adj_adjust_detail");
            Property(t => t.AdjustDetailID).HasColumnName("AdjustDetailID");
            Property(t => t.AdjustID).HasColumnName("AdjustID");
            Property(t => t.ReferenceID).HasColumnName("ReferenceID");
            Property(t => t.AdjustTransactionType).HasColumnName("AdjustTransactionType");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.ProductLot).HasColumnName("ProductLot");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.Barcode).HasColumnName("Barcode");
            Property(t => t.AdjustStatus).HasColumnName("AdjustStatus");
            Property(t => t.ProductUnitID).HasColumnName("ProductUnitID");
            Property(t => t.AdjustStockQty).HasColumnName("AdjustStockQty");
            Property(t => t.AdjustStockUnitID).HasColumnName("AdjustStockUnitID");
            Property(t => t.AdjustBaseQty).HasColumnName("AdjustBaseQty");
            Property(t => t.AdjustBaseUnitID).HasColumnName("AdjustBaseUnitID");
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.MFGDate).HasColumnName("MFGDate");
            Property(t => t.IsSentInterface);
            // Relationships
            HasRequired(t => t.Adjust)
                .WithMany(t => t.AdjustDetail)
                .HasForeignKey(d => d.AdjustID);

        }
    }
}
