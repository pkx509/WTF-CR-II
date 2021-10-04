using DITS.HILI.WMS.TruckQueueModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.TruckQueueConfiguration
{
    public class QueueRegConfiguration : EntityTypeConfiguration<QueueReg>
    {
        public QueueRegConfiguration() : this("dbo")
        { }
        public QueueRegConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.QueueId);
            // Properties
            // Table & Column Mappings
            ToTable(schema + ".queue_reg");
            Property(t => t.QueueId).HasColumnName(@"QueueId");
            Property(t => t.Sequence).HasColumnName(@"Sequence").IsRequired();
            Property(t => t.QueueNo).HasColumnName(@"QueueNo").HasColumnType("varchar").IsUnicode(false).HasMaxLength(10).IsRequired();
            Property(t => t.PONO).HasColumnName(@"PONO").HasColumnType("varchar").IsUnicode(false).HasMaxLength(50).IsOptional();
            Property(t => t.QueueDate).HasColumnName(@"QueueDate").IsOptional();
            Property(t => t.TruckRegNo).HasColumnName(@"RegNo").HasColumnType("varchar").IsUnicode(false).HasMaxLength(20).IsOptional();
            //Property(t => t.TruckRegProviceId).HasColumnName(@"RegProvice").HasColumnType("uniqueidentifier").IsOptional();
            Property(t => t.TimeIn).HasColumnName(@"TimeIn").IsRequired();
            Property(t => t.TimeOut).HasColumnName(@"TimeOut").IsOptional();
            Property(x => x.ShipToId).HasColumnName(@"ShippToId").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ShipFromId).HasColumnName(@"ShipFromId").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.QueueDockID).HasColumnName(@"DockID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.QueueRegisterTypeID).HasColumnName(@"RegisterTypeID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.QueueStatusID).HasColumnName(@"StatusID").HasColumnType("int").IsRequired();
            Property(x => x.TruckTypeID).HasColumnName(@"TruckTypeID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.EstimateTime).HasColumnName(@"EstimateTime").IsOptional(); 
            Property(t => t.IsActive).HasColumnName(@"IsActive").IsRequired();
            Property(t => t.UserCreated).HasColumnName(@"UserCreated").IsRequired();
            Property(t => t.DateCreated).HasColumnName(@"DateCreated").IsRequired();
            Property(t => t.UserModified).HasColumnName(@"UserModified").IsOptional();
            Property(t => t.DateModified).HasColumnName(@"DateModified").IsOptional();
        }
    }
}
