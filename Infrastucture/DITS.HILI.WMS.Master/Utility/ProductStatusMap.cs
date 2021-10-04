using System;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ProductStatusMap
    {
        public Guid ProductStatusID { get; set; }
        public Guid ProductSubStatusID { get; set; }
        public bool IsDefault { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        public virtual ProductSubStatus ProductSubStatus { get; set; }

        public ProductStatusMap()
        { }
    }
}
