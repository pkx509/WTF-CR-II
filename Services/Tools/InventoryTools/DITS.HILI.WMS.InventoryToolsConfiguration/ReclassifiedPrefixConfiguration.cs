using DITS.HILI.WMS.InventoryToolsModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class ReclassifiedPrefixConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ReclassifiedPrefix>
    {
        public ReclassifiedPrefixConfiguration()
            : this("dbo")
        {
        }

        public ReclassifiedPrefixConfiguration(string schema)
        {
            ToTable(schema + ".qa_reclassified_prefix");

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
