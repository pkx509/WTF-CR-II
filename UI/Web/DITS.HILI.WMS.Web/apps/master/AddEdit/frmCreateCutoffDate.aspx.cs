using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateCutoffDate : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void BindData(string oDataKeyId)
        {
            try
            {
                if (oDataKeyId == "new")
                {
                    return;
                }

                Guid id = new Guid(oDataKeyId);
                Core.Domain.ApiResponseMessage data = ClientService.Master.MonthEndClient.GetById(id).Result;
                Monthend _data = data.Get<List<Monthend>>().FirstOrDefault();
                if (_data == null)
                {
                    return;
                }

                txtCutoffID.Text = oDataKeyId;
                dtCutoffDate.SelectedDate = _data.CutOffDate;
                dtCutoffTime.SelectedTime = new TimeSpan(_data.CutOffDate.Hour, _data.CutOffDate.Minute, _data.CutOffDate.Second);
            }
            catch
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                DateTime cutoffdate = dtCutoffDate.SelectedDate;
                TimeSpan time = dtCutoffTime.SelectedTime;
                Monthend monthenObj = new Monthend()
                {
                    CutOffDate = new DateTime(cutoffdate.Year, cutoffdate.Month, cutoffdate.Day, time.Hours, time.Minutes, 0, DateTimeKind.Utc)
                };
                System.Threading.Tasks.Task<Core.Domain.ApiResponseMessage> apiResp = ClientService.Master.MonthEndClient.CreateOrUpdate(monthenObj);
                if (apiResp.Result.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiResp.Result.ResponseMessage);
                }
                else
                {
                    NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                    X.Call("parent.App.direct.Reload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            X.Call("parent.App.direct.Exit");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            FormPanelDetail.Reset();
            BindData(Request.QueryString["oDataKeyId"]);
        }
    }
}