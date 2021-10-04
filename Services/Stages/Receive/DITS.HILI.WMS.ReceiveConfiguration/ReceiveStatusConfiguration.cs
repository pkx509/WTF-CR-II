using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ReceiveConfiguration
{
    public class ReceiveStatusConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ReceiveStatus>
    {
        public ReceiveStatusConfiguration()
            : this("dbo")
        {
        }

        public ReceiveStatusConfiguration(string schema)
        {
            ToTable(schema + ".rcv_receive_status");
            HasKey(x => x.ID);

            Property(x => x.ID).IsRequired().HasColumnName("ID").HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 
            Property(x => x.StatusID).IsRequired().HasColumnName("StatusID").HasColumnType("int") ;
            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.GroupStatus).IsRequired().HasColumnName("GroupStatus").HasColumnType("int");
            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
             
        }
    }
}