using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class QueueConfigConfiguration : EntityTypeConfiguration<TruckQueueModel.QueueConfiguration>
    {
        public QueueConfigConfiguration() : this("dbo")
        { }
        public QueueConfigConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ConfigurationID);

            // Properties
            // Table & Column Mappings
            ToTable(schema + ".queue_config");
            Property(t => t.ConfigurationID).HasColumnName(@"ConfigurationId");//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Message).HasColumnName(@"Message").HasColumnType("varchar").IsRequired().IsUnicode(true);
            Property(t => t.StartHour).HasColumnName(@"StartHour");
            Property(t => t.StartMinute).HasColumnName(@"StartMinute");
            Property(t => t.EnableMessage).HasColumnName(@"EnableMessage");
            Property(t => t.IsActive).HasColumnName(@"IsActive");
            Property(t => t.UserCreated).HasColumnName(@"UserCreated");
            Property(t => t.DateCreated).HasColumnName(@"DateCreated");
            Property(t => t.UserModified).HasColumnName(@"UserModified");
            Property(t => t.DateModified).HasColumnName(@"DateModified");
        }
    }
}
