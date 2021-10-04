using DITS.HILI.WMS.MasterModel.Warehouses;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Warehouses
{
    public class ConditionConfigConfiguration : EntityTypeConfiguration<ConditionConfig>
    {
        public ConditionConfigConfiguration()
            : this("dbo")
        {
        }

        public ConditionConfigConfiguration(string schema)
        {
            ToTable("sys_condition_config", schema);
            HasKey(x => x.ConfigId);

            Property(x => x.ConfigId).HasColumnName(@"ConfigID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ModuleName).HasColumnName(@"ModuleName").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.ConfigName).HasColumnName(@"ConfigName").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.PrioritySeq).HasColumnName(@"PrioritySeq").HasColumnType("int").IsRequired();
            Property(x => x.IsOrder).HasColumnName(@"IsOrder").HasColumnType("bit").IsRequired();
            Property(x => x.ConfigVariableId).HasColumnName(@"ConfigVariableID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.ConfigVariable).HasColumnName(@"ConfigVariable").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsComboBox).HasColumnName(@"IsComboBox").HasColumnType("bit").IsOptional();
            Property(x => x.ConfigScript).HasColumnName(@"ConfigScript").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
        }

    }
}
