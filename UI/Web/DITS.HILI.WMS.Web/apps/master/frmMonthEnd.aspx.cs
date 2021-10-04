using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmMonthEnd : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }
        [Ext.Net.DirectMethod(Timeout = 900000)]
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }
        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["DataKeyId"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }

            GetAddEditForm(oDataKeyId);
        }

        private void GetAddEditForm(string oDataKeyId)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("MONTHENDDATE");
            WindowShow.Show(this, strTitle, "Create Cutoff Date", "AddEdit/frmCreateCutoffDate.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 400, 300);
        }
        [Ext.Net.DirectMethod(Timeout = 900000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            Ext.Net.StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(extraParams);
            int total = 0;
            Core.Domain.ApiResponseMessage apiResp = ClientService.Master.MonthEndClient.GetAll(prms.Page, null).Result;
            List<Monthend> data = new List<Monthend>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<Monthend>>();
            }
            return new { data, total };
        }
        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void CommandClick(object sender, Ext.Net.DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId);
            }
            else if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage data = ClientService.Master.MonthEndClient.GetById(Id).Result;
                Monthend _data = data.Get<List<Monthend>>().FirstOrDefault();
                if (_data == null)
                {
                    return;
                }

                System.Threading.Tasks.Task<Core.Domain.ApiResponseMessage> apiResp = ClientService.Master.MonthEndClient.Delete(_data);
                if (apiResp.Result.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiResp.Result.ResponseMessage);
                }
                else
                {
                    NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
            }
        }
        [DirectMethod(Timeout=180000)]
        public void Reload()
        {
            NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void Exit()
        {
            PagingToolbar1.MoveFirst();
        }

    }
}