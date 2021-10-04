using DITS.HILI.WMS.Stock.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.MasterConfiguration.Stock
{
    public class StockBalanceConfiguration : EntityTypeConfiguration<StockBalance>
    {
        public StockBalanceConfiguration()
            : this("dbo")
        { }

        public StockBalanceConfiguration(string schema)
        {
            ToTable(schema + ".stock_balance");

            HasKey(x => x.StockBalanceID);

            Property(x => x.StockBalanceID).HasColumnName("StockBalanceID").HasColumnType("uniqueidentifier")
                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StockInfoID).IsRequired().HasColumnName("StockInfoID").HasColumnType("uniqueidentifier");
            Property(x => x.LocationID).IsRequired().HasColumnName("LocationID").HasColumnType("uniqueidentifier");
            Property(x => x.PalletCode).HasColumnName("PalletCode").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.BaseQuantity).IsRequired().HasColumnName("BaseQuantity").HasColumnType("decimal");
            Property(x => x.ReserveQuantity).IsRequired().HasColumnName("ReserveQuantity").HasColumnType("decimal");
            Property(x => x.ConversionQty).IsRequired().HasColumnName("ConversionQty").HasColumnType("decimal");

            HasRequired(x => x.Location).WithMany(x => x.StockBalanceCollection).HasForeignKey(x => x.LocationID).WillCascadeOnDelete(false);
            HasRequired(x => x.StockInfo).WithMany(x => x.StockBalanceCollection).HasForeignKey(x => x.StockInfoID).WillCascadeOnDelete(true);
        }
    }
}
