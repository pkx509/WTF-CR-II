using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayConfirm : BaseEntity
    {
        public Guid PutAwayConfirmID { get; set; }
        public Guid PutAwayDetailID { get; set; }
        public decimal Quantity { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid BaseUnitID { get; set; }
        public Guid ConfirmLocationID { get; set; }


        public PutAwayConfirm()
        {

        }
    }
}
