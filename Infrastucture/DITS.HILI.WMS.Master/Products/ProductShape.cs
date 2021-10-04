using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductShape : BaseEntity
    {
        public Guid ProductShapeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public ICollection<Product> ProductCollection { get; set; }

        //public ProductShape()
        //{
        //    ProductCollection = new List<Product>();
        //}
    }
}
