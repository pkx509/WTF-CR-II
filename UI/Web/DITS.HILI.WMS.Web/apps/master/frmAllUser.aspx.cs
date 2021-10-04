using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;


namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllUser : BaseUIPage
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
            Core.Domain.ApiResponseMessage apiResp = UserAccountClient.GetAll(txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<UserAccounts> data = new List<UserAccounts>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<UserAccounts>>();

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
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("PROGRAM_INFO");
            WindowShow.Show(this, strTitle, "CreateProgram", "AddEdit/frmCreateUser.aspx?oDataKeyId=" + oDataKeyId, Icon.Add, 500, 200);
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
                Core.Domain.ApiResponseMessage ok = ClientService.Master.UserAccountClient.Remove(Id).Result;
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




        protected void RowRole_Select(object sender, DirectEventArgs e)
        {
            try
            {
                string UserID = e.ExtraParams["UserID"];
                if (string.IsNullOrEmpty(UserID))
                {
                    return;
                }

                LoadRoles(new Guid(UserID));

                LoadGroup(new Guid(UserID));
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

                List<UserInRole> userModelList = new List<UserInRole>();
                foreach (Dictionary<string, string> obj in gridData)
                {

                    UserInRole userModelItem = new UserInRole
                    {
                        IsRole = obj["IsRole"].ToBool(),
                        UserID = new Guid(obj["UserID"].ToString()),
                        RoleID = new Guid(obj["RoleID"].ToString())
                    };

                    userModelList.Add(userModelItem);
                }

                Core.Domain.ApiResponseMessage apiResp = UserAccountClient.SaveRole(userModelList).Result;
                List<UserInRole> data = new List<UserInRole>();
                if (apiResp.ResponseCode == "0")
                {
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void btnSaveGroup_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string gridJson = e.ExtraParams["ParamStorePages"];
                // Array of Dictionaries
                Dictionary<string, string>[] gridData = JSON.Deserialize<Dictionary<string, string>[]>(gridJson);

                List<UserInGroup> userModelList = new List<UserInGroup>();
                foreach (Dictionary<string, string> obj in gridData)
                {

                    UserInGroup userModelItem = new UserInGroup
                    {
                        IsGroup = obj["IsGroup"].ToBool(),
                        UserID = new Guid(obj["UserID"].ToString()),
                        GroupID = new Guid(obj["GroupID"].ToString())
                    };

                    userModelList.Add(userModelItem);
                }

                Core.Domain.ApiResponseMessage apiResp = UserAccountClient.SaveGroup(userModelList).Result;
                List<UserInGroup> data = new List<UserInGroup>();
                if (apiResp.ResponseCode == "0")
                {

                    Clear();
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }



        private void LoadGroup(Guid id)
        {
            try
            {
                btnSaveGroup.Enable();

                Core.Domain.ApiResponseMessage apiResp = UserAccountClient.getGroup(id).Result;
                List<UserInGroup> data = new List<UserInGroup>();
                if (apiResp.ResponseCode == "0")
                {

                    data = apiResp.Get<List<UserInGroup>>();
                }
                Store1.DataSource = data;
                Store1.DataBind();

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        private void LoadRoles(Guid id)
        {
            try
            {
                btnSavePage.Enable();

                Core.Domain.ApiResponseMessage apiResp = UserAccountClient.getRole(id).Result;
                List<UserInRole> data = new List<UserInRole>();
                if (apiResp.ResponseCode == "0")
                {

                    data = apiResp.Get<List<UserInRole>>();
                }
                StorePages.DataSource = data;
                StorePages.DataBind();

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
            btnSaveGroup.Disable();
            StorePages.RemoveAll();
            Store1.RemoveAll();
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