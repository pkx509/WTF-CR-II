using DITS.HILI.WMS.WorkEngineModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineConfiguration
{
    public class ModuleWorkFlowConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ModuleWorkFlow>
    {
        public ModuleWorkFlowConfiguration()
            : this("dbo")
        {
        }

        public ModuleWorkFlowConfiguration(string schema)
        {
            ToTable(schema + ".core_module_workflow");
            HasKey(x => x.ModuleWorkFlowID);

            Property(x => x.ModuleWorkFlowID).IsRequired().HasColumnName("ModuleWorkFlowID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(x => x.DocumentTypeID).IsRequired().HasColumnName("ActivityID").HasColumnType("uniqueidentifier");
            Property(x => x.ModuleID).IsRequired().HasColumnName("ModuleID").HasColumnType("uniqueidentifier");
            Property(x => x.Method).IsRequired().HasColumnName("Method").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Sequence).HasColumnName("Sequence").HasColumnType("int");

            HasRequired(x => x.Module).WithMany(x => x.ModuleWorkFlowCollection).HasForeignKey(x => x.ModuleID);

        }
    }
}
