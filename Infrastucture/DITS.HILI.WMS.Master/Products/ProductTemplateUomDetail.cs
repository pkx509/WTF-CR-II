using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class ProductTemplateUomDetail : BaseEntity
    {
        public Guid Product_UOM_Template_Detail_ID { get; set; } // Product_UOM_Template_Detail_ID (Primary key)
        public Guid Product_UOM_Template_ID { get; set; } // Product_UOM_Template_ID
        public string Product_UOM_Template_Detail_Name { get; set; } // Product_UOM_Template_Detail_Name (length: 50)
        public string Product_UOM_Template_Detail_Short_Name { get; set; } // Product_UOM_Template_Detail_Short_Name (length: 50)
        public double? Product_UOM_Template_Detail_Quantity { get; set; } // Product_UOM_Template_Detail_Quantity
        public int? Product_UOM_Template_Detail_SKU { get; set; } // Product_UOM_Template_Detail_SKU
        public int? Product_UOM_Template_Detail_Status { get; set; } // Product_UOM_Template_Detail_Status
        public double? Product_UOM_Template_Detail_Package_Width { get; set; } // Product_UOM_Template_Detail_Package_Width
        public double? Product_UOM_Template_Detail_Package_Length { get; set; } // Product_UOM_Template_Detail_Package_Length
        public double? Product_UOM_Template_Detail_Package_Height { get; set; } // Product_UOM_Template_Detail_Package_Height
        public double? Product_UOM_Template_Detail_Package_Weight { get; set; } // Product_UOM_Template_Detail_Package_Weight
        public string Product_UOM_Template_Detail_Barcode { get; set; } // Product_UOM_Template_Detail_Barcode (length: 50)
        public string Product_UOM_Template_Detail_Win_Code { get; set; } // Product_UOM_Template_Detail_Win_Code (length: 50)
        public bool IsActive { get; set; } // IsActive
        public double? Product_UOM_Template_Detail_Weight { get; set; } // Product_UOM_Template_Detail_Weight

        [NotMapped]
        public double? Product_UOM_Template_Detail_Gross_Weight { get; set; }
        [NotMapped]
        public string Product_UOM_Template_Detail_SKU_Text { get; set; }


        public virtual ProductTemplateUom ProductTemplateUom { get; set; }

    }
}
