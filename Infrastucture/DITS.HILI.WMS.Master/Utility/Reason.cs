using System;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class Reason : BaseEntity
    {
        public Guid ReasonID { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
        public bool? IsDefault { get; set; }
    }
}
