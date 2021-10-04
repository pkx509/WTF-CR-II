using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DITS.HILI.WMS.ReceiveAPIs.Models
{
    public class mdlMobileReceiveProductDetail
    {
        public Guid receiveID { get; set; }
        public Guid receiveDetailID { get; set; }
        public Guid productID { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public decimal? quantity { get; set; } //inform QTY
        public decimal? baseQuantity { get; set; } //conversionQty x quantity
        public decimal? conversionQty { get; set; } //relate QTY
        public Guid quantityUnitID { get; set; }
        public string quantityUnitCode { get; set; }
        public string quantityUnitName { get; set; }
        public string conversionQtyUnitCode { get; set; }
        public string conversionQtyUnitName { get; set; }
        public string palletCode { get; set; }
        public string lot { get; set; }
        public Guid productStatusID { get; set; }
        public string productStatus_Code { get; set; }
        public string productStatus_Name { get; set; }
        public Guid? productSubStatusID { get; set; }
        public string productSubStatus_Code { get; set; }
        public string productSubStatus_Name { get; set; }
        public string productRemark { get; set; }
        public Guid locationID { get; set; }
        public string location_Code { get; set; }
        public DateTime? mfgDate { get; set; }
        public DateTime? expDate { get; set; }
    }
}