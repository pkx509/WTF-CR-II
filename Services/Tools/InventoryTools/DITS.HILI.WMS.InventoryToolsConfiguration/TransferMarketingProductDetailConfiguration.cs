using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class TransferMarketingProductDetailConfiguration : EntityTypeConfiguration<TRMTransferMarketingProductDetail>
    {
        public TransferMarketingProductDetailConfiguration() : this("dbo")
        {

        }
        public TransferMarketingProductDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.TRM_Product_Detail_ID);

            // Properties
            Property(t => t.PalletCode)
                .HasMaxLength(40);

            Property(t => t.NewPalletCode)
                .HasMaxLength(40);

            Property(t => t.Remark)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable("TRM_Transfer_Marketing_Product_Detail");
            Property(t => t.TRM_Product_Detail_ID).HasColumnName("TRM_Product_Detail_ID");
            Property(t => t.TRM_Product_ID).HasColumnName("TRM_Product_ID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.PalletQty).HasColumnName("PalletQty");
            Property(t => t.PalletUnitID).HasColumnName("PalletUnitID");
            Property(t => t.PalletBaseUnitID).HasColumnName("PalletBaseUnitID");
            Property(t => t.PalletBaseQty).HasColumnName("PalletBaseQty");
            Property(t => t.PickQty).HasColumnName("PickQty");
            Property(t => t.ConfirmPickQty).HasColumnName("ConfirmPickQty");
            Property(t => t.LotNo).HasColumnName("LotNo");
            Property(t => t.PalletCode).HasColumnName("PalletCode");
            Property(t => t.NewPalletCode).HasColumnName("NewPalletCode");
            Property(t => t.PickStatus).HasColumnName("PickStatus");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasRequired(t => t.TRMTransferMarketingProduct)
                .WithMany(t => t.TRMTransferMarketingProductDetail)
                .HasForeignKey(d => d.TRM_Product_ID);
        }
    }
}
