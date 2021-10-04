using DITS.HILI.WMS.MasterModel.Rule;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class SpecialBookingRuleConfiguration : EntityTypeConfiguration<SpecialBookingRule>
    {
        public SpecialBookingRuleConfiguration()
            : this("dbo")
        {
        }

        public SpecialBookingRuleConfiguration(string schema)
        {
            ToTable("sys_special_booking_rule", schema);
            HasKey(x => x.RuleId);

            Property(x => x.RuleId).HasColumnName(@"RuleID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.RuleName).HasColumnName(@"Rule_name").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.ProductId).HasColumnName(@"ProductID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.Aging).HasColumnName(@"Aging").HasColumnType("int").IsRequired();
            Property(x => x.UnitAging).HasColumnName(@"UnitAging").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(10);
            Property(x => x.DurationNotOver).HasColumnName(@"DurationNotOver").HasColumnType("int").IsOptional();
            Property(x => x.UnitDuration).HasColumnName(@"UnitDuration").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(10);
            Property(x => x.LotNo).HasColumnName(@"Lot_No").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(20);
            Property(x => x.NoMoreThanDo).HasColumnName(@"NoMoreThanDO").HasColumnType("int").IsOptional();
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsOptional();




        }
    }
}
