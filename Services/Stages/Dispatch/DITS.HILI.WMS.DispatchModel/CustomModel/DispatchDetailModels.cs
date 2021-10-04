using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class DispatchDetailModels
    {

        public System.Guid? DispatchDetailId { get; set; }
        public System.Guid? DispatchId { get; set; }
        public int? Sequence { get; set; }
        public System.Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductLot { get; set; }
        public System.Guid StockUnitId { get; set; }
        public string StockUnitName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? BaseQuantity { get; set; }
        public System.Guid? BaseUnitId { get; set; }
        public string BaseUnitName { get; set; }
        public decimal? ConversionQty { get; set; }
        public System.Guid? ProductOwnerId { get; set; }
        public string ProductOwnerName { get; set; }
        public decimal? DispatchDetailProductWidth { get; set; }
        public decimal? DispatchDetailProductLength { get; set; }
        public decimal? DispatchDetailProductHeight { get; set; }
        public System.Guid? DispatchPriceUnitId { get; set; }
        public string DispatchPriceUnitName { get; set; }
        public decimal? DispatchPrice { get; set; }
        public int? DispatchDetailStatusId { get; set; }
        public string DispatchDetailStatusName { get; set; }
        public System.Guid? RuleId { get; set; }
        public string RuleName { get; set; }
        public string Remark { get; set; }
        public bool? IsActive { get; set; }
        public System.Guid? ProductStatusId { get; set; }
        public string ProductStatusName { get; set; }
        public System.Guid? ProductSubStatusId { get; set; }
        public string ProductSubStatusName { get; set; }
        public string LocationCode { get; set; }
        public string PalletCode { get; set; }
        public bool? IsBackOrder { get; set; }


        public System.Guid? PickLocationId { get; set; }
        public string PickLocationCode { get; set; }
        public decimal? PickQTY { get; set; }
        public System.Guid? PickQTYUnitId { get; set; }
        public string PickQTYUnitName { get; set; }
        public decimal? PickBaseQTY { get; set; }
        public string PickPalletCode { get; set; }
        public string PickProductLot { get; set; }

        public decimal? ConsolidateQTY { get; set; }
        public System.Guid? ConsolidateQTYUnitId { get; set; }
        public string ConsolidateQTYUnitName { get; set; }
        public decimal? ConsolidateBaseQTY { get; set; }

        public System.Guid? DeliveryId { get; set; }
        public System.Guid? BookingId { get; set; }
        public string OrderNo { get; set; }


    }

    public class DispatchDetailCustom
    {
        public Guid DispatchDetailID { get; set; }
        public Guid DispatchID { get; set; }
        public Guid? RuleID { get; set; }
        public int DispatchDetailStatus { get; set; }
        public int BookingStatus { get; set; }
        public decimal? DispatchDetailProductWidth { get; set; }
        public decimal? DispatchDetailProductLength { get; set; }
        public decimal? DispatchDetailProductHeight { get; set; }
        public decimal? DispatchPrice { get; set; }

        public bool? IsBackOrder { get; set; }

        public int Sequence { get; set; }
        public Guid ProductId { get; set; }
        public Guid StockUnitId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? BaseQuantity { get; set; }
        public Guid? BaseUnitId { get; set; }
        public decimal? ConversionQty { get; set; }
        public Guid ProductOwnerId { get; set; }
        public Guid? ProductStatusId { get; set; }
        public Guid? ProductSubStatusId { get; set; }

        public Guid LocationId { get; set; }
        public string ProductLot { get; set; }
        public DateTime Mfgdate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public bool IsActive { get; set; }
        public Guid UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserModified { get; set; }
        public DateTime DateModified { get; set; }
        public string Remark { get; set; }
        public string PalletCode { get; set; }

        public List<DispatchDetailPalletCustom> PalletCodes { get; set; }
    }

    public class DispatchDetailPalletCustom
    {
        public string PalletCode { get; set; }
        public DateTime Mfgdate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal RequestQty { get; set; }
        public Guid RequestStockUnitId { get; set; }
        public decimal RequestBaseQty { get; set; }
        public Guid RequestBaseUnitId { get; set; }
    }
}
