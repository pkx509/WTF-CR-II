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
    public class StockTransactionConfiguration : EntityTypeConfiguration<StockTransaction>
    {
        public StockTransactionConfiguration()
            : this("dbo")
        { }

        public StockTransactionConfiguration(string schema)
        {
            ToTable(schema + ".stock_transaction");

            HasKey(x => x.StockTransID);

            Property(x => x.StockTransID).IsRequired().HasColumnName("StockTransactionID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StockBalanceID).IsRequired().HasColumnName("StockBalanceID").HasColumnType("uniqueidentifier");
            Property(x => x.DocumentID).IsRequired().HasColumnName("DocumentID").HasColumnType("uniqueidentifier");
            Property(x => x.PackageID).IsRequired().HasColumnName("PackageID").HasColumnType("uniqueidentifier");
            Property(x => x.StockTransactionType).IsRequired().HasColumnName("StockTransType").HasColumnType("int");
            Property(x => x.LocationID).IsRequired().HasColumnName("LocationID").HasColumnType("uniqueidentifier");
            Property(x => x.PalletCode).HasColumnName("PalletCode").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.BaseQuantity).IsRequired().HasColumnName("BaseQuantity").HasColumnType("decimal");
            Property(x => x.ConversionQty).IsRequired().HasColumnName("ConversionQty").HasColumnType("decimal");

            HasRequired(x => x.StockBalance).WithMany(x => x.StockTransactionCollection).HasForeignKey(x => x.StockBalanceID).WillCascadeOnDelete(true);
            HasRequired(x => x.Location).WithMany(x => x.StockTransactionCollection).HasForeignKey(x => x.LocationID).WillCascadeOnDelete(false);
        }
    }

}
