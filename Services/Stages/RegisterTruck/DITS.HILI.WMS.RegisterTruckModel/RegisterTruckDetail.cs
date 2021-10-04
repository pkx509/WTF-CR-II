using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisterTruckDetail : BaseEntity
    {
        public RegisterTruckDetail()
        {
            RegisterTruckConsolidate = new List<RegisterTruckConsolidate>();
        }

        public System.Guid ShippingDetailID { get; set; }
        public System.Guid ShippingID { get; set; }
        public System.Guid ProductID { get; set; }
        public decimal ShippingQuantity { get; set; }
        public System.Guid ShippingUnitID { get; set; }
        public decimal BasicQuantity { get; set; }
        public System.Guid BasicUnitID { get; set; }
        public decimal ConversionQty { get; set; }
        public System.Guid ReferenceID { get; set; }
        public System.Guid BookingID { get; set; }
        public int TransactionTypeID { get; set; }
        public System.DateTime Shipping_DT { get; set; }
        public Nullable<decimal> ConfirmQuantity { get; set; }
        public Nullable<System.Guid> ConfirmUnitID { get; set; }
        public Nullable<decimal> ConfirmBasicQuantity { get; set; }
        public Nullable<System.Guid> ConfirmBasicUnitID { get; set; }
        public virtual RegisterTruck RegisterTruck { get; set; }
        public virtual ICollection<RegisterTruckConsolidate> RegisterTruckConsolidate { get; set; }
    }
}
