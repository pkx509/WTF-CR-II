using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductGroupLevel3 : BaseEntity
    {
        public Guid ProductGroupLevel3ID { get; set; }
        public Guid ProductOwnerID { get; set; }
        public Guid? ProductGroupLevel2ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }
        public virtual ProductGroupLevel2 ProductGroupLevel2 { get; set; }
        public virtual ICollection<Product> ProductCollection { get; set; }
        public virtual ICollection<LogicalZoneGroupDetail> LogicalZoneGroupDetail { get; set; }
        public ProductGroupLevel3()
        {
            LogicalZoneGroupDetail = new List<LogicalZoneGroupDetail>();
            ProductCollection = new List<Product>();
        }
    }
}
