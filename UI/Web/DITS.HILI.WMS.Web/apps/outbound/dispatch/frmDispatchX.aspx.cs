using Ext.Net;
using System;
using System.Configuration;

namespace DITS.HILI.WMS.Web.apps.outbound.dispatch
{
    public partial class frmDispatchX : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MasterModel.Secure.UserAccounts user = ClientService.Common.User;

            if (ConfigurationManager.AppSettings["powerUser"].ToString() != user.UserName)
            {
                Response.Redirect("~/apps/Default.aspx");
            }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {

            Core.Domain.ApiResponseMessage ok = ClientService.Outbound.DispatchClient.OnCancelDispatchComplete(txtPONo.Text, cmbDisType.SelectedItem.Value).Result;

            if (ok.ResponseCode == "0")
            {
                MessageBoxExt.Show();
            }
            else
            {

            }
        }
    }

}