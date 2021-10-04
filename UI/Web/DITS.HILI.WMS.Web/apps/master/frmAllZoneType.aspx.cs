using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllZoneType : BaseUIPage
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
            System.Guid? id = new Guid();
            if (string.IsNullOrWhiteSpace(prms.Query))
            {
                id = null;
            }

            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = WarehouseClient.GetZoneType(id, txtSearch.Text, ckbIsActive.Checked, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<ZoneType> data = new List<ZoneType>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ZoneType>>();
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

        private void GetAddEditForm(string oDataKeyId)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("ZONE_TYPE");
            WindowShow.Show(this, strTitle, "CreateZoneType", "AddEdit/frmCreateZoneType.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 450, 235);
        }

        protected void btnSearch_Event(object sender, DirectEventArgs e)
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
                Core.Domain.ApiResponseMessage ok = ClientService.Master.WarehouseClient.RemoveZoneType(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                }
            }
        }


        #region "DirectMethod"
        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
        #endregion
    }
}