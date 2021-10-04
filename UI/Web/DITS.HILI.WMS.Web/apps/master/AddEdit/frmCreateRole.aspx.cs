using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmCreateRole : BaseUIPage
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
                ApiResponseMessage data = ClientService.Master.RoleClient.GetByID(id).Result;
                Roles _data = data.Get<Roles>();
                if (_data == null)
                {
                    return;
                }

                txtName.Text = _data.Name;
                txtDescription.Text = _data.Description;
                txtIsActive.Value = _data.IsActive;


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
                Roles _roles = new Roles();

                if (hddKey.Text != "new")
                {
                    _roles.RoleID = new Guid(hddKey.Text);
                }

                _roles.Name = txtName.Text;
                _roles.Description = txtDescription.Text;
                _roles.IsActive = txtIsActive.Checked;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (hddKey.Text == "new")
                {
                    datasave = ClientService.Master.RoleClient.Add(_roles).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.RoleClient.Modify(_roles).Result;
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