using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DITS.HILI.WMS.ReceiveAPIs.Models
{
    public class mdlMobileReceiveProducts
    {
        public Guid ReceiveID { get; set; }
        public Guid ReceiveDetailID { get; set; }
        public string receiveCode { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public decimal quantity { get; set; }
        public string unitName { get; set; }
        public string palletCode { get; set; }
    }
}