using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductCodes : BaseEntity
    {
        public Guid ProductID { get; set; }
        public string Code { get; set; }
        public ProductCodeTypeEnum CodeType { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }

        public virtual Product Product { get; set; }
    }
}
