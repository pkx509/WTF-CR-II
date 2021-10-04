using DITS.HILI.WMS.PutAwayModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayReasonConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayReason>
    {
        public PutAwayReasonConfiguration()
            : this("dbo")
        {
        }

        public PutAwayReasonConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_reason");
            HasKey(x => x.PutAwayReasonID);

            Property(x => x.PutAwayReasonID).IsRequired().HasColumnName("PutAwayReasonID").HasColumnType("uniqueidentifier")
                                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Description).IsRequired().HasColumnName("Description").HasColumnType("nvarchar").HasMaxLength(250);


        }
    }
}
