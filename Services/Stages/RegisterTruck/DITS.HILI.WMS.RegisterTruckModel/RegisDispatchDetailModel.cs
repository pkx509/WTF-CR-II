using System;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisDispatchDetailModel
    {
        //-------DispatchDetail--------//
        public System.Guid DispatchDetailId { get; set; }
        public string ProductCode { get; set; }
        public decimal? DispatchDetail_Product_Quantity { get; set; }
        public decimal? DispatchDetail_Product_Rquantity { get; set; }
        public decimal? Order_Qty { get; set; }
        public decimal? Remain_Qty { get; set; }
        public Guid DeliveryUnit { get; set; }

        public Guid ProductUnitID { get; set; }
        public string ProductUnitName { get; set; }
        public string ProductName { get; set; }
    }
}
