using DITS.HILI.WMS.DailyPlanModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace DITS.HILI.WMS.DailyPlanConfiguration
{
    public class ProductionPlanDetailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductionPlanDetail>
    {
        public ProductionPlanDetailConfiguration() : this("dbo")
        { }
        public ProductionPlanDetailConfiguration(string schema)
        { // Primary Key
            HasKey(t => t.ProductionDetailID);

            // Properties
            Property(t => t.Film)
                .HasMaxLength(100);

            Property(t => t.Box)
                .HasMaxLength(100);

            Property(t => t.Powder)
                .HasMaxLength(100);

            Property(t => t.Oil)
                .HasMaxLength(100);

            Property(t => t.FD)
                .HasMaxLength(50);

            Property(t => t.Stamp)
                .HasMaxLength(100);

            Property(t => t.Sticker)
                .HasMaxLength(20);

            Property(t => t.Mark)
                .HasMaxLength(20);

            Property(t => t.WorkingTime)
                .HasMaxLength(100);

            Property(t => t.OilType)
                .HasMaxLength(50);

            Property(t => t.Formula)
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable(schema + ".pp_production_plan_detail");
            Property(t => t.ProductionDetailID).HasColumnName("ProductionDetailID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ProductionID).HasColumnName("ProductionID");
            Property(t => t.Seq).HasColumnName("Seq");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.ProductionQty).HasColumnName("ProductionQty");
            Property(t => t.ProductUnitID).HasColumnName("ProductUnitID");
            Property(t => t.Weight_G).HasColumnName("Weight_G");
            Property(t => t.Film).HasColumnName("Film");
            Property(t => t.Box).HasColumnName("Box");
            Property(t => t.Powder).HasColumnName("Powder");
            Property(t => t.Oil).HasColumnName("Oil");
            Property(t => t.FD).HasColumnName("FD");
            Property(t => t.Stamp).HasColumnName("Stamp");
            Property(t => t.Sticker).HasColumnName("Sticker");
            Property(t => t.Mark).HasColumnName("Mark");
            Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");
            Property(t => t.WorkingTime).HasColumnName("WorkingTime");
            Property(t => t.OilType).HasColumnName("OilType");
            Property(t => t.Formula).HasColumnName("Formula");
            Property(t => t.PalletQty).HasColumnName("PalletQty");
            Property(t => t.Remark).HasColumnName("Remark");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.UserCreated).HasColumnName("UserCreated");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.UserModified).HasColumnName("UserModified");
            Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            HasRequired(t => t.ProductionPlan)
                .WithMany(t => t.ProductionPlanDetail)
                .HasForeignKey(d => d.ProductionID);

        }
    }
}
