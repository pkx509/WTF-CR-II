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

namespace DITS.HILI.WMS.Mobile.Activities
{
    public class LocalLib
    {
        public LocalLib()
        {

        }

        public void mainPage(Activity currentActivity)
        {
            try
            {
                Intent Page = new Intent(currentActivity, typeof(MainActivity));
                currentActivity.StartActivity(Page);
            }
            catch
            {
                throw;
            }
        }
    }
}