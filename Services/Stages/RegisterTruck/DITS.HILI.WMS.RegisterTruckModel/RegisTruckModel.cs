using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.RegisterTruckModel
{
    public class RegisTruckModel : BaseEntity
    {
        //-------RegisterTruck--------//
        public virtual IEnumerable<RegisterTruckDetailModel> RegisTruckDetail { get; set; }

        public ICollection<RegisterTruckDetail> RegisterTruckDetail { get; set; }

        public System.Guid ShippingDetailID { get; set; }
        public System.Guid ShippingID { get; set; }
        public string ShippingCode { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int RegisterTypeID { get; set; }
        public string RegisterType { get; set; }
        public string TruckType { get; set; }
        public Nullable<System.Guid> DockTypeID { get; set; }
        public string DockTypeName { get; set; }
        public System.Guid TruckTypeID { get; set; }
        public string TruckTypeName { get; set; }
        public Nullable<System.Guid> WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string ShippingTruckNo { get; set; }
        public string DriverName { get; set; }
        public string LogisticCompany { get; set; }
        public string OrderNo { get; set; }
        public string Container_No { get; set; }
        public string SealNo { get; set; }
        public string BookingNo { get; set; }
        public string PoNo { get; set; }
        public Nullable<int> ShippingStatus { get; set; }
        public System.Guid ShiptoID { get; set; }
        public string ShipptoCode { get; set; }
        public string ShipptoName { get; set; }
        public string DocumentNo { get; set; }
        public string DispatchCode { get; set; }
        public System.Guid? DispatchId { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }


        //public RegisTruckModel()
        //{
        //    RegisTruckDetail = new List<RegisterTruckDetailModel>();
        //}
    }

    public class RegisterTruckDetailModel : BaseEntity
    {

        //-------RegisterTruckDetail--------//
        public System.Guid? ShippingDetailID { get; set; }
        public System.Guid? ShippingID { get; set; }
        public System.Guid? ProductID { get; set; }
        public Nullable<decimal> ShippingQuantity { get; set; }
        public Nullable<System.Guid> ShippingUnitID { get; set; }
        public Nullable<decimal> BasicQuantity { get; set; }
        public Nullable<System.Guid> BasicUnitID { get; set; }
        public Nullable<decimal> ConversionQty { get; set; }
        public Nullable<System.Guid> ReferenceID { get; set; }
        public System.Guid? BookingID { get; set; }
        public Nullable<int> TransactionTypeID { get; set; }
        public Nullable<System.DateTime> Shipping_DT { get; set; }
        public Nullable<decimal> ConfirmQuantity { get; set; }
        public Nullable<System.Guid> ConfirmUnitID { get; set; }
        public Nullable<decimal> ConfirmBasicQuantity { get; set; }
        public Nullable<System.Guid> ConfirmBasicUnitID { get; set; }
        public System.Guid? DispatchDetailId { get; set; }
        public string ProductCode { get; set; }
        public decimal? DispatchDetail_Product_Quantity { get; set; }
        public decimal? DispatchDetail_Product_Rquantity { get; set; }
        public decimal? Order_Qty { get; set; }
        public decimal? Remain_Qty { get; set; }
        public Guid? DeliveryUnit { get; set; }

        public Guid? ProductUnitID { get; set; }
        public string ProductUnitName { get; set; }
        public string ProductName { get; set; }
        public string DispatchCode { get; set; }
        public System.Guid DispatchId { get; set; } // DispatchID (Primary key)
        public decimal? BookingQty { get; set; } // BookingQty
        public System.Guid? BookingStockUnitId { get; set; } // BookingStockUnitID
        public System.Guid? BookingBaseUnitId { get; set; } // BookingBaseUnitID
        public decimal? BookingBaseQty { get; set; } // BookingBaseQty

        public System.Guid ShiptoID { get; set; }
        public virtual RegisterTruck RegisterTruck { get; set; }

        public Nullable<int> ShippingStatus { get; set; }

    }

    public class RegisterTruckConsolidateModel
    {
        //-------RegisterTruckConsolidate--------//
        public System.Guid DeliveryID { get; set; }
        public Nullable<decimal> BasickQuantity { get; set; }
        public Nullable<System.Guid> BasicUnit { get; set; }
        public string Barcode { get; set; }
        public Nullable<System.Guid> StockUnitID { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
    }

    public class DispatchAllModel
    {
        //-------RegisterTruck--------//
        public string ShippingCode { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int RegisterTypeID { get; set; }
        public string RegisterType { get; set; }
        public string TruckTypeName { get; set; }
        public Nullable<System.Guid> DockTypeID { get; set; }
        public string DockTypeName { get; set; }
        public string ShippingTruckNo { get; set; }
        public string DriverName { get; set; }
        public string LogisticCompany { get; set; }
        public string OrderNo { get; set; }
        public string Container_No { get; set; }
        public string SealNo { get; set; }
        public string BookingNo { get; set; }
        public string PoNo { get; set; }
        public Nullable<int> ShippingStatus { get; set; }
        //public string Dispatchcode { get; set; }
        public string ShipptoCode { get; set; }
        public string DocumentNo { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        //-------RegisterTruckDetail--------//
        public System.Guid ShippingDetailID { get; set; }
        public System.Guid ShippingID { get; set; }
        public System.Guid? ProductID { get; set; }
        public Nullable<decimal> ShippingQuantity { get; set; }
        public Nullable<System.Guid> ShippingUnitID { get; set; }
        public Nullable<decimal> BasicQuantity { get; set; }
        public Nullable<System.Guid> BasicUnitID { get; set; }
        public Nullable<decimal> ConversionQty { get; set; }
        public Nullable<System.Guid> ReferenceID { get; set; }
        public Nullable<int> TransactionTypeID { get; set; }
        public Nullable<System.DateTime> Shipping_DT { get; set; }
        public Nullable<decimal> ConfirmQuantity { get; set; }
        public Nullable<System.Guid> ConfirmUnitID { get; set; }
        public Nullable<decimal> ConfirmBasicQuantity { get; set; }
        public Nullable<System.Guid> ConfirmBasicUnitID { get; set; }
        //-------Dispatch--------//
        public string DispatchCode { get; set; }
        public System.DateTime? OrderDate { get; set; }
        public string ShipptoName { get; set; }
        public Guid DocumentTypeID { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string Remark { get; set; }
        public System.DateTime? DeliveryDate { get; set; }
        //-------RegisterTruckConsolidate--------//
        public System.Guid DeliveryID { get; set; }
        public Nullable<decimal> BasickQuantity { get; set; }
        public Nullable<System.Guid> BasicUnit { get; set; }
        public string Barcode { get; set; }
        public Nullable<System.Guid> StockUnitID { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsApprove { get; set; }

        public string CustomerName { get; set; }
        public Guid DockConfigID { get; set; }
        public string DockName { get; set; }

        public System.Guid ShipToId { get; set; }
        public System.Guid LocationId { get; set; } // LocationID
        public System.Guid BookingId { get; set; } // BookingID (Primary key)
        public Guid PickingID { get; set; }
        public Guid ZoneID { get; set; }
        public Guid LocationID { get; set; }
        public Guid WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string ProductLot { get; set; } // ProductLot (length: 50)
        public string PalletCode { get; set; } // ProductLot (length: 50)
        public Nullable<decimal> BookingBaseQty { get; set; }
        public Nullable<decimal> BookingQty { get; set; }
        public Guid? RemainStockUnitID { get; set; }
        public Guid? RemainBaseUnitID { get; set; }
        public decimal? RemainQTY { get; set; }
        public decimal? RemainBaseQTY { get; set; }

        public virtual IEnumerable<RegisterTruckDetailModel> DispatchDetails { get; set; }
    }

}
