using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        public LocationConfiguration()
            : this("dbo")
        {
        }

        public LocationConfiguration(string schema)
        {
            ToTable(schema + ".sys_location");
            HasKey(x => x.LocationID);

            Property(x => x.LocationID).IsRequired().HasColumnName("LocationID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Code).IsRequired().HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(50)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.ZoneID).IsRequired().HasColumnName("ZoneID").HasColumnType("uniqueidentifier");
            Property(x => x.LocationType).HasColumnName("LocationType").HasColumnType("int")
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.RowNo).IsRequired().HasColumnName("RowNo").HasColumnType("int");
            Property(x => x.ColumnNo).IsRequired().HasColumnName("ColumnNo").HasColumnType("int");
            Property(x => x.LevelNo).IsRequired().HasColumnName("LevelNo").HasColumnType("int");
            Property(x => x.Height).IsRequired().HasColumnName("Height").HasColumnType("decimal");
            Property(x => x.Width).IsRequired().HasColumnName("Width").HasColumnType("decimal");
            Property(x => x.Length).IsRequired().HasColumnName("Length").HasColumnType("decimal");
            Property(x => x.Weight).IsRequired().HasColumnName("Weight").HasColumnType("decimal");
            Property(x => x.ReserveWeight).HasColumnName(@"ReserveWeight").HasColumnType("decimal");
            Property(x => x.PalletCapacity).HasColumnName(@"PalletCapacity").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.SizeCapacity).HasColumnName(@"SizeCapacity").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);
            Property(x => x.LocationReserveQty).HasColumnName(@"LocationReserveQty").HasColumnType("decimal").IsOptional().HasPrecision(18, 2);

            Property(x => x.IsBlock).IsRequired().HasColumnName("IsBlock").HasColumnType("bit");
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            HasRequired(x => x.Zone).WithMany(x => x.LocationCollection).HasForeignKey(x => x.ZoneID).WillCascadeOnDelete(false);
            HasOptional(a => a.PalletType).WithMany(b => b.LocationCollection).HasForeignKey(c => c.PalletTypeID);
        }
    }
}
