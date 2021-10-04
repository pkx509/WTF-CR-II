using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductGroupLevel2 : BaseEntity
    {
        public Guid ProductGroupLevel2ID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public Guid? ProductGroupLevel1ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }
        public virtual ProductGroupLevel1 ProductGroupLevel1 { get; set; }
        public virtual ICollection<ProductGroupLevel3> ProductGroupLevel3Collection { get; set; }
        public ProductGroupLevel2()
        {
            ProductGroupLevel3Collection = new List<ProductGroupLevel3>();
        }
    }
}
