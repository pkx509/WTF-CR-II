using DITS.HILI.WMS.DailyPlanModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.DailyPlanConfiguration
{
    public class LineConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Line>
    {
        public LineConfiguration() : this("dbo")
        { }

        public LineConfiguration(string schema)
        {
            ToTable("pp_production_line", schema);
            HasKey(x => x.LineID);

            Property(x => x.LineID).HasColumnName(@"LineID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.LineCode).HasColumnName(@"LineCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.WarehouseID).HasColumnName(@"WarehouseID").HasColumnType("uniqueidentifier").IsOptional();
            Property(x => x.BoiCard).HasColumnName(@"BOICard").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.LineType).HasColumnName(@"LineType").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);

            Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            Property(x => x.UserCreated).HasColumnName(@"UserCreated").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.UserModified).HasColumnName(@"UserModified").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.DateModified).HasColumnName(@"DateModified").HasColumnType("datetime").IsRequired();
        }
    }
}
