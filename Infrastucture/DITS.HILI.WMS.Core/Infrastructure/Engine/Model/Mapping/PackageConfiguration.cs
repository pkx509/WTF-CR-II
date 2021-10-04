using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model.Mapping
{
    public class PackageConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Package>
    {
        public PackageConfiguration()
            : this("dbo")
        {
        }

        public PackageConfiguration(string schema)
        {
            ToTable(schema + ".core_package");
            HasKey(x => x.PackageID);

            Property(x => x.PackageID).IsRequired().HasColumnName("PackageID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Sequence).HasColumnName("Sequene").HasColumnType("int");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.PackageCategoryID).HasColumnName("PackageCategoryID").HasColumnType("uniqueidentifier");

            HasRequired(x => x.PackageCategory).WithMany(x => x.PackageCollection).HasForeignKey(x => x.PackageCategoryID).WillCascadeOnDelete(false);

        }
    }
}
