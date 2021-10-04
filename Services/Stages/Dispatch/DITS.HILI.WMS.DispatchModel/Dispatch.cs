using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.DispatchModel
{
    public class Dispatch : BaseEntity
    {
        public System.Guid DispatchId { get; set; } // DispatchID (Primary key)
        public string DispatchCode { get; set; } // DispatchCode (length: 20)
        public System.Guid? DocumentId { get; set; } // DocumentID
        public System.DateTime? OrderDate { get; set; } // OrderDate
        public System.DateTime? DocumentDate { get; set; } // DocumentDate
        public System.DateTime? DeliveryDate { get; set; } // DeliveryDate
        public System.DateTime? DocumentApproveDate { get; set; } // DocumentApproveDate
        public string Pono { get; set; } // Pono (length: 50)
        public string OrderNo { get; set; } // OrderNo (length: 50)
        public System.Guid? ShipptoId { get; set; } // ShipptoID (length: 20)
        public bool? IsUrgent { get; set; } // IsUrgent
        public bool? IsBackOrder { get; set; } // IsBackOrder
        public System.Guid? ReferenceId { get; set; } // ReferenceID
        public System.Guid? FromwarehouseId { get; set; } // FromwarehouseID
        public System.Guid? TowarehouseId { get; set; } // TowarehouseID
        public DispatchStatusEnum DispatchStatus { get; set; } // DocumentStatus
        public System.Guid? SupplierId { get; set; } // SupplierID
        public System.Guid? CustomerId { get; set; } // CustomerID
        public DateTime? ReviseDateTime { get; set; } // ReviseDateTime
        public string ReviseReason { get; set; } // ReviseReason


        public virtual System.Collections.Generic.ICollection<DispatchDetail> DispatchDetailCollection { get; set; } // dp_dispatch_detail.FK_dispatchID

        public Dispatch()
        {
            DispatchDetailCollection = new System.Collections.Generic.List<DispatchDetail>();
        }
    }
}
