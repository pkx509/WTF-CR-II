using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogicalZoneGroupDetailConfiguration : EntityTypeConfiguration<LogicalZoneGroupDetail>
    {
        public LogicalZoneGroupDetailConfiguration()
        {
            // Primary Key
            HasKey(t => t.LogicalGroupDetailID);

            // Properties
            // Table & Column Mappings
            ToTable("sys_logicalzone_group_detail");
            Property(t => t.LogicalGroupDetailID).HasColumnName("LogicalGroupDetailID");
            Property(t => t.LogicalZoneGroupID).HasColumnName("LogicalZoneGroupID");
            Property(t => t.ProductGroupLevel3ID).HasColumnName("ProductGroupLevel3ID");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            HasRequired(t => t.LogicalZoneGroup)
                .WithMany(t => t.LogicalZoneGroupDetail)
                .HasForeignKey(d => d.LogicalZoneGroupID);
            HasOptional(t => t.ProductGroupLevel3)
                .WithMany(t => t.LogicalZoneGroupDetail)
                .HasForeignKey(d => d.ProductGroupLevel3ID);

        }
    }
}
