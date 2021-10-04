using DITS.HILI.WMS.PutAwayModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PutAwayConfiguration
{
    public class PutAwayPrefixConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PutAwayPrefix>
    {
        public PutAwayPrefixConfiguration()
            : this("dbo")
        {
        }

        public PutAwayPrefixConfiguration(string schema)
        {
            ToTable(schema + ".pw_putaway_prefix");

            HasKey(x => x.PrefixID);

            Property(x => x.PrefixID).IsRequired().HasColumnName("PrefixID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PrefixKey).HasColumnName("PrefixKey").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.FormatKey).HasColumnName("FormatKey").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.LengthKey).HasColumnName("LengthKey").HasColumnType("int");
            Property(x => x.LastedKey).HasColumnName("LastedKey").HasColumnType("nvarchar").HasMaxLength(20);

        }
    }
}
