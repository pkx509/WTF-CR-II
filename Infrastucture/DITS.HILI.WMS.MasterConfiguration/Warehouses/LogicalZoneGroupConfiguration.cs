using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogicalZoneGroupConfiguration : EntityTypeConfiguration<LogicalZoneGroup>
    {
        public LogicalZoneGroupConfiguration()
        {
            // Primary Key
            HasKey(t => t.LogicalZoneGroupID);

            // Properties
            Property(t => t.LogicalZoneGroupName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("sys_logicalzone_group");
            Property(t => t.LogicalZoneGroupID).HasColumnName("LogicalZoneGroupID");
            Property(t => t.LogicalZoneGroupName).HasColumnName("LogicalZoneGroupName");
            Property(t => t.IsActive).HasColumnName("IsActive");

        }
    }
}
