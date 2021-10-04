using DITS.HILI.WMS.InventoryToolsModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class GoodsReturnDetailConfiguration : EntityTypeConfiguration<GoodsReturnDetail>
    {
        public GoodsReturnDetailConfiguration() : this("dbo")
        {

        }
        public GoodsReturnDetailConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.GoodsReturnDetailID);

            // Table & Column Mappings
            ToTable(schema + ".qa_goodsReturn_detail");
            Property(t => t.GoodsReturnDetailID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.GoodsReturnID);
            Property(t => t.ProductID);
            Property(t => t.ReceiveQTY);
            Property(t => t.ReceiveBaseQTY);
            Property(t => t.ConversionQTY);
            Property(t => t.ReceiveUnitID);
            Property(t => t.ReceiveBaseUnitID);
            Property(t => t.MFGDate);
            Property(t => t.LineID);
            Property(t => t.ReceiveLot).HasMaxLength(20);
        }
    }
}
