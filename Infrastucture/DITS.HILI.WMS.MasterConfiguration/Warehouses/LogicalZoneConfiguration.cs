using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogicalZoneConfiguration : EntityTypeConfiguration<LogicalZone>
    {
        public LogicalZoneConfiguration()
        {
            // Primary Key
            HasKey(t => t.LogicalZoneID);

            // Properties
            Property(t => t.LogicalZoneName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("sys_logicalzone");
            Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            Property(t => t.ZoneID).HasColumnName("ZoneID");
            Property(t => t.LogicalZoneName).HasColumnName("LogicalZoneName");
            Property(t => t.LogicalZoneAutoLocation).HasColumnName("LogicalZoneAutoLocation");
            Property(t => t.ZoneBalanceLocation).HasColumnName("ZoneBalanceLocation");
            Property(t => t.LogicalZoneStatus).HasColumnName("LogicalZoneStatus");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.IsPallet).HasColumnName("IsPallet");

            // Relationships
            HasOptional(t => t.Zone)
                .WithMany(t => t.LogicalZone)
                .HasForeignKey(d => d.ZoneID);


            //// Properties
            //this.Property(t => t.LogicalZoneName)
            //    .IsRequired()
            //    .HasMaxLength(50);

            //// Table & Column Mappings
            //this.ToTable("sys_logicalzone");
            //// Primary Key
            //HasKey(x => x.LogicalZoneID);
            //this.Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Property(t => t.ZoneID).HasColumnName("ZoneID");
            //this.Property(t => t.LogicalZoneName).HasColumnName("LogicalZoneName");
            //this.Property(t => t.LogicalZoneAutoLocation).HasColumnName("LogicalZoneAutoLocation");
            //this.Property(t => t.ZoneBalanceLocation).HasColumnName("ZoneBalanceLocation");
            //this.Property(t => t.LogicalZoneStatus).HasColumnName("LogicalZoneStatus");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            //this.Property(t => t.UserModified).HasColumnName("UserModified");
            //this.Property(t => t.DateModified).HasColumnName("DateModified");
            //this.Property(t => t.IsActive).HasColumnName("IsActive");
            //this.Property(t => t.IsPallet).HasColumnName("IsPallet");

            //// Relationships
            //this.HasOptional(t => t.Zone)
            //    .WithMany(t => t.LogicalZone)
            //    .HasForeignKey(d => d.ZoneID);

        }
    }
}
