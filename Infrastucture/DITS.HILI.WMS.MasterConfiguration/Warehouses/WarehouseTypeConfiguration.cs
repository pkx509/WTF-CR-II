using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class WarehouseTypeConfiguration : EntityTypeConfiguration<WarehouseType>
    {
        public WarehouseTypeConfiguration()
            : this("dbo")
        {
        }
        public WarehouseTypeConfiguration(string schema)
        {
            ToTable(schema + ".sys_warehouse_type");
            HasKey(x => x.WarehouseTypeID);

            Property(x => x.WarehouseTypeID).IsRequired().HasColumnName("ID").HasColumnType("uniqueidentifier")
                                             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.KeyType).IsRequired().HasColumnName("KeyType").HasColumnType("int");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

        }
    }
}
