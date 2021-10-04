using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class PhysicalZoneConfiguration : EntityTypeConfiguration<PhysicalZone>
    {
        public PhysicalZoneConfiguration()
            : this("dbo")
        {

        }

        public PhysicalZoneConfiguration(string schema)
        {
            ToTable(schema + ".sys_physicalzone");
            HasKey(x => x.Physicalzone_Id);
            Property(x => x.Physicalzone_Id).HasColumnName(@"Physicalzone_ID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PhysicalZone_Code).HasColumnName(@"PhysicalZone_Code").HasColumnType("nvarchar").HasMaxLength(10)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;
            Property(x => x.Warehouse_Code).HasColumnName(@"Warehouse_Code").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.ZoneType_Code).HasColumnName(@"ZoneType_Code").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.PhysicalZone_Name).HasColumnName(@"PhysicalZone_Name").HasColumnType("nvarchar").IsOptional().HasMaxLength(25);
            Property(x => x.PhysicalZone_Short_Name).HasColumnName(@"PhysicalZone_Short_Name").HasColumnType("nvarchar").IsOptional().HasMaxLength(2);
            Property(x => x.PhysicalZone_Status).HasColumnName(@"PhysicalZone_Status").HasColumnType("int").IsOptional();
            Property(x => x.CreateUser).HasColumnName(@"Create_User").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.CreateDate).HasColumnName(@"Create_Date").HasColumnType("datetime").IsOptional();
            Property(x => x.UpdateUser).HasColumnName(@"Update_User").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.UpdateDate).HasColumnName(@"Update_Date").HasColumnType("datetime").IsOptional();
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.Theme).HasColumnName(@"Theme").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(10);
            Property(x => x.Width).HasColumnName(@"Width").HasColumnType("decimal").IsOptional().HasPrecision(6, 2);
            Property(x => x.Height).HasColumnName(@"Height").HasColumnType("decimal").IsOptional().HasPrecision(6, 2);
            Property(x => x.XAxis).HasColumnName(@"X_Axis").HasColumnType("decimal").IsOptional().HasPrecision(6, 2);
            Property(x => x.YAxis).HasColumnName(@"Y_Axis").HasColumnType("decimal").IsOptional().HasPrecision(6, 2);
        }
    }
}
