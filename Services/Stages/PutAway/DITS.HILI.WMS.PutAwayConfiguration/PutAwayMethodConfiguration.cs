using DITS.HILI.WMS.PutAwayModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    class PutAwayMethodConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayMethodConfig>
    {
        public PutAwayMethodConfiguration()
            : this("dbo")
        {
        }

        public PutAwayMethodConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_method_config");
            HasKey(x => x.MethodID);

            Property(x => x.MethodID).IsRequired().HasColumnName("MethodID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Sequence).IsRequired().HasColumnName("Sequence").HasColumnType("int");
            Property(x => x.Method).IsRequired().HasColumnName("Method").HasColumnType("nvarchar").HasMaxLength(100);

            Property(x => x.Remark).HasColumnName("Remark").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsActive).IsRequired().HasColumnName("IsActive").HasColumnType("bit");
            Property(x => x.UserCreated).IsRequired().HasColumnName("UserCreated").HasColumnType("uniqueidentifier");
            Property(x => x.DateCreated).IsRequired().HasColumnName("DateCreated").HasColumnType("datetime");
            Property(x => x.UserModified).IsRequired().HasColumnName("UserModified").HasColumnType("uniqueidentifier");
            Property(x => x.DateModified).IsRequired().HasColumnName("DateModified").HasColumnType("datetime");
        }
    }
}
