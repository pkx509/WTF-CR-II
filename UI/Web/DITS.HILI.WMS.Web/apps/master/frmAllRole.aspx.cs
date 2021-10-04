using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllRole : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

        }


        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = RoleClient.GetAll(txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<Roles> data = new List<Roles>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<Roles>>();

                Clear();
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
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("ROLE");
            WindowShow.Show(this, strTitle, "CreateProgram", "AddEdit/frmCreateRole.aspx?oDataKeyId=" + oDataKeyId, Icon.Add, 450, 220);
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
                Core.Domain.ApiResponseMessage ok = ClientService.Master.RoleClient.Remove(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    MasterModel.Core.CustomMessage msg = GetMessage("MSG00002");
                    NotificationExt.Show(msg.MessageTitle, msg.MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                }
            }
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        private void LoadPermission(Guid id)
        {
            try
            {
                btnSavePage.Enable();

                Core.Domain.ApiResponseMessage apiResp = RoleClient.getPermission(id).Result;
                List<PermissionInRole> data = new List<PermissionInRole>();
                if (apiResp.ResponseCode == "0")
                {

                    data = apiResp.Get<List<PermissionInRole>>();
                }
                StorePages.DataSource = data;
                StorePages.DataBind();

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void RowRole_Select(object sender, DirectEventArgs e)
        {
            try
            {
                string RoleID = e.ExtraParams["RoleID"];
                if (string.IsNullOrEmpty(RoleID))
                {
                    return;
                }

                LoadPermission(new Guid(RoleID));
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnSavePage_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string gridJson = e.ExtraParams["ParamStorePages"];
                // Array of Dictionaries
                Dictionary<string, string>[] gridData = JSON.Deserialize<Dictionary<string, string>[]>(gridJson);

                List<PermissionInRole> userModelList = new List<PermissionInRole>();
                foreach (Dictionary<string, string> obj in gridData)
                {

                    PermissionInRole userModelItem = new PermissionInRole
                    {
                        IsPermission = obj["IsPermission"].ToBool(),
                        PermissionID = new Guid(obj["PermissionID"].ToString()),
                        RoleID = new Guid(obj["RoleID"].ToString())
                    };

                    userModelList.Add(userModelItem);
                }

                Core.Domain.ApiResponseMessage apiResp = RoleClient.SavePermission(userModelList).Result;
                List<PermissionInRole> data = new List<PermissionInRole>();
                if (apiResp.ResponseCode == "0")
                {

                    Clear();
                }
                else
                {
                    MessageBoxExt.Warning(apiResp.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnClearPage_Click(object sender, DirectEventArgs e)
        {
            try
            {
                Clear();

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }


        public void Clear()
        {
            btnSavePage.Disable();
            StorePages.RemoveAll();
            GridPanelExt.ClearSelection(grdDataList);
        }


        #region "DirectMethod"
        [DirectMethod(Timeout=180000)]
        public void Reload()
        {
            MasterModel.Core.CustomMessage msg = GetMessage("MSG00001");
            NotificationExt.Show(msg.MessageTitle, msg.MessageValue);
            PagingToolbar1.MoveFirst();
        }
        #endregion
    }
}