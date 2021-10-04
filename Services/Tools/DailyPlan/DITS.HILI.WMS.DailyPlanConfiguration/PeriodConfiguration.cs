using DITS.HILI.WMS.DailyPlanModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.DailyPlanConfiguration
{
    public class PeriodConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Period>
    {
        public PeriodConfiguration() : this("dbo")
        { }
        public PeriodConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.PeriodID);

            // Properties
            // Table & Column Mappings
            ToTable(schema + ".sys_period");
            Property(t => t.PeriodID).HasColumnName("PeriodID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;
            Property(t => t.P_StartTime).HasColumnName("P_StartTime");
            Property(t => t.P_EndTime).HasColumnName("P_EndTime");
            Property(t => t.IsDefault).HasColumnName("IsDefault");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
        }
    }
}
