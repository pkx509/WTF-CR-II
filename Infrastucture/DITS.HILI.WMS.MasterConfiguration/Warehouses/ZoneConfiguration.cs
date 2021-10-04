using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ZoneConfiguration : EntityTypeConfiguration<Zone>
    {
        public ZoneConfiguration()
            : this("dbo")
        {
        }

        public ZoneConfiguration(string schema)
        {
            ToTable(schema + ".sys_zone");
            HasKey(x => x.ZoneID);

            Property(x => x.ZoneID).IsRequired().HasColumnName("ID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Code).IsRequired().HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(50)
                            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute())); ;

            Property(x => x.ZoneTypeID).IsRequired().HasColumnName("ZoneTypeID").HasColumnType("uniqueidentifier");
            Property(x => x.WarehouseID).IsRequired().HasColumnName("WarehouseID").HasColumnType("uniqueidentifier");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ShortName).HasColumnName("ShortName").HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsBlock).IsRequired().HasColumnName("IsBlock").HasColumnType("bit");

            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");

            HasRequired(x => x.ZoneType).WithMany(x => x.ZoneCollection).HasForeignKey(x => x.ZoneTypeID).WillCascadeOnDelete(false);
            HasRequired(x => x.Warehouse).WithMany(x => x.ZoneCollection).HasForeignKey(x => x.WarehouseID).WillCascadeOnDelete(false);

        }
    }
}
