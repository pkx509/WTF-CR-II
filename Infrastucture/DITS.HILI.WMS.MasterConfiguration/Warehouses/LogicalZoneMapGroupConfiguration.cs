using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogicalZoneMapGroupConfiguration : EntityTypeConfiguration<LogicalZoneMapGroup>
    {
        public LogicalZoneMapGroupConfiguration()
            : this("dbo")
        {
        }

        public LogicalZoneMapGroupConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => new { t.LogicalZoneID, t.LogicalZoneGroupID });

            // Properties
            // Table & Column Mappings
            ToTable("sys_logicalzone_map_group");
            Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            Property(t => t.LogicalZoneGroupID).HasColumnName("LogicalZoneGroupID");
            Property(t => t.PutAwayRuleID).HasColumnName("PutAwayRuleID");
            Property(t => t.Seq).HasColumnName("Seq");
            Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            HasRequired(t => t.LogicalZone)
                .WithMany(t => t.LogicalZoneMapGroup)
                .HasForeignKey(d => d.LogicalZoneID);
            HasRequired(t => t.LogicalZoneGroup)
                .WithMany(t => t.LogicalZoneMapGroup)
                .HasForeignKey(d => d.LogicalZoneGroupID);
            HasOptional(t => t.SpecialPutawayRule)
                .WithMany(t => t.LogicalZoneMapGroup)
                .HasForeignKey(d => d.PutAwayRuleID);
        }
    }
}
