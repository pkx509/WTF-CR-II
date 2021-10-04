using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model.Mapping
{
    public class PackageWorkFlowConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PackageWorkFlow>
    {
        public PackageWorkFlowConfiguration()
            : this("dbo")
        {
        }

        public PackageWorkFlowConfiguration(string schema)
        {
            ToTable(schema + ".core_package_workflow");
            HasKey(x => x.PackageWorkFlowID);

            Property(x => x.PackageWorkFlowID).IsRequired().HasColumnName("PackageWorkFlowID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(x => x.DocumentTypeID).IsRequired().HasColumnName("DocumentTypeID").HasColumnType("uniqueidentifier");
            Property(x => x.PackageID).IsRequired().HasColumnName("PackageID").HasColumnType("uniqueidentifier");
            Property(x => x.Method).IsRequired().HasColumnName("Method").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Sequence).HasColumnName("Sequence").HasColumnType("int");

            HasRequired(x => x.Package).WithMany(x => x.PackageWorkFlowCollection).HasForeignKey(x => x.PackageID).WillCascadeOnDelete(false);

        }
    }
}
