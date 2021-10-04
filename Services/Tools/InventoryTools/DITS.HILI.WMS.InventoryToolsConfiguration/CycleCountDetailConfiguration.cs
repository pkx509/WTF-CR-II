using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{

    public class CycleCountDetailConfiguration : EntityTypeConfiguration<CycleCountDetail>
    {
        public CycleCountDetailConfiguration() : this("dbo")
        {

        }
        public CycleCountDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.CyclecountDetailID);

            // Properties
            Property(t => t.ProductID);

            Property(t => t.ProductUnitID);

            Property(t => t.Product_Lot)
                .HasMaxLength(50);

            Property(t => t.Barcode)
                .HasMaxLength(50);

            Property(t => t.LocationID);

            // Table & Column Mappings
            ToTable(schema + ".cc_cyclecount_detail");
            Property(t => t.CyclecountDetailID).HasColumnName("CyclecountDetailID");
            Property(t => t.CycleCountID).HasColumnName("CycleCountID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.ProductUnitID).HasColumnName("ProductUnitID");
            Property(t => t.Product_Lot).HasColumnName("Product_Lot");
            Property(t => t.Barcode).HasColumnName("Barcode");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.CountingStockQty).HasColumnName("CountingStockQty");
            Property(t => t.DiffQty).HasColumnName("DiffQty");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.DetailStatus).HasColumnName("DetailStatus");
            Property(t => t.PalletCode).HasColumnName("PalletCode");

            // Relationships
            HasOptional(t => t.Cyclecount)
                .WithMany(t => t.CycCountDetail)
                .HasForeignKey(d => d.CycleCountID);
        }
    }
}
