using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class QueueRunningConfiguration : EntityTypeConfiguration<QueueRunning>
    {
        public QueueRunningConfiguration() : this("dbo")
        { }
        public QueueRunningConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.QueueRunId);
            // Properties
            // Table & Column Mappings
            ToTable(schema + ".queue_run");
            Property(t => t.QueueRunId).HasColumnName(@"RunId");
            Property(t => t.QueuDate).HasColumnName(@"QueuDate").IsRequired();
            Property(t => t.QueueRun).HasColumnName(@"QueueRun").IsRequired();
            Property(t => t.IsActive).HasColumnName(@"IsActive");
            Property(t => t.UserCreated).HasColumnName(@"UserCreated");
            Property(t => t.DateCreated).HasColumnName(@"DateCreated");
            Property(t => t.UserModified).HasColumnName(@"UserModified");
            Property(t => t.DateModified).HasColumnName(@"DateModified");
        }
    }
}
