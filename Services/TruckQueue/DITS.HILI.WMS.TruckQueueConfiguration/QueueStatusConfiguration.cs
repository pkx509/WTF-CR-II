using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class QueueStatusConfiguration : EntityTypeConfiguration<QueueStatus>
    {
        public QueueStatusConfiguration() : this("dbo")
        { }
        public QueueStatusConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.QueueStatusID);
            // Properties
            // Table & Column Mappings
            ToTable(schema + ".queue_status");
            Property(t => t.QueueStatusID).HasColumnName(@"StatusID").HasColumnType("int");//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.QueueStatusName).HasColumnName(@"StatusName").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(50);
            Property(t => t.QueueStatusDesc).HasColumnName(@"StatusDesc").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(255);
            Property(t => t.IsActive).HasColumnName(@"IsActive"); 
            //Property(t => t.IsCancel).HasColumnName(@"DefaultCancel");
            //Property(t => t.IsCompleted).HasColumnName(@"DefaultCompleted");
            //Property(t => t.IsInQueue).HasColumnName(@"DefaultInQueue");
            //Property(t => t.IsWaiting).HasColumnName(@"DefaultWaiting");
            Property(t => t.UserCreated).HasColumnName(@"UserCreated");
            Property(t => t.DateCreated).HasColumnName(@"DateCreated");
            Property(t => t.UserModified).HasColumnName(@"UserModified");
            Property(t => t.DateModified).HasColumnName(@"DateModified");
        }
    }
}
