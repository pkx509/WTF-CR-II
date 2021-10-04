using DITS.HILI.WMS.ClientService.Inbound;
using DITS.HILI.WMS.ReceiveModel;
using System;

namespace DITS.HILI.WMS.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/apps");

        }

        private async void Save()
        {
            try
            {
                ///แด่ ตั้ม
                Receive r = new Receive();
                Receive result = await ReceiveClient.Add(r);


            }
            catch (Exception)
            {

            }
        }
    }
}