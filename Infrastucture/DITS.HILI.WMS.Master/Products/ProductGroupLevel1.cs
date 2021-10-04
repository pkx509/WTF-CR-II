using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductGroupLevel1 : BaseEntity
    {
        public Guid ProductGroupLevel1ID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }
        public virtual ICollection<ProductGroupLevel2> ProductGroupLevel2Collection { get; set; }

        public ProductGroupLevel1()
        {
            ProductGroupLevel2Collection = new List<ProductGroupLevel2>();
        }
    }
}
