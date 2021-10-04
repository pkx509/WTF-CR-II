using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.ProductionControl
{
    public partial class frmCancelPallet : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData(Request.QueryString["packingID"], Request.QueryString["palletCode"]);
        }

        private void BindData(string packingDetailID, string palletCode)
        {
            hdPackingDetailID.SetValue(packingDetailID);
            txtPalletCode.Text = palletCode;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            Guid packingID = Guid.Empty;
            if (!Guid.TryParse(hdPackingDetailID.Text, out packingID))
            {
                MessageBoxExt.Warning("Data not found");
                return;
            }

            CancelPalletModel cancelPallet = new CancelPalletModel()
            {
                PackingID = packingID,
                UserName = txtUsername.Text,
                Password = txtPassword.Text,
                Remark = txtRemark.Text
            };

            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.CancelPallet(cancelPallet).Result;

            if (apiResp.IsSuccess)
            {
                X.Call("parent.App.direct.Reload", apiResp);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }
        }
    }
}