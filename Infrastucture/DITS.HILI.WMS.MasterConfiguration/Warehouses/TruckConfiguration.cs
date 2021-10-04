using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Warehouses
{
    public class TruckConfiguration : EntityTypeConfiguration<Truck>
    {
        public TruckConfiguration()
            : this("dbo")
        {
        }

        public TruckConfiguration(string schema)
        {
            ToTable("sys_truck", schema);
            HasKey(x => x.TruckID);

            Property(x => x.TruckID).HasColumnName(@"TruckID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.TruckTypeID).HasColumnName(@"TruckTypeID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.TruckNo).HasColumnName(@"TruckNo").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.ProvinceID).HasColumnName(@"ProvinceId").HasColumnType("int").IsOptional();

            Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.UserCreated).HasColumnName(@"UserCreated").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.UserModified).HasColumnName(@"UserModified").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateModified).HasColumnName(@"DateModified").HasColumnType("datetime").IsRequired();

            // Foreign keys
            HasOptional(a => a.TruckType).WithMany(b => b.TruckCollection).HasForeignKey(c => c.TruckTypeID).WillCascadeOnDelete(false); // FK_TruckTypeID
        }
    }
}
