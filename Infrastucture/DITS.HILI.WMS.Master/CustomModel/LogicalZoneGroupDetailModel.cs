using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneGroupDetailModel
    {
        public Guid LogicalZoneGroupDetailId { get; set; }
        public Guid LogicalZoneGroupId { get; set; }
        public Guid? LogicalZoneGroupLevel3Id { get; set; }
        public string LogicalZoneGroupLevel3Name { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool IsActive { get; set; }

    }
}
