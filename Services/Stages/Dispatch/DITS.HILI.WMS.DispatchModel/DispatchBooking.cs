using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.DispatchModel
{
    public class DispatchBooking : BaseEntity
    {
        public System.Guid BookingId { get; set; } // BookingID (Primary key)
        public System.Guid DispatchDetailId { get; set; } // DispatchDetailID
        public int Sequence { get; set; } // Sequence
        public System.Guid ProductId { get; set; } // ProductID
        public string ProductLot { get; set; } // ProductLot (length: 50)
        public decimal RequestQty { get; set; } // RequestQty
        public System.Guid? RequestStockUnitId { get; set; } // RequestStockUnitID
        public System.Guid? RequestBaseUnitId { get; set; } // RequestBaseUnitID
        public decimal? RequestBaseQty { get; set; } // RequestBaseQty
        public decimal BookingQty { get; set; } // BookingQty
        public System.Guid? BookingStockUnitId { get; set; } // BookingStockUnitID
        public System.Guid? BookingBaseUnitId { get; set; } // BookingBaseUnitID
        public decimal? BookingBaseQty { get; set; } // BookingBaseQty
        public decimal? ConversionQty { get; set; } // ConversionQty
        public bool? IsBackOrder { get; set; } // IsBackOrder
        public BookingStatusEnum BookingStatus { get; set; } // BookingStatus
        public System.Guid LocationId { get; set; } // LocationID
        public System.DateTime Mfgdate { get; set; } // MFGDATE
        public System.DateTime? ExpirationDate { get; set; } // ExpirationDate
        public string PalletCode { get; set; } // PalletCode


        // Foreign keys

        public virtual DispatchDetail DispatchDetails { get; set; } // FK_dispatchDetailID

        public DispatchBooking()
        {
            RequestQty = 0;
            BookingQty = 0;
            IsActive = true;
        }

    }

    public class PcPackingDetail
    {
        public System.Guid PackingId { get; set; }
        public decimal ReserveQTY { get; set; }
        public decimal ReserveBaseQTY { get; set; }
    }

    public class BookingDispatchModel
    {
        public System.Guid ProductId { get; set; }
        public System.Guid DispatchDetailId { get; set; }
        public System.Guid RuleId { get; set; }
        public System.Guid StockInfoId { get; set; }
        public System.Guid StockUnitId { get; set; }
        public System.Guid LocationId { get; set; }
        public string LocationCode { get; set; }
        public decimal StockQuantity { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public System.Guid BaseUnitId { get; set; }
        public decimal BaseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public string Lot { get; set; }
        public System.Guid PackingId { get; set; }
        public int GroupLot { get; set; }
        public int DayDiff { get; set; }
        public string OrderNo { get; set; }
        public string PalletCode { get; set; }
        public int WarehouseSeqno { get; set; }

    }

    public class BookingDispatchCustom
    {
        public Guid? RuleId { get; set; }
        public Guid ProductId { get; set; }
        public Guid StockUnitId { get; set; }
        public Guid? BaseUnitId { get; set; }
        public Guid? ProductStatusId { get; set; }
        public Guid DispatchDetailId { get; set; }
    }

    public class InterfaceDispatchModel
    {
        public System.Guid ProductId { get; set; }
        public string DispatchCode { get; set; }
        public string PONo { get; set; }
        public string Lot { get; set; }
        public decimal Quantity { get; set; }
        public System.Guid UnitId { get; set; }
        public decimal BaseQuantity { get; set; }
        public System.Guid BaseUnitId { get; set; }


    }
}
