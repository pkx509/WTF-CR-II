using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineConfiguration
{
    public class ModuleGroupConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ModuleGroup>
    {
        public ModuleGroupConfiguration()
            : this("dbo")
        {
        }

        public ModuleGroupConfiguration(string schema)
        {
            ToTable(schema + ".core_module_group");
            HasKey(x => x.ModuleGroupID);

            Property(x => x.ModuleGroupID).IsRequired().HasColumnName("ActivityID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.ModuleTypeID).HasColumnName("ModuleTypeID").HasColumnType("uniqueidentifier");

            HasRequired(x => x.ModuleType).WithMany(x => x.ModuleGroupCollection).HasForeignKey(x => x.ModuleTypeID);

        }
    }
}
