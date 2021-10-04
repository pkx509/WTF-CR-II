using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Model.Mapping
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
