using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Warehouses
{
    public class EquipZoneConfigConfiguration : EntityTypeConfiguration<EquipZoneConfig>
    {
        public EquipZoneConfigConfiguration()
            : this("dbo")
        {
        }

        public EquipZoneConfigConfiguration(string schema)
        {
            ToTable("sys_equip_zone_config", schema);
            HasKey(x => x.EquipID);

            Property(x => x.EquipID).HasColumnName(@"EquipID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.EquipName).HasColumnName(@"EquipName").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Barcode).HasColumnName(@"Barcode").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.Serialnumber).HasColumnName(@"Serialnumber").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.ZoneID).HasColumnName(@"ZoneID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.TruckTypeID).HasColumnName(@"TruckTypeID").HasColumnType("uniqueidentifier").IsOptional();


            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
        }

    }
}
