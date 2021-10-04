using DITS.HILI.WMS.MasterModel.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class ReasonConfiguration : EntityTypeConfiguration<Reason>
    {
        public ReasonConfiguration()
            : this("dbo")
        {

        }

        public ReasonConfiguration(string schema)
        {
            ToTable(schema + ".sys_reason");
            HasKey(x => x.ReasonID);

            Property(x => x.ReasonID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ReasonCode).HasMaxLength(3);
            Property(x => x.ReasonName).HasMaxLength(50);
            Property(x => x.IsDefault);

            Property(x => x.Remark).HasMaxLength(250);

        }
    }
}
