using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayDetail : BaseEntity
    {

        public Guid PutAwayDetailID { get; set; }
        public Guid PutAwayID { get; set; }
        public Guid StockUnitID { get; set; }
        public decimal Quantity { get; set; }
        public Guid BaseUnitID { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public decimal ConfirmQty { get; set; }

        public virtual PutAway PutAway { get; set; }

        public virtual ICollection<PutAwayConfirm> PutAwayConfirmCollection { get; set; }



        public PutAwayDetail()
        {
            PutAwayConfirmCollection = new List<PutAwayConfirm>(); ;
        }
    }
}
