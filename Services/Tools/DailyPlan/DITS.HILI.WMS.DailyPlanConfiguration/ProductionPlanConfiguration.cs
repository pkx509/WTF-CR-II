using DITS.HILI.WMS.DailyPlanModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace DITS.HILI.WMS.DailyPlanConfiguration
{
    public class ProductionConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductionPlan>
    {
        public ProductionConfiguration() : this("dbo")
        { }

        public ProductionConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.ProductionID);

            // Properties
            Property(t => t.OrderNo)
                .HasMaxLength(50);

            Property(t => t.OrderType)
                .HasMaxLength(20);

            Property(t => t.Remark)
                .HasMaxLength(250);

            // Table & Column Mappings
            ToTable(schema + ".pp_production_plan");
            Property(t => t.ProductionID).HasColumnName("ProductionID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ProductionDate).HasColumnName("ProductionDate");
            Property(t => t.PeriodID).HasColumnName("PeriodID");
            Property(t => t.OrderNo).HasColumnName("OrderNo");
            Property(t => t.OrderType).HasColumnName("OrderType");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");
            Property(t => t.LineID).HasColumnName("LineID");
            Property(t => t.DailyPlanStatus).HasColumnName("DailyPlanStatus");

            // Relationships
            HasOptional(t => t.Line)
                .WithMany(t => t.ProductionCollection)
                .HasForeignKey(d => d.LineID);

        }
    }
}
