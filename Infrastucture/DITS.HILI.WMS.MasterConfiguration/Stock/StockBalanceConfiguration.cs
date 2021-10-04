using DITS.HILI.WMS.MasterModel.Stock;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Stock
{
    public class StockBalanceConfiguration : EntityTypeConfiguration<StockBalance>
    {
        public StockBalanceConfiguration()
            : this("dbo")
        { }

        public StockBalanceConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.StockBalanceID);

            // Properties
            //this.Property(t => t.PalletCode)
            //    .HasMaxLength(20);

            // Table & Column Mappings
            ToTable(schema + ".stock_balance");
            Property(t => t.StockInfoID).HasColumnName("StockInfoID");
            //this.Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.ReserveQuantity).HasColumnName("ReserveQuantity");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.StockBalanceID).HasColumnName("StockBalanceID");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasRequired(t => t.StockInfo)
                .WithMany(t => t.StockBalanceCollection)
                .HasForeignKey(d => d.StockInfoID);
        }
    }
}
