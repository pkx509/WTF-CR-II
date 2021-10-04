using DITS.HILI.WMS.MasterModel.Rule;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration
{
    public class SpecialPutawayRuleConfiguration : EntityTypeConfiguration<SpecialPutawayRule>
    {
        public SpecialPutawayRuleConfiguration()
            : this("dbo")
        {
        }

        public SpecialPutawayRuleConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.PutAwayRuleID);

            // Properties
            Property(t => t.Remark)
                .HasMaxLength(50);

            Property(t => t.Condition)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            // Table & Column Mappings
            ToTable("sys_special_putaway_rule");
            Property(x => x.PutAwayRuleID).IsRequired().HasColumnName("PutAwayRuleID").HasColumnType("uniqueidentifier")
                                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.LogicalZoneID).HasColumnName("LogicalZoneID");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.PeriodLot).HasColumnName("PeriodLot");
            Property(t => t.Priority).HasColumnName("Priority");
            Property(t => t.Condition).HasColumnName("Condition");
            Property(t => t.IsActive).HasColumnName("IsActive");

        }
    }
}
