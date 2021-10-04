using DITS.HILI.WMS.PickingModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.PickingConfiguration
{
    public class PickingPrefixConfiguration : EntityTypeConfiguration<PickingPrefix>
    {
        public PickingPrefixConfiguration()
            : this("dbo")
        {
        }

        public PickingPrefixConfiguration(string schema)
        {
            ToTable(schema + ".pk_picking_prefix");

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
