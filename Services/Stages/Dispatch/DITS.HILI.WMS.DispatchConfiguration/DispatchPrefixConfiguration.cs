using DITS.HILI.WMS.DispatchModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.DispatchConfiguration
{
    public class DispatchPrefixConfiguration : EntityTypeConfiguration<DispatchPrefix>
    {
        public DispatchPrefixConfiguration()
            : this("dbo")
        {
        }

        public DispatchPrefixConfiguration(string schema)
        {
            ToTable("dp_dispatch_prefix", schema);
            HasKey(x => x.PrefixId);

            Property(x => x.PrefixId).HasColumnName(@"PrefixID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PrefixKey).HasColumnName(@"PrefixKey").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.FormatKey).HasColumnName(@"FormatKey").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.LengthKey).HasColumnName(@"LengthKey").HasColumnType("int").IsOptional();
            Property(x => x.LastedKey).HasColumnName(@"LastedKey").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.PrefixType).HasColumnName(@"PrefixType").HasColumnType("int");

        }
    }
}
