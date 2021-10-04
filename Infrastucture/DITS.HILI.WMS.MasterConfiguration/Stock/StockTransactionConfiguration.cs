using DITS.HILI.WMS.MasterModel.Stock;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Stock
{
    public class StockTransactionConfiguration : EntityTypeConfiguration<StockTransaction>
    {
        public StockTransactionConfiguration()
            : this("dbo")
        { }

        public StockTransactionConfiguration(string schema)
        {


            // Primary Key
            HasKey(t => t.StockTransactionID);

            // Properties
            Property(t => t.PalletCode)
                .HasMaxLength(40);

            Property(t => t.DocumentCode)
                .HasMaxLength(20);

            // Table & Column Mappings
            ToTable(schema + ".stock_transaction");
            Property(t => t.StockTransactionID).HasColumnName("StockTransactionID");
            Property(t => t.DocumentID).HasColumnName("DocumentID");
            Property(t => t.PackageID).HasColumnName("PackageID");
            Property(t => t.StockTransType).HasColumnName("StockTransType");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.StockLocationID).HasColumnName("StockLocationID");
            Property(t => t.DocumentCode).HasColumnName("DocumentCode");
            Property(t => t.DocumentTypeID).HasColumnName("DocumentTypeID");
            Property(t => t.IsStockNonCalculate);

            // Relationships
            HasOptional(t => t.StockLocationBalance)
                .WithMany(t => t.StockTransactionCollection)
                .HasForeignKey(d => d.StockLocationID);

            HasRequired(t => t.Location)
                .WithMany(t => t.StockTransactionCollection)
                .HasForeignKey(d => d.LocationID);

        }
    }

}
