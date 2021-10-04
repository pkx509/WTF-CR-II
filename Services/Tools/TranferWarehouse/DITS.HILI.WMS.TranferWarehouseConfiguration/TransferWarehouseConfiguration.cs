
using DITS.HILI.WMS.TransferWarehouseModel;
using System.Data.Entity.ModelConfiguration;


namespace DITS.HILI.WMS.TransferWarehouseConfiguration
{
    public class TransferWarehouseConfiguration : EntityTypeConfiguration<TransferWarehouse>
    {
        public TransferWarehouseConfiguration()
            : this("dbo")
        {
        }

        public TransferWarehouseConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.TranID);


            // Table & Column Mappings
            ToTable(schema + ".tfw_transfer_warehouse");
            Property(t => t.TranID).HasColumnName("TranID");
            Property(t => t.FromWarehouseID).HasColumnName("FromWarehouseID");
            Property(t => t.ToWarehouseID).HasColumnName("ToWarehouseID");
            Property(t => t.TransferWarehouseStatus).HasColumnName("TransferStatusID").HasColumnType("int"); ;
            Property(t => t.CloseDTTrans).HasColumnName("CloseDTTrans");
            Property(t => t.StartDTTrans).HasColumnName("StartDTTrans");
            Property(t => t.TruckID).HasColumnName("TruckID");
            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Relationships
            //this.HasOptional(t => t.Truck)
            //    .WithMany(t => t.tfw_transfer_warehouse)
            //    .HasForeignKey(d => d.TruckID);
        }
    }
}
