using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.PickingModel
{
    public class PickingCustomModel
    {
    }

    public class AssignJobModel
    {
        public Guid? PickingID { get; set; }
        public string PickingCode { get; set; }
        public string PONo { get; set; }
        public string OrderNo { get; set; }
        public string DocNo { get; set; }
        public string DispatchCode { get; set; }
        public DateTime? PickingDate { get; set; }
        public string PickingStatus { get; set; }
        public PickingStatusEnum PickingStatusEnums { get; set; }
        public string ShippingCode { get; set; }
        public string ShippingTruckNo { get; set; }
        public Guid? EmployeeAssignID { get; set; }
        public string EmployeeAssign { get; set; }
        public string ShipTo { get; set; }
        public string Remark { get; set; }

        public IEnumerable<AssignJobDetailModel> AssignJobDetailCollection { get; set; }
    }

    public class AssignJobDetailModel
    {
        public Guid? AssignID { get; set; }
        public Guid? BookingID { get; set; }
        public int? OrderPick { get; set; }
        public Guid? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid? SGLocationID { get; set; }
        public Guid? OldSGLocationID { get; set; }
        public string SGLocation { get; set; }
        public Guid? PickLocationID { get; set; }
        public string PickLocation { get; set; }
        public decimal? OrderQTY { get; set; }
        public decimal? OrderBaseQTY { get; set; }
        public Guid? OrderUnitID { get; set; }
        public string OrderUnit { get; set; }
        public Guid? OrderBaseUnitID { get; set; }
        public decimal? PickQTY { get; set; }
        public Guid? PickUnitID { get; set; }
        public string PickUnit { get; set; }
        public string OldPalletNo { get; set; }
        public string PalletNo { get; set; }
        public string PickingLot { get; set; }
        public decimal? PalletQTY { get; set; }
        public Guid? PalletUnitID { get; set; }
        public string PalletUnit { get; set; }
        public string Dock { get; set; }
    }

    public class DispatchforAssignJobModel
    {
        public string SearchPO { get; set; }
        public Guid? DispatchID { get; set; }
        public string PONo { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ShiptoName { get; set; }
    }

    public class PickingListHHModel
    {
        public Guid PickingID { get; set; }
        public string PickingCode { get; set; }
        public string PONo { get; set; }
        public string ShippingCode { get; set; }
        public string SGLocation { get; set; }
        public string PalletNo { get; set; }
        //public IEnumerable<string> RefPalletNo { get; set; }
        public List<KeyValuePair<string, decimal?>> RefPalletNo { get; set; }
        public Guid? ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? OrderQTY { get; set; }
        public decimal? PickQTY { get; set; }
        public decimal? PalletQTY { get; set; }
        public decimal? ConfrimQTY { get; set; }
        public decimal? ConsolidateQTY { get; set; }
        public Guid? UnitID { get; set; }
        public string Unit { get; set; }
        public string PickingStatus { get; set; }
        public bool IsRegisTruck { get; set; }
        public bool IsShowReason { get; set; }
        public bool IsReprocess { get; set; }

        public  int OrderPick { get; set; }

        public PickingListHHModel()
        {
            IsShowReason = IsRegisTruck = IsReprocess = false;
        }
    }

    public class PalletInfo
    {
        public string PalletCode { get; set; }
        public string OldPalletCode { get; set; }
        public Guid OldProductID { get; set; }
        public decimal OrderQTY { get; set; }
    }
}
