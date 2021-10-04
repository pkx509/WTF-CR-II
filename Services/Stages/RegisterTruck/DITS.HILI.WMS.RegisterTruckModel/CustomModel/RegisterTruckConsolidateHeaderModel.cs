using DITS.HILI.WMS.RegisterTruckModel.CustomModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisterTruckConsolidateHeaderModel
    {


        public System.Guid ShippingID { get; set; }
        public string ShippingCode { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int RegisterTypeID { get; set; }
        public string RegisterTypeName { get; set; }
        public string TruckTypeName { get; set; }
        public Nullable<System.Guid> DockTypeID { get; set; }
        public string DockTypeName { get; set; }
        public System.Guid TruckTypeID { get; set; }
        public Nullable<System.Guid> WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string ShippingTruckNo { get; set; }
        public string DriverName { get; set; }
        public string LogisticCompany { get; set; }
        public string OrderNo { get; set; }
        public string Container_No { get; set; }
        public Nullable<int> ShippingStatus { get; set; }
        public string ShippingStatusName { get; set; }
        public string SealNo { get; set; }
        public string BookingNo { get; set; }
        public string PoNo { get; set; }
        public string Dispatchcode { get; set; }
        public System.Guid ShiptoID { get; set; }
        public string ShiptoName { get; set; }
        public string ShipptoCode { get; set; }
        public string DocumentNo { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public virtual ICollection<RegisterTruckConsolidateDeatilModel> RegisterTruckConsolidateDeatilModels { get; set; }

        public RegisterTruckConsolidateHeaderModel()
        {

            RegisterTruckConsolidateDeatilModels = new List<RegisterTruckConsolidateDeatilModel>();
        }
    }
}
