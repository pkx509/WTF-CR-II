using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class GoodsReturn : BaseEntity
    {
        public Guid GoodsReturnID { get; set; }
        public string GoodsReturnCode { get; set; }
        public Guid? ReceiveID { get; set; }
        public GoodsReturnStatusEnum? GoodsReturnStatus { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Description { get; set; }

        public Guid? ApproveBy { get; set; }
        public virtual ICollection<GoodsReturnDetail> GoodsReturnDetailCollection { get; set; }

        [NotMapped]
        public string ReceiveCode { get; set; }
        [NotMapped]
        public string PONumber { get; set; }
        [NotMapped]
        public bool IsApprove { get; set; }
        [NotMapped]
        public string GoodsReturnStatusName { get; set; }
        [NotMapped]
        public DateTime ReceiveDate { get; set; }
        [NotMapped]
        public List<ItemGoodsReturn> ItemGoodsReturns { get; set; }
    }
}
