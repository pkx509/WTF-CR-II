using DITS.HILI.WMS.InventoryToolsModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class GoodsReturnPrefixConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<GoodsReturnPrefix>
    {
        public GoodsReturnPrefixConfiguration()
            : this("dbo")
        {
        }

        public GoodsReturnPrefixConfiguration(string schema)
        {
            ToTable(schema + ".qa_goodsreturn_prefix");

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
