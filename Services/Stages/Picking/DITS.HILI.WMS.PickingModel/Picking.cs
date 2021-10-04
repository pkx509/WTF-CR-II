using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.PickingModel
{
    public class Picking : BaseEntity
    {
        public Picking()
        {
            PickingAssignCollection = new List<PickingAssign>();
        }

        public Guid PickingID { get; set; }
        public string PickingCode { get; set; }
        public DateTime? PickingStartDate { get; set; }
        public DateTime? PickingCompleteDate { get; set; }
        public PickingStatusEnum PickingStatus { get; set; }
        public string PickingCloseReason { get; set; }
        public string ShippingCode { get; set; }
        public Guid? EmployeeAssignID { get; set; }
        public string DispatchCode { get; set; }
        public DateTime? PickingEntryDate { get; set; }
        public string DocumentNo { get; set; }
        public string OrderNo { get; set; }
        public string PONo { get; set; }


        public virtual ICollection<PickingAssign> PickingAssignCollection { get; set; }
    }
}
