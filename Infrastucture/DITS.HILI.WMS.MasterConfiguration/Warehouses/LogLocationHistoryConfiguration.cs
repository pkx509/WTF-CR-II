using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogLocationHistoryConfiguration : EntityTypeConfiguration<LogLocationHistory>
    {
        public LogLocationHistoryConfiguration()
            : this("dbo")
        {
        }

        public LogLocationHistoryConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.LocationHistoryID);

            // Properties
            Property(t => t.OrderNo)
                .HasMaxLength(50);

            Property(t => t.Lot)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("log_location_history");
            Property(t => t.LocationHistoryID).HasColumnName("LocationHistoryID");
            Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.OrderNo).HasColumnName("OrderNo");
            Property(t => t.PackingID).HasColumnName("PackingID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.Lot).HasColumnName("Lot");
            Property(t => t.MFGDate).HasColumnName("MFGDate");
            Property(t => t.StockQty).HasColumnName("StockQty");
            Property(t => t.BasicQty).HasColumnName("BasicQty");
            Property(t => t.BalanceStockQty).HasColumnName("BalanceStockQty");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.BasicUnitID).HasColumnName("BasicUnitID");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasRequired(t => t.LogicalZone)
                .WithMany(t => t.LogLocationHistory)
                .HasForeignKey(d => d.LogicalZoneID);
        }
    }
}
