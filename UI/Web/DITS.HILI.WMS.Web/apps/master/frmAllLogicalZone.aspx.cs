
using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllLogicalZone : BaseUIPage
    {
        public static string ProgramCode = "P-0054";



        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }

            GetAddEditForm(oDataKeyId);
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("new");
        }

        private void GetAddEditForm(string oDataKeyId)
        {

            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("LOGICALZONE");
            WindowShow.Show(this, strTitle, "CreateLogicalZone", "AddEdit/frmCreateLogicalZone.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 900, 420);
        }

        private void GetConfigForm(string oDataKeyId)
        {

            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("LOGICALZONECONFIG");
            WindowShow.Show(this, strTitle, "CreateLogicalZoneConfig", "AddEdit/frmAllLogicalZone_Config.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 380, 250);

        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
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
                Core.Domain.ApiResponseMessage ok = ClientService.Master.LogicalZoneClient.Remove(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    //NotificationExt.Show("Delete", "Delete complete");
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
            }
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = LogicalZoneClient.Get(txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<LogicalZoneModel> data = new List<LogicalZoneModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<LogicalZoneModel>>();
            }

            return new { data, total };
        }

        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            NotificationExt.Show("Save", param);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void Reload()
        {
            NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            PagingToolbar1.MoveFirst();
        }
        [DirectMethod(Timeout=180000)]
        public void Close()
        {
            PagingToolbar1.MoveFirst();
        }
    }
}