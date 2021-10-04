using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DITS.HILI.WMS.MasterModel;

namespace DITS.HILI.WMS.PickingModel
{
   public class PickingProductAssign : BaseEntity
    {
        public Guid ProductAssignID { get; set; }
        public Guid PickingID { get; set; }
        public Guid ProductID { get; set; }
        public decimal BaseQuantity { get; set; } 
        public Guid BaseUnitID { get; set; } 
        public Guid StockUnitID { get; set; }
        public decimal StockQuantity { get; set; }
    }
}
