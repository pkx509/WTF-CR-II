using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{

    public class Reclassified : BaseEntity
    {
        public Reclassified()
        {
            ReclassifiedDetailCollection = new List<ReclassifiedDetail>();
        }

        public Guid ReclassifiedID { get; set; }
        public string ReclassifiedCode { get; set; }
        public string ReclassFromLot { get; set; }
        public string ReclassToLot { get; set; }
        public ReclassifiedStatus ReclassStatus { get; set; }
        public DateTime? ApproveDate { get; set; }

        public Guid? ApproveBy { get; set; }
        public DateTime? ApproveDispatchDate { get; set; }

        public Guid? ApproveDispatchBy { get; set; }
        public string Description { get; set; }
        public TimeSpan? MFGTimeFrom { get; set; }
        public TimeSpan? MFGTimeEnd { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? LineID { get; set; }
        public DateTime? MFGDate { get; set; }
        public DateTime? EXPDate { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? FromProductStatusID { get; set; }
        [NotMapped]
        public string ProductStatus { get; set; }
        [NotMapped]
        public string FromProductStatus { get; set; }
        [NotMapped]
        public virtual ICollection<ReclassifiedDetail> ReclassifiedDetailCollection { get; set; }
        [NotMapped]
        public string ReclassStatusName { get; set; }
        [NotMapped]
        public bool IsApprove { get; set; }
        [NotMapped]
        public bool IsApproveDispatch { get; set; }
        [NotMapped]
        public List<ItemReclassified> ReclassifiedItem { get; set; }
        [NotMapped]
        public object ProductCode { get; set; }
        [NotMapped]
        public object ProductName { get; set; }
    }
}
