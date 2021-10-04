using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class TransferMarketingConfiguration : EntityTypeConfiguration<TRMTransferMarketing>
    {
        public TransferMarketingConfiguration() : this("dbo")
        {

        }
        public TransferMarketingConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.TRM_ID);

            // Properties
            Property(t => t.TRM_CODE)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            ToTable("TRM_Transfer_Marketing");
            Property(t => t.TRM_ID).HasColumnName("TRM_ID");
            Property(t => t.TRM_CODE).HasColumnName("TRM_CODE");
            Property(t => t.TransferDate).HasColumnName("TransferDate");
            Property(t => t.ApproveDate).HasColumnName("ApproveDate");
            Property(t => t.TransferStatus).HasColumnName("TransferStatus");
            Property(t => t.Description).HasColumnName("Description");
        }
    }
}
