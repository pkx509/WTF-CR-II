using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class QueueTypeConfiguration : EntityTypeConfiguration<QueueRegisterType>
    {
        public QueueTypeConfiguration() : this("dbo")
        { }
        public QueueTypeConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.QueueRegisterTypeID);
            // Properties
            // Table & Column Mappings
            ToTable(schema + ".queue_type");
            Property(t => t.QueueRegisterTypeID).HasColumnName(@"TypeID");//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.QueueRegisterTypeName).HasColumnName(@"TypeName").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(50);
            Property(t => t.QueueRegisterTypeDesc).HasColumnName(@"TypeDesc").HasColumnType("varchar").IsRequired().IsUnicode(true).HasMaxLength(255);
            Property(t => t.IsActive).HasColumnName(@"IsActive");
            Property(t => t.UserCreated).HasColumnName(@"UserCreated");
            Property(t => t.DateCreated).HasColumnName(@"DateCreated");
            Property(t => t.UserModified).HasColumnName(@"UserModified");
            Property(t => t.DateModified).HasColumnName(@"DateModified");
        }
    }
}
