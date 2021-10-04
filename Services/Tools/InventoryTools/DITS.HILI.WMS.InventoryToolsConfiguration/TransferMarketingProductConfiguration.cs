using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class TransferMarketingProductConfiguration : EntityTypeConfiguration<TRMTransferMarketingProduct>
    {
        public TransferMarketingProductConfiguration() : this("dbo")
        {

        }
        public TransferMarketingProductConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.TRM_Product_ID);

            // Properties
            // Table & Column Mappings
            ToTable("TRM_Transfer_Marketing_Product");
            Property(t => t.TRM_Product_ID).HasColumnName("TRM_Product_ID");
            Property(t => t.TRM_ID).HasColumnName("TRM_ID");
            Property(t => t.Product_ID).HasColumnName("Product_ID");
            Property(t => t.TransferQty).HasColumnName("TransferQty");
            Property(t => t.TransferUnitID).HasColumnName("TransferUnitID");
            Property(t => t.ProductStatusID).HasColumnName("ProductStatusID");
            Property(t => t.TransferBaseQty).HasColumnName("TransferBaseQty");
            Property(t => t.TransferBaseUnitID).HasColumnName("TransferBaseUnitID");
            Property(t => t.PickQty).HasColumnName("PickQty");
            Property(t => t.ConfirmQty).HasColumnName("ConfirmQty");
            Property(t => t.PickStatus).HasColumnName("PickStatus");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasRequired(t => t.TRMTransferMarketing)
                .WithMany(t => t.TRMTransferMarketingProduct)
                .HasForeignKey(d => d.TRM_ID);
        }
    }
}
