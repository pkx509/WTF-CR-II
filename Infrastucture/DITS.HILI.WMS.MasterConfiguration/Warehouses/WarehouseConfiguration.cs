using DITS.HILI.WMS.MasterModel.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class WarehouseConfiguration : EntityTypeConfiguration<Warehouse>
    {
        public WarehouseConfiguration()
            : this("dbo")
        {
        }

        public WarehouseConfiguration(string schema)
        {
            ToTable(schema + ".sys_warehouse");
            HasKey(x => x.WarehouseID);
            Property(x => x.WarehouseID).IsRequired().HasColumnName("WarehouseID").HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Code).IsRequired().HasColumnName("Code").HasColumnType("nvarchar").HasMaxLength(50)
                                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ShortName).HasColumnName("ShortName").HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.WarehouseTypeID).IsRequired().HasColumnName("WarehouseTypeID").HasColumnType("uniqueidentifier");
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.ReferenceCode).HasColumnName(@"ReferenceCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(t => t.SiteID).HasColumnName("SiteID");
            Property(t => t.Seqno).HasColumnName("Seqno");
            Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(250);
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            HasRequired(x => x.WarehouseType).WithMany(x => x.WarehouseCollection).HasForeignKey(x => x.WarehouseTypeID).WillCascadeOnDelete(false);
            HasOptional(t => t.Site)
               .WithMany(t => t.WarehouseCollection)
               .HasForeignKey(d => d.SiteID);
        }
    }
}
