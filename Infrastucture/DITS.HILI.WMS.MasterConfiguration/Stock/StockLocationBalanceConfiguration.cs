using DITS.HILI.WMS.MasterModel.Stock;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Stock
{
    public class StockLocationBalanceConfiguration : EntityTypeConfiguration<StockLocationBalance>
    {
        public StockLocationBalanceConfiguration()
            : this("dbo")
        { }

        public StockLocationBalanceConfiguration(string schema)
        { // Primary Key
            HasKey(t => t.StockLocationID);

            // Properties
            // Table & Column Mappings
            ToTable(schema + ".stock_location_balance");
            Property(t => t.StockLocationID).HasColumnName("StockLocationID");
            Property(t => t.StockBalanceID).HasColumnName("StockBalanceID");
            Property(t => t.WarehouseID).HasColumnName("WarehouseID");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.ReserveQuantity).HasColumnName("ReserveQuantity");
            Property(t => t.ZoneID).HasColumnName("ZoneID");

            // Relationships
            HasOptional(t => t.StockBalance)
                .WithMany(t => t.StockLocationBalanceCollection)
                .HasForeignKey(d => d.StockBalanceID);
        }
    }
}
