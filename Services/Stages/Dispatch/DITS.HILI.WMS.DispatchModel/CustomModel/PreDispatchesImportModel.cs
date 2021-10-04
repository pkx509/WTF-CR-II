using System;
using System.ComponentModel.DataAnnotations;

namespace DITS.HILI.WMS.DispatchModel.CustomModel
{
    public class PreDispatchesImportModel
    {
        public string DispatchCode { get; set; }


        [Required(ErrorMessage = "Customer Code is required.")]
        [StringLength(10, ErrorMessage = "This Customer Code property must be a string with a maximum lenght of 10.")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Dispatch Type is required.")]
        [StringLength(50, ErrorMessage = "This Dispatch Type property must be a string with a maximum lenght of 50.")]
        public string DispatchType { get; set; }
        public int? Dispatch_Type_ID { get; set; }
        public string Dispatch_Type_Code { get; set; }



        [Required(ErrorMessage = "Est. Dispatch Date is required.")]
        public DateTime? EstDispatchDate { get; set; }

        [StringLength(50, ErrorMessage = "This PO Number property must be a string with a maximum lenght of 50.")]
        public string PONumber { get; set; }


        [StringLength(50, ErrorMessage = "This Order Number property must be a string with a maximum lenght of 50.")]
        public string OrderNumber { get; set; }


        public string IsBackOrder { get; set; }

        public string IsUrgent { get; set; }

        [Required(ErrorMessage = "Product Code is required.")]
        [StringLength(15, ErrorMessage = "This Product Code property must be a string with a maximum lenght of 15.")]
        public string ProductCode { get; set; }



        [Required(ErrorMessage = "Quantity is required.")]
        public double? Quantity { get; set; }

        [Required(ErrorMessage = "UOM is required.")]
        [StringLength(50, ErrorMessage = "This UOM property must be a string with a maximum lenght of 50.")]
        public string UOM { get; set; }

        public string Remark { get; set; }

        [StringLength(20, ErrorMessage = "This Product Status property must be a string with a maximum lenght of 20.")]
        public string Status { get; set; }
        [StringLength(20, ErrorMessage = "This Product Sub Status property must be a string with a maximum lenght of 20.")]
        public string SubStatus { get; set; }

        public double? Price { get; set; }
        public string UnitPrice { get; set; }
        public string Remark2 { get; set; }

        public DateTime? DocumentDate { get; set; }

        public string ShippingTo { get; set; }

        public DateTime? DeliveryDate { get; set; }
    }

    public class GridImportDispatchModel
    {

        public string F1 { get; set; }
        public string F2 { get; set; }
        public string F3 { get; set; }
        public DateTime? F4 { get; set; }
        public DateTime? F5 { get; set; }
        public string F6 { get; set; }
        public DateTime? F7 { get; set; }
        public string F8 { get; set; }
        public string F9 { get; set; }
        public string F10 { get; set; }
        public string F11 { get; set; }
        public string F12 { get; set; }
        public string F13 { get; set; }
        public double? F14 { get; set; }
        public string F15 { get; set; }
        public double? F16 { get; set; }
        public string F17 { get; set; }
        public string F18 { get; set; }
        public string F19 { get; set; }
    }
}
