using DITS.HILI.WMS.MasterModel.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class T_BalanceBFMap : EntityTypeConfiguration<T_BalanceBF>
    {
        public T_BalanceBFMap()
        {
            // Primary Key
            HasKey(t => new { t.BFBalanceDT, t.ProductID, t.BaseQuantity, t.BaseUnitID, t.StockQuantity, t.StockUnitID });

            // Properties
            Property(t => t.BaseQuantity)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.StockQuantity)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("T_BalanceBF");
            Property(t => t.BFBalanceDT).HasColumnName("BFBalanceDT");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.BaseQuantity).HasColumnName("BaseQuantity");
            Property(t => t.BaseUnitID).HasColumnName("BaseUnitID");
            Property(t => t.StockQuantity).HasColumnName("StockQuantity");
            Property(t => t.StockUnitID).HasColumnName("StockUnitID");
        }
    }
}
