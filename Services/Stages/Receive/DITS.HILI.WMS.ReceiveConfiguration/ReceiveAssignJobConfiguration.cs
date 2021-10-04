using DITS.HILI.WMS.ReceiveModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.ReceiveConfiguration
{
    public class ReceiveAssignJobConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ReceiveAssignJob>
    {
        public ReceiveAssignJobConfiguration()
            : this("dbo")
        {
        }

        public ReceiveAssignJobConfiguration(string schema)
        {
            ToTable(schema + ".rcv_receive_assign");
            HasKey(x => new { x.ReferenceID, x.EmployeeID });

            Property(x => x.ReferenceID).IsRequired().HasColumnName("ReceiveID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.EmployeeID).IsRequired().HasColumnName("EmployeeID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            HasRequired(x => x.Receive).WithMany(x => x.ReceiveAssignJobCollection).HasForeignKey(x => x.ReferenceID).WillCascadeOnDelete(false);

        }
    }
}
