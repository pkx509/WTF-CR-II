using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisterTruck : BaseEntity
    {
        public RegisterTruck()
        {

            RegisterTruckDetail = new List<RegisterTruckDetail>();
        }

        public System.Guid ShippingID { get; set; }
        public string ShippingCode { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int RegisterTypeID { get; set; }
        public string TruckType { get; set; }
        public Nullable<System.Guid> DockTypeID { get; set; }
        public System.Guid TruckTypeID { get; set; }
        public Nullable<System.Guid> WarehouseID { get; set; }
        public string ShippingTruckNo { get; set; }
        public string DriverName { get; set; }
        public string LogisticCompany { get; set; }
        public string OrderNo { get; set; }
        public string Container_No { get; set; }
        public Nullable<int> ShippingStatus { get; set; }
        public string SealNo { get; set; }
        public string BookingNo { get; set; }
        public string PoNo { get; set; }
        public string Dispatchcode { get; set; }
        public System.Guid ShiptoID { get; set; }
        public string ShipptoCode { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public virtual ICollection<RegisterTruckDetail> RegisterTruckDetail { get; set; }
    }
}
