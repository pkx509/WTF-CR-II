using System;

namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class DispatchOtherModel
    {

        public class DPLocationBalance
        {
            public System.Guid LocationId { get; set; }
            public decimal StockQuantity { get; set; }
        }

        public class DPBalanceInfo
        {
            public System.Guid StockInfoId { get; set; }
            public decimal StockQuantity { get; set; }
        }

        public class DPStockBalance
        {
            public System.Guid StockInfoId { get; set; }
            public decimal StockQuantity { get; set; }
            public decimal BaseQuantity { get; set; }
            public decimal ReserveQuantity { get; set; }
            public Guid BookingId { get; set; }
            public decimal BookStockQuantity { get; set; }
            public decimal BookBaseQuantity { get; set; }
            public decimal BookReserveQuantity { get; set; }
            public string PalletCode { get; set; }
        }
        public class DPStockBalanceLocation
        {
            public System.Guid StockLocationId { get; set; }
            public decimal ReserveQuantity { get; set; }
            public decimal BaseQuantity { get; set; }
            public decimal StockQuantity { get; set; }
            public Guid BookingId { get; set; }
            public decimal BookReserveQuantity { get; set; }
            public decimal BookBaseQuantity { get; set; }
            public decimal BookStockQuantity { get; set; }
            public string PalletCode { get; set; }
        }

        public class PCPackingDetail
        {
            public string PalletCode { get; set; }
            public decimal ReserveQuantity { get; set; }
            public decimal BaseQuantity { get; set; }
            public decimal StockQuantity { get; set; }
            public decimal BookReserveQuantity { get; set; }
            public decimal BookBaseQuantity { get; set; }
            public decimal BookStockQuantity { get; set; }
        }

        public class DPDetailBackOrder
        {
            public System.Guid DispatchDetailId { get; set; }
            public decimal BackOrderQuantity { get; set; }
        }

        public class DPDetailItemBackOrder
        {
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal BookingQTY { get; set; }
            public decimal RemainQTY { get; set; }
            public decimal BackOrderQTY { get; set; }
            public string UnitName { get; set; }
            public string RuleName { get; set; }
        }

        public class GroupLotModel
        {
            public string GroupLotList { get; set; }
            public int GroupIndex { get; set; }
            public decimal GroupSumQTY { get; set; }
        }

    }
}
