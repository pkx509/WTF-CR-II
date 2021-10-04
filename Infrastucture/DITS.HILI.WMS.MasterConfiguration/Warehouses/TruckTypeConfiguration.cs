using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Warehouses
{
    public class TruckTypeConfiguration : EntityTypeConfiguration<TruckType>
    {
        public TruckTypeConfiguration()
            : this("dbo")
        {
        }

        public TruckTypeConfiguration(string schema)
        {
            ToTable("sys_truck_type", schema);
            HasKey(x => x.TruckTypeID);

            Property(x => x.TruckTypeID).HasColumnName(@"TruckTypeID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TypeName).HasColumnName(@"Type_name").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Description).HasColumnName(@"Type_Description").HasColumnType("nvarchar").IsOptional().HasMaxLength(500);
            Property(x => x.EsitmateTime).HasColumnName(@"Type_EsitmateTime").HasColumnType("int").IsOptional();
            Property(x => x.EquipmentFlag).HasColumnName(@"EquipmentFlag").HasColumnType("bit").IsOptional();
            Property(x => x.IsTransferWH).HasColumnName(@"IsTransferWH").HasColumnType("bit").IsOptional();
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsOptional();
            Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.UserCreated).HasColumnName(@"UserCreated").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.UserModified).HasColumnName(@"UserModified").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateModified).HasColumnName(@"DateModified").HasColumnType("datetime").IsRequired();
        }
    }
}
