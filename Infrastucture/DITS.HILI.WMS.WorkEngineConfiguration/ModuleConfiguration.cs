using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineConfiguration
{
    public class ModuleConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Module>
    {
        public ModuleConfiguration()
            : this("dbo")
        {
        }

        public ModuleConfiguration(string schema)
        {
            ToTable(schema + ".core_module");
            HasKey(x => x.ModuleID);

            Property(x => x.ModuleID).IsRequired().HasColumnName("ActivityID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.ModuleGroupID).HasColumnName("ModuleGroupID").HasColumnType("int");

            HasRequired(x => x.ModuleGroup).WithMany(x => x.ModuleCollection).HasForeignKey(x => x.ModuleGroupID);

        }
    }
}
