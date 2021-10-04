using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogicalZoneConfigConfiguration : EntityTypeConfiguration<LogicalZoneConfig>
    {
        public LogicalZoneConfigConfiguration()
        {
            // Primary Key
            HasKey(t => t.LogicalConfigID);

            // Properties
            Property(t => t.ConfigValue)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable("sys_logicalzone_config");
            Property(t => t.LogicalConfigID).HasColumnName("LogicalConfigID");
            Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            Property(t => t.ConfigID).HasColumnName("ConfigID");
            Property(t => t.ConfigValue).HasColumnName("ConfigValue");
            Property(t => t.ConfigValueID).HasColumnName("ConfigValueID");
            Property(t => t.PrioritySeq).HasColumnName("PrioritySeq");
            Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            HasRequired(t => t.LogicalZone)
                .WithMany(t => t.LogicalZoneConfig)
                .HasForeignKey(d => d.LogicalZoneID);

            //// Properties
            //this.Property(t => t.ConfigValue)
            //    .HasMaxLength(30);

            //// Table & Column Mappings
            //this.ToTable("sys_logicalzone_config");
            //// Primary Key
            //HasKey(x => x.LogicalConfigID);
            //this.Property(t => t.LogicalConfigID).HasColumnName("LogicalConfigID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            //this.Property(t => t.ConfigID).HasColumnName("ConfigID");
            //this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");
            //this.Property(t => t.PrioritySeq).HasColumnName("PrioritySeq");
            //this.Property(t => t.IsActive).HasColumnName("IsActive");

            //// Relationships
            //this.HasRequired(t => t.LogicalZone)
            //    .WithMany(t => t.LogicalZoneConfig)
            //    .HasForeignKey(d => d.LogicalZoneID);

        }
    }
}
