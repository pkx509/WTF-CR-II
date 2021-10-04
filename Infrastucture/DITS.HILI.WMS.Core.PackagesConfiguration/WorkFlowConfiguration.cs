using DITS.HILI.WMS.Core.PackagesModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.Core.PackagesConfiguration
{
    public class WorkFlowConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WorkFlow>
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

            Property(x => x.DocumentTypeID).IsRequired().HasColumnName("DocumentTypeID").HasColumnType("uniqueidentifier");
            Property(x => x.PackagePrevID).IsRequired().HasColumnName("PrevID").HasColumnType("uniqueidentifier");
            Property(x => x.PackageNextID).IsRequired().HasColumnName("NextID").HasColumnType("uniqueidentifier");
            Property(x => x.Sequence).HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Start).HasColumnName("Start").HasColumnType("bit");
            Property(x => x.IsStockReserve).HasColumnName("IsStockReserve").HasColumnType("bit");

            HasRequired(x => x.PackagePrev).WithMany(x => x.WorkFlowSourceCollection).HasForeignKey(x => x.PackagePrevID).WillCascadeOnDelete(false);
            HasRequired(x => x.PackageNext).WithMany(x => x.WorkFlowDestinationCollection).HasForeignKey(x => x.PackageNextID).WillCascadeOnDelete(false);

        }
    }
}
