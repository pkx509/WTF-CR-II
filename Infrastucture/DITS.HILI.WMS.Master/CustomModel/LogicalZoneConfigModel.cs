using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class LogicalZoneConfigModel
    {

        public Guid LogicalConfigID { get; set; }
        public Guid LogicalZoneID { get; set; }
        public Guid ConfigVariable { get; set; }
        public Guid ConfigID { get; set; }
        public string ConfigName { get; set; }
        public string ConfigCode { get; set; }
        public string ConfigValue { get; set; }
        public Guid? ConfigValueId { get; set; }
        public int? ConfigSeq { get; set; }

    }
}
