using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmCreateGroup : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsPostBack)
                {
                    return;
                }

                string code = Request.QueryString["oDataKeyId"];
                hddKey.Text = code;
                if (code != "new")
                {
                    getDataRole(code);
                }
            }
        }

        private void getDataRole(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.UserGroupClient.GetByID(id).Result;
                UserGroups _data = data.Get<UserGroups>();
                if (_data == null)
                {
                    return;
                }

                txtName.Text = _data.GroupName;
                txtDescription.Text = _data.GroupDescription;
                txtIsActive.Checked = _data.IsActive;

                SetButton("Edit");
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        private void SetButton(string code)
        {
            switch (code)
            {
                case "Add":
                    btnSave.Disable();
                    break;
                case "Edit":
                    btnSave.Enable();
                    break;
                default: break;
            }
        }


        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                UserGroups _groups = new UserGroups();

                if (hddKey.Text != "new")
                {
                    _groups.GroupID = new Guid(hddKey.Text);
                }

                _groups.GroupName = txtName.Text;
                _groups.GroupDescription = txtDescription.Text;
                _groups.IsActive = txtIsActive.Checked;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (hddKey.Text == "new")
                {
                    datasave = ClientService.Master.UserGroupClient.Add(_groups).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.UserGroupClient.Modify(_groups).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (hddKey.Text == "new")
            {
                txtDescription.Clear();
                txtName.Clear();
                btnSave.Hide();
            }
        }
    }
}