using DITS.HILI.WMS.InventoryToolsModel;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{

    public class CycleCountAssignConfiguration : EntityTypeConfiguration<CycleCountAssign>
    {
        public CycleCountAssignConfiguration() : this("dbo")
        {

        }
        public CycleCountAssignConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.CyclecountAssignID);

            // Properties
            Property(t => t.PalletCode)
                .HasMaxLength(40);


            // Table & Column Mappings
            ToTable(schema + ".cc_cyclecount_assign");
            Property(t => t.CyclecountAssignID).HasColumnName("CyclecountAssignID");
            Property(t => t.CyclecountDetailID).HasColumnName("CyclecountDetailID");
            Property(t => t.BasickQuantity).HasColumnName("BasickQuantity");
            Property(t => t.BasicUnit).HasColumnName("BasicUnit");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.PalletCode).HasColumnName("PalletCode");

            // Relationships
            HasOptional(t => t.CycCountDetail)
                .WithMany(t => t.CycCountAssign)
                .HasForeignKey(d => d.CyclecountDetailID);
        }
    }
}
