using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class ReclassifiedConfiguration : EntityTypeConfiguration<Reclassified>
    {
        public ReclassifiedConfiguration() : this("dbo")
        {

        }
        public ReclassifiedConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ReclassifiedID);

            // Properties
            Property(t => t.ReclassifiedCode)
                .IsRequired()
                .HasMaxLength(20);

            Property(t => t.ReclassFromLot)
                .HasMaxLength(20);

            Property(t => t.ReclassToLot)
                .HasMaxLength(20);

            Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable(schema + ".qa_reclassified");
            Property(t => t.ReclassifiedID).HasColumnName("ReclassifiedID");
            Property(t => t.ReclassifiedCode).HasColumnName("ReclassifiedCode");
            Property(t => t.ReclassFromLot).HasColumnName("ReclassFromLot");
            Property(t => t.ReclassToLot).HasColumnName("ReclassToLot");
            Property(t => t.ReclassStatus).HasColumnName("ReclassStatus");
            Property(t => t.ApproveDate).HasColumnName("ApproveDate");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.MFGTimeFrom).HasColumnName("MFGTimeFrom");
            Property(t => t.MFGTimeEnd).HasColumnName("MFGTimeEnd");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.LineID).HasColumnName("LineID");
            Property(t => t.MFGDate).HasColumnName("MFGDate");
            Property(t => t.EXPDate).HasColumnName("EXPDate");
            Property(t => t.ProductStatusID).HasColumnName("ProductStatusID");
            Property(t => t.FromProductStatusID).HasColumnName("FromProductStatusID");
        }
    }
}
