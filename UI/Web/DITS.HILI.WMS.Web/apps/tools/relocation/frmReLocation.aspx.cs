using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.tools.relocation
{
    public partial class frmReLocation : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [DirectMethod(Timeout=180000)]
        public void GetProduct()
        {
            ucProductReLocationSelect.Show();
        }
    }
}