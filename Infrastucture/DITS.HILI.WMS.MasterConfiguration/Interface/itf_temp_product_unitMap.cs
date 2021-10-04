using DITS.HILI.WMS.MasterModel.Interface;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.MasterConfiguration.Interface
{
    public class itf_temp_product_unitMap : EntityTypeConfiguration<itf_temp_product_unit>
    {
        public itf_temp_product_unitMap()
        {
            // Primary Key
            HasKey(t => t.ProductUnitID);

            // Properties
            Property(t => t.Product_system_code)
                .HasMaxLength(15);

            Property(t => t.AlternateUnit)
                .HasMaxLength(3);

            Property(t => t.BasicUnit)
                .HasMaxLength(3);

            Property(t => t.GDATE)
                .HasMaxLength(10);

            Property(t => t.GTIME)
                .HasMaxLength(10);

            Property(t => t.GSTT)
                .HasMaxLength(1);

            Property(t => t.FDATE)
                .HasMaxLength(10);

            Property(t => t.FTIME)
                .HasMaxLength(10);

            Property(t => t.FSTT)
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("itf_temp_product_unit");
            Property(t => t.ProductUnitID).HasColumnName("ProductUnitID");
            Property(t => t.Product_system_code).HasColumnName("Product_system_code");
            Property(t => t.UnitSeq).HasColumnName("UnitSeq");
            Property(t => t.AlternateUnit).HasColumnName("AlternateUnit");
            Property(t => t.Quantity).HasColumnName("Quantity");
            Property(t => t.Conversion).HasColumnName("Conversion");
            Property(t => t.BasicUnit).HasColumnName("BasicUnit");
            Property(t => t.GDATE).HasColumnName("GDATE");
            Property(t => t.GTIME).HasColumnName("GTIME");
            Property(t => t.GSTT).HasColumnName("GSTT");
            Property(t => t.FDATE).HasColumnName("FDATE");
            Property(t => t.FTIME).HasColumnName("FTIME");
            Property(t => t.FSTT).HasColumnName("FSTT");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            Property(t => t.UpdateUserID).HasColumnName("UpdateUserID");
            Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");
            Property(t => t.ProductID).HasColumnName("ProductID");
            Property(t => t.IsBaseUOM).HasColumnName("IsBaseUOM");
            Property(t => t.StandardPallet).HasColumnName("StandardPallet");
        }
    }
}
