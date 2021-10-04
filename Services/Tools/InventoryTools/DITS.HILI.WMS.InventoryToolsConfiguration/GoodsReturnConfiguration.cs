using DITS.HILI.WMS.InventoryToolsModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DITS.HILI.WMS.InventoryToolsConfiguration
{
    public class GoodsReturnConfiguration : EntityTypeConfiguration<GoodsReturn>
    {
        public GoodsReturnConfiguration() : this("dbo")
        {

        }
        public GoodsReturnConfiguration(string schema)
        {
            // Primary Key
            HasKey(t => t.GoodsReturnID);

            // Table & Column Mappings
            ToTable(schema + ".qa_goodsreturn");
            Property(t => t.GoodsReturnID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.GoodsReturnCode).HasMaxLength(20);
            Property(t => t.ReceiveID);
            Property(t => t.GoodsReturnStatus);
            Property(t => t.ApproveDate);
            Property(t => t.Description).HasMaxLength(100);
        }
    }
}
