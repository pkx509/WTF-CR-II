using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{

    public class CycleCountConfiguration : EntityTypeConfiguration<CycleCount>
    {
        public CycleCountConfiguration() : this("dbo")
        {

        }
        public CycleCountConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.CycleCountID);

            // Properties
            Property(t => t.CycleCountCode)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            ToTable(schema + ".cc_cyclecount");
            Property(t => t.CycleCountID).HasColumnName("CycleCountID");
            Property(t => t.CycleCountCode).HasColumnName("CycleCountCode");
            Property(t => t.WarehounseID).HasColumnName("WarehounseID");
            Property(t => t.ZoneID).HasColumnName("ZoneID");
            Property(t => t.CycleCountStartDate).HasColumnName("CycleCountStartDate");
            Property(t => t.CycleCountCompleteDate).HasColumnName("CycleCountCompleteDate");
            Property(t => t.CycleCountAssignDate).HasColumnName("CycleCountAssignDate");
            Property(t => t.CycleCountStatus).HasColumnName("CycleCountStatus");
        }
    }
}
