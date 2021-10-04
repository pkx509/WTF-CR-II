using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DITS.HILI.WMS.PickingModel;

namespace DITS.HILI.WMS.PickingConfiguration
{ 
    public class PickingProductAssignConfiguration : EntityTypeConfiguration<PickingProductAssign>
    {
        public PickingProductAssignConfiguration() : this("dbo")
        {

        }

        public PickingProductAssignConfiguration(string schema)
        {
            ToTable(schema + ".pk_picking_product_assign");
            HasKey(t => t.ProductAssignID);
            Property(t => t.PickingID); 
            Property(t => t.ProductID);
            Property(t => t.StockQuantity);
            Property(t => t.StockUnitID);
            Property(t => t.BaseQuantity);
            Property(t => t.BaseUnitID);
        }
    }
}
