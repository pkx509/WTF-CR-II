using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LogicalZoneDetailConfiguration : EntityTypeConfiguration<LogicalZoneDetail>
    {
        public LogicalZoneDetailConfiguration()
        {
            // Primary Key
            HasKey(t => new { t.LogicalZoneDetailID });

            // Properties
            Property(t => t.Seq)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("sys_logicalzone_detail");
            Property(t => t.LogicalZoneDetailID).HasColumnName("LogicalZoneDetailID");
            Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.Seq).HasColumnName("Seq");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.AvailableQty).HasColumnName("AvailableQty");
            Property(t => t.ReserveQty).HasColumnName("ReserveQty");

            // Relationships
            HasRequired(t => t.Location)
                .WithMany(t => t.LogicalZoneDetail)
                .HasForeignKey(d => d.LocationID);
            HasRequired(t => t.LogicalZone)
                .WithMany(t => t.LogicalZoneDetail)
                .HasForeignKey(d => d.LogicalZoneID);


            //// Table & Column Mappings
            //this.ToTable("sys_logicalzone_detail");
            //// Primary Key
            //HasKey(x => x.LogicalZoneDetailID);
            //this.Property(t => t.LogicalZoneDetailID).HasColumnName("LogicalZoneDetailID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            //this.Property(t => t.LocationID).HasColumnName("LocationID");
            //this.Property(t => t.Seq).HasColumnName("Seq");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            //this.Property(t => t.UserModified).HasColumnName("UserModified");
            //this.Property(t => t.DateModified).HasColumnName("DateModified");
            //this.Property(t => t.IsActive).HasColumnName("IsActive");

            //// Relationships
            //this.HasRequired(t => t.Location)
            //    .WithMany(t => t.LogicalZoneDetail)
            //    .HasForeignKey(d => d.LocationID);
            //this.HasRequired(t => t.LogicalZone)
            //    .WithMany(t => t.LogicalZoneDetail)
            //    .HasForeignKey(d => d.LogicalZoneID);



        }
    }
}
