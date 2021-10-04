using DITS.HILI.WMS.ReceiveModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.ReceiveConfiguration
{
    public class DocumentNoPrefixConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DocumentNoPrefix>
    {
        public DocumentNoPrefixConfiguration()
            : this("dbo")
        {
        }

        public DocumentNoPrefixConfiguration(string schema)
        {
            ToTable(schema + ".reg_documentno_prefix");

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
