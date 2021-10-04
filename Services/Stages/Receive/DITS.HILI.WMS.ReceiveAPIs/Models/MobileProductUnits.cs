using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DITS.HILI.WMS.ReceiveAPIs.Models
{
    public class mdlMobileProductUnits
    {
        public string unitCode { get; set; }
        public string unitBarCode { get; set; }
        public string unitName { get; set; }
        public decimal QTY { get; set; }
    }
}