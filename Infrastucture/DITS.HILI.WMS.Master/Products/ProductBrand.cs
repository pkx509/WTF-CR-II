using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductBrand : BaseEntity
    {
        public Guid ProductBrandID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public virtual ICollection<Product> ProductCollection { get; set; }

        //public ProductBrand()
        //{
        //    ProductCollection = new List<Product>();
        //}
    }
}
