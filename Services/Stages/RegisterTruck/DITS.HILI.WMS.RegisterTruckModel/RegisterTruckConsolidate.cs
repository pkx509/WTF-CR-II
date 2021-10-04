using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisterTruckConsolidate : BaseEntity
    {
        public System.Guid DeliveryID { get; set; }
        public Nullable<System.Guid> ShippingDetailID { get; set; }
        public Nullable<decimal> BaseQuantity { get; set; }
        public Nullable<System.Guid> BaseUnitID { get; set; }
        public string Barcode { get; set; }
        public Nullable<System.Guid> StockUnitID { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public int ConsolidateStatus { get; set; }
        public string PalletCode { get; set; }
        public virtual RegisterTruckDetail RegisterTruckDetail { get; set; }
    }
}
