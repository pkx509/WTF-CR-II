using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class QueueDockConfiguration : EntityTypeConfiguration<QueueDock>
    {
        public QueueDockConfiguration() : this("dbo")
        { }
        public QueueDockConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.QueueDockID);
            // Properties
            // Table & Column Mappings
            ToTable(schema + ".queue_dock");
            Property(t => t.QueueDockID).HasColumnName(@"DockID"); ;
            Property(t => t.QueueDockName).HasColumnName(@"DockName").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(50);
            Property(t => t.QueueDockDesc).HasColumnName(@"DockDesc").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(255);
            Property(t => t.IsActive).HasColumnName(@"IsActive");
            Property(t => t.UserCreated).HasColumnName(@"UserCreated");
            Property(t => t.DateCreated).HasColumnName(@"DateCreated");
            Property(t => t.UserModified).HasColumnName(@"UserModified");
            Property(t => t.DateModified).HasColumnName(@"DateModified");
        }
    }
}
