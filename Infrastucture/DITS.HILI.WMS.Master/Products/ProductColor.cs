using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductColor : BaseEntity
    {
        public Guid ProductID { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Product Product { get; set; }

        public ProductColor()
        { }
    }
}
