using DITS.HILI.WMS.ReceiveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DITS.HILI.WMS.ReceiveAPIs.Models
{
    public class mdlMobileReceive
    {
        public Guid ReceiveID { get; set; }
        public string ReceiveCode { get; set; }
        public DateTime EstimateDate { get; set; }
        public ReceiveStatusEnum ReceiveStatus { get; set; }
        public string ReceiveStatus_Name { get; set; }
        public string InvoiceNo { get; set; }
        public string PONumber { get; set; }
        public string ContainerNo { get; set; }
        public string Location_Code { get; set; }
        public string Supplier_Name { get; set; }
        public int productCount { get; set; }
    }
}