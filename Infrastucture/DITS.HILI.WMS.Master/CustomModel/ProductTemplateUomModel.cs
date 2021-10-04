using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductTemplateUomModel
    {
        public Guid ProductUomTemplateId { get; set; } // Product_UOM_Template_ID (Primary key)
        public string ProductUomTemplateName { get; set; } // Product_UOM_Template_Name (length: 50)
        public string CreateUser { get; set; } // Create_User (length: 50)
        public bool IsActive { get; set; } // IsActive


        public virtual ICollection<ProductTemplateUomDetail> ProductTemplateUomDetailCollection { get; set; }
    }
}
