using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllLocation : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            //if (IsPostBack)
            //    GetData();
        }
        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = WarehouseClient.GetAll(txtSearch.Text, ckbIsActive.Checked, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<LocationModel> data = new List<LocationModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<LocationModel>>();
            }

            return new { data, total };
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        private void GetAddEditForm(string oDataKeyId)
        {
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("LOCATION");
            Icon iconWindows = Icon.ApplicationFormEdit;

            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
                WindowShow.Show(this, strTitle, "CreateLocation",
                                     "AddEdit/frmCreateLocation.aspx?oDataKeyId=" + oDataKeyId,
                                     iconWindows, 670, 540);
            }
            else
            {
                WindowShow.Show(this, strTitle, "CreateLocation",
                                     "AddEdit/frmCreateLocation.aspx?oDataKeyId=" + oDataKeyId,
                                     iconWindows, 670, 260);
            }
        }
        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);


            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId);
            }
            if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.Master.WarehouseClient.RemoveLocation(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    MessageBoxExt.ShowError(ok.ResponseMessage);
                }
            }
        }
        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
    }
}
