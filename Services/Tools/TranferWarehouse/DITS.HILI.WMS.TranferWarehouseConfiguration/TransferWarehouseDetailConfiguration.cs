using DITS.HILI.WMS.TransferWarehouseModel;
using System.Data.Entity.ModelConfiguration;


namespace DITS.HILI.WMS.TransferWarehouseConfiguration
{
    public class TransferWarehouseDetailConfiguration : EntityTypeConfiguration<TransferWarehouseDetail>
    {
        public TransferWarehouseDetailConfiguration() : this("dbo")
        {
        }
        public TransferWarehouseDetailConfiguration(string schema)
        { // Primary Key
            HasKey(t => t.TranDetailID);

            // Properties
            Property(t => t.PalletCode)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable(schema + ".tfw_tran_warehouse_detail");
            Property(t => t.TranDetailID).HasColumnName("TranDetailID");
            Property(t => t.TranID).HasColumnName("TranID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.FromLocationID).HasColumnName("FromLocationID");
            Property(t => t.ToLocationID).HasColumnName("ToLocationID");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.ConversionQty).HasColumnName("ConversionQty");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.ProductOwnerID).HasColumnName("ProductOwnerID");
            Property(t => t.SupplierID).HasColumnName("SupplierID");
            Property(t => t.ProductStatusID).HasColumnName("ProductStatusID");
            Property(t => t.ProductSubStatusID).HasColumnName("ProductSubStatusID");
            Property(t => t.ReferenceID).HasColumnName("ReferenceID");
            Property(t => t.PackageID).HasColumnName("PackageID");
            Property(t => t.StartDT).HasColumnName("StartDT");
            Property(t => t.FinishDT).HasColumnName("FinishDT");
            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            HasOptional(t => t.TransferWarehouse)
                .WithMany(t => t.TransferWarehouseDetailCollection)
                .HasForeignKey(d => d.TranID);
        }
    }
}