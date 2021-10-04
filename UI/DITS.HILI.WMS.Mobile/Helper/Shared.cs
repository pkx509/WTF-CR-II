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

namespace DITS.HILI.WMS.Mobile.Helper
{
    public class Shared
    {
        public static string Service
        {
            get
            {
                return "http://172.16.14.42/hili4.2service/api/";
                //return "http://172.16.15.20/hili4.2service/api/";
                //return "http://172.16.11.84/DITS.HILI.WMS.WebAPIs/api/";
            }
        }
    }
}