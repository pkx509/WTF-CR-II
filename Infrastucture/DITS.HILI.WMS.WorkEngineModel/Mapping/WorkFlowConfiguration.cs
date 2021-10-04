using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineModel.Mapping
{
    public class WorkFlowConfiguration :   System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WorkFlow>
    {
        public WorkFlowConfiguration()
            : this("dbo")
        {
        }

        public WorkFlowConfiguration(string schema)
        {
            ToTable(schema + ".core_workflow");
            HasKey(x => x.WorkFlowID);

            Property(x => x.WorkFlowID).IsRequired().HasColumnName("WorkFlowID").HasColumnType("uniqueidentifier")
                                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.DocumentTypeID).IsRequired().HasColumnName("ActivityID").HasColumnType("uniqueidentifier");
            Property(x => x.Source).IsRequired().HasColumnName("Source").HasColumnType("uniqueidentifier");
            Property(x => x.Destination).IsRequired().HasColumnName("Destination").HasColumnType("uniqueidentifier") ;
            Property(x => x.Sequence).HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Incoming).HasColumnName("Incoming").HasColumnType("int");
            Property(x => x.Outgoing).HasColumnName("Outgoing").HasColumnType("int");
            Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);

            HasRequired(x => x.ModuleSource).WithMany(x => x.WorkFlowSourceCollection).HasForeignKey(x => x.Source);
            HasRequired(x => x.ModuleDestination).WithMany(x => x.WorkFlowDestinationCollection).HasForeignKey(x => x.Destination);

        } 
    }
}
