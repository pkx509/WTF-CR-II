using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{

    public class AdjustConfiguration : EntityTypeConfiguration<Adjust>
    {
        public AdjustConfiguration() : this("dbo")
        {

        }
        public AdjustConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.AdjustID);

            // Properties
            Property(t => t.AdjustCode)
                .IsRequired()
                .HasMaxLength(20);

            Property(t => t.ReferenceDoc)
                .HasMaxLength(20);

            Property(t => t.Remark)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable("adj_adjust");
            Property(t => t.AdjustID).HasColumnName("AdjustID");
            Property(t => t.AdjustCode).HasColumnName("AdjustCode");
            Property(t => t.AdjustTypeID).HasColumnName("AdjustTypeID");
            Property(t => t.IsEffect).HasColumnName("IsEffect");
            Property(t => t.AdjustStatus).HasColumnName("AdjustStatus");
            Property(t => t.AdjustStartDate).HasColumnName("AdjustStartDate");
            Property(t => t.AdjustCompleteDate).HasColumnName("AdjustCompleteDate");
            Property(t => t.ReferenceDoc).HasColumnName("ReferenceDoc");
        }
    }
}
