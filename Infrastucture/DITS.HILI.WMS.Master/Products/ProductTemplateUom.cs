using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductTemplateUom : BaseEntity
    {
        public Guid Product_UOM_Template_ID { get; set; } // Product_UOM_Template_ID (Primary key)
        public string Product_UOM_Template_Name { get; set; } // Product_UOM_Template_Name (length: 50)
        public bool IsActive { get; set; } // IsActive

        public virtual ICollection<ProductTemplateUomDetail> ProductTemplateUomDetailCollection { get; set; }


        public ProductTemplateUom()
        {
            ProductTemplateUomDetailCollection = new List<ProductTemplateUomDetail>();
        }
    }
}
