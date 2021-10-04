using DITS.HILI.WMS.ProductionControlModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.ProductionControlConfiguration
{
    public class PalletPrefixConfiguration : EntityTypeConfiguration<PalletPrefix>
    {
        public PalletPrefixConfiguration() : this("dbo")
        {

        }

        public PalletPrefixConfiguration(string schema)
        {
            ToTable(schema + ".pc_pallet_prefix");
            HasKey(x => x.PrefixID);

            Property(x => x.PrefixID).IsRequired().HasColumnName("PrefixID").HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PrefixKey).HasColumnName("PrefixKey").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.FormatKey).HasColumnName("FormatKey").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.LengthKey).HasColumnName("LengthKey").HasColumnType("int");
            Property(x => x.LastedKey).HasColumnName("LastedKey").HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.IsLastest);
        }
    }
}
