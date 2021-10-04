using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DispatchModel
{
    public class DispatchDetail : BaseEntity
    {
        public System.Guid DispatchDetailId { get; set; } // DispatchDetailID (Primary key)
        public System.Guid DispatchId { get; set; } // DispatchID
        public int Sequence { get; set; } // Sequence
        public System.Guid ProductId { get; set; } // ProductID
        public System.Guid StockUnitId { get; set; } // StockUnitID
        public decimal? Quantity { get; set; } // Quantity
        public decimal? BaseQuantity { get; set; } // BasicQty
        public System.Guid? BaseUnitId { get; set; } // BasicUnitID
        public decimal? ConversionQty { get; set; } // ConversionQty (length: 10)
        public System.Guid ProductOwnerId { get; set; } // ProductOwnerID
        public decimal? DispatchDetailProductWidth { get; set; } // DispatchDetail_Product_Width
        public decimal? DispatchDetailProductLength { get; set; } // DispatchDetail_Product_Length
        public decimal? DispatchDetailProductHeight { get; set; } // DispatchDetail_Product_Height
        public System.Guid? DispatchPriceUnitId { get; set; } // DispatchPriceUnitID
        public decimal? DispatchPrice { get; set; } // DispatchPrice
        public DispatchDetailStatusEnum DispatchDetailStatus { get; set; } // DispatchDetail_Status
        public System.Guid? RuleId { get; set; } // RuleID
        public System.Guid? ProductStatusId { get; set; } // ProductStatusID
        public System.Guid? ProductSubStatusId { get; set; } // ProductSubStatusID

        public DateTime? ReviseDateTime { get; set; } // ReviseDateTime
        public string ReviseReason { get; set; } // ReviseReason
        public bool? IsBackOrder { get; set; } // IsBackOrder
        public decimal? BackOrderQuantity { get; set; } // IsBackOrder

        public virtual Dispatch Dispatch { get; set; } // FK_dispatchID
        public bool? IsSentInterface { get; set; }

        public virtual ICollection<DispatchBooking> DispatchBookings { get; set; } // bk_dispatch_booking.FK_dispatchDetailID

        public DispatchDetail()
        {
            DispatchBookings = new List<DispatchBooking>();
        }

    }
}
