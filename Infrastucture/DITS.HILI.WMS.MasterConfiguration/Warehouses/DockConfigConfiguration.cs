using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Warehouses
{
    public class DockConfigConfiguration : EntityTypeConfiguration<DockConfig>
    {
        public DockConfigConfiguration()
            : this("dbo")
        {
        }

        public DockConfigConfiguration(string schema)
        {
            ToTable("sys_dock_config", schema);
            HasKey(x => x.DockConfigID);

            Property(x => x.DockConfigID).HasColumnName(@"DockconfigID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DockName).HasColumnName(@"DockName").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.Barcode).HasColumnName(@"Barcode").HasColumnType("nvarchar").IsOptional().HasMaxLength(150);
            Property(x => x.WarehouseID).HasColumnName(@"WarehouseID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.TruckTypeID).HasColumnName(@"TruckTypeID").HasColumnType("uniqueidentifier").IsOptional();

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.TruckType).WithMany(b => b.DockConfigCollection).HasForeignKey(c => c.TruckTypeID).WillCascadeOnDelete(false); // FK_Truck
            HasOptional(a => a.Warehouse).WithMany(b => b.DockConfigCollection).HasForeignKey(c => c.WarehouseID).WillCascadeOnDelete(false); // FK_Warehouse
        }
    }
}
