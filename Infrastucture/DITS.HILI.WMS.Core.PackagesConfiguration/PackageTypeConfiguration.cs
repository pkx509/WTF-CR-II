using DITS.HILI.WMS.Core.PackagesModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.Core.PackagesConfiguration
{
    public class PackageTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PackageType>
    {
        public PackageTypeConfiguration()
            : this("dbo")
        {
        }

        public PackageTypeConfiguration(string schema)
        {
            ToTable(schema + ".core_package_type");
            HasKey(x => x.PackageTypeID);

            Property(x => x.PackageTypeID).IsRequired().HasColumnName("PackageTypeID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

        }
    }
}
