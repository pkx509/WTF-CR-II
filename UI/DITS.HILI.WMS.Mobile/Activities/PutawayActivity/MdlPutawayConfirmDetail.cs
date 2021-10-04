using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DITS.HILI.WMS.Mobile.Activities.PutawayActivity
{
    public class MdlPutawayConfirmDetail
    {
        public string putawayID { get; set; }
        public string palletCode { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public decimal Quantity { get; set; }
        public decimal remainQuantity { get; set; }
        public string QuantityUnit { get; set; }
        public string lot { get; set; }
        public string productImage { get; set; }
        public string locationCode { get; set; }
        public string suggLocationCode { get; set; }
        public string confLocationCode { get; set; }
        public string stockUnitID { get; set; }
        public string baseUnitID { get; set; }
        public decimal baseQuantity { get; set; }
        public decimal ConversionQty { get; set; }
        public string whereHouseName { get; set; }
    }
}