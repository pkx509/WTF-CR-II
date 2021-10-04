using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model.Mapping
{
    public class PackageCategoryConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PackageCategory>
    {
        public PackageCategoryConfiguration()
            : this("dbo")
        {
        }

        public PackageCategoryConfiguration(string schema)
        {
            ToTable(schema + ".core_package_category");
            HasKey(x => x.PackageCategoryID);

            Property(x => x.PackageCategoryID).IsRequired().HasColumnName("PackageCategoryID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.PackageTypeID).HasColumnName("PackageTypeID").HasColumnType("uniqueidentifier");

           HasRequired(x => x.PackageType).WithMany(x => x.PackageCategoryCollection).HasForeignKey(x => x.PackageTypeID).WillCascadeOnDelete(false);

        }
    }
}
