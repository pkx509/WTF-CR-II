using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Secure;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllGroup : BaseUIPage
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
            Core.Domain.ApiResponseMessage apiResp = UserGroupClient.GetAll(txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<UserGroups> data = new List<UserGroups>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<UserGroups>>();

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
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("USER_GROUP");
            WindowShow.Show(this, strTitle, "CreateProgram", "AddEdit/frmCreateGroup.aspx?oDataKeyId=" + oDataKeyId, Icon.Add, 450, 220);
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
                Core.Domain.ApiResponseMessage ok = ClientService.Master.UserGroupClient.Remove(Id).Result;
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
                btnSelctAll.Enable();

                Core.Domain.ApiResponseMessage apiResp = UserGroupClient.getProgram(id).Result;
                List<ProgramInGroup> data = new List<ProgramInGroup>();
                if (apiResp.ResponseCode == "0")
                {

                    data = apiResp.Get<List<ProgramInGroup>>();
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
                string GroupID = e.ExtraParams["GroupID"];
                if (string.IsNullOrEmpty(GroupID))
                {
                    return;
                }

                LoadPermission(new Guid(GroupID));
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

                List<ProgramInGroup> userModelList = new List<ProgramInGroup>();

                foreach (Dictionary<string, string> obj in gridData)
                {

                    ProgramInGroup userModelItem = new ProgramInGroup
                    {
                        IsCheck = obj["IsCheck"].ToBool(),
                        ProgramID = new Guid(obj["ProgramID"].ToString()),
                        GroupID = new Guid(obj["GroupID"].ToString())
                    };

                    userModelList.Add(userModelItem);
                }

                Core.Domain.ApiResponseMessage apiResp = UserGroupClient.SaveProgram(userModelList).Result;
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

        protected void btnSelctAll_Click(object sender, DirectEventArgs e)
        {
            try
            {

                string gridJson = e.ExtraParams["ParamStorePages"];
                // Array of Dictionaries
                List<ProgramInGroup> gridData = JSON.Deserialize<List<ProgramInGroup>>(gridJson);
                List<ProgramInGroup> userModelList = new List<ProgramInGroup>();

                foreach (ProgramInGroup obj in gridData)
                {
                    ProgramInGroup userModelItem = new ProgramInGroup
                    {
                        IsCheck = true,
                        Description = obj.Description,
                        GroupID = obj.GroupID,
                        ProgramID = obj.ProgramID,
                        Remark = obj.Remark,
                        Module = obj.Module,
                        Sequence = obj.Sequence
                    };

                    userModelList.Add(userModelItem);
                }

                StorePages.DataSource = userModelList.OrderBy(x => x.Sequence);
                StorePages.DataBind();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        protected void btnClearCheckAll_Click(object sender, DirectEventArgs e)
        {
            try
            {

                string gridJson = e.ExtraParams["ParamStorePages"];
                // Array of Dictionaries
                List<ProgramInGroup> gridData = JSON.Deserialize<List<ProgramInGroup>>(gridJson);
                List<ProgramInGroup> userModelList = new List<ProgramInGroup>();

                foreach (ProgramInGroup obj in gridData)
                {
                    ProgramInGroup userModelItem = new ProgramInGroup
                    {
                        IsCheck = false,
                        Description = obj.Description,
                        GroupID = obj.GroupID,
                        ProgramID = obj.ProgramID,
                        Remark = obj.Remark,
                        Module = obj.Module,
                        Sequence = obj.Sequence
                    };

                    userModelList.Add(userModelItem);
                }

                StorePages.DataSource = userModelList.OrderBy(x => x.Sequence);
                StorePages.DataBind();
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