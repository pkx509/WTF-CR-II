using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineConfiguration
{
    public class ModuleTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ModuleType>
    {
        public ModuleTypeConfiguration()
            : this("dbo")
        {
        }

        public ModuleTypeConfiguration(string schema)
        {
            ToTable(schema + ".core_module_type");
            HasKey(x => x.ModuleTypeID);

            Property(x => x.ModuleTypeID).IsRequired().HasColumnName("ActivityID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250); 

        }
    }
}
