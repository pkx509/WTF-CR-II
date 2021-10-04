using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmCreateUser : BaseUIPage
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
                    getData(code);
                }
            }
        }

        private void getData(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.UserAccountClient.GetByID(id).Result;
                UserAccounts _data = data.Get<UserAccounts>();
                if (_data == null)
                {
                    return;
                }

                txtUserName.Text = _data.UserName;
                txtUserName.ReadOnly = true;
                //this.txtPassword.ReadOnly = true; 
                //this.txtConfirmPassword.ReadOnly = true;
                //this.txtPassword.AllowBlank = true;
                //this.txtConfirmPassword.AllowBlank = true;
                txtSurName.Text = _data.Employee.LastName;
                txtName.Text = _data.Employee.FirstName;
                txtEmail.Text = _data.Employee.Email;
                ChkIsActive.Checked = _data.IsActive;

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
                UserAccounts _user = new UserAccounts();

                if (hddKey.Text != "new")
                {
                    _user.UserID = new Guid(hddKey.Text);
                }

                _user.UserName = txtUserName.Text;
                _user.Password = txtPassword.Text;

                _user.IsActive = ChkIsActive.Checked;
                _user.Employee = new Employee
                {
                    FirstName = txtName.Text,
                    LastName = txtSurName.Text,
                    Email = txtEmail.Text
                };

                // _user.IsActive = txtIsActive.Checked;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (hddKey.Text == "new")
                {
                    datasave = ClientService.Master.UserAccountClient.Add(_user).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.UserAccountClient.Modify(_user).Result;
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
            //if (hddKey.Text == "new")
            //{
            //    this.txtDescription.Clear();
            //    this.txtName.Clear();
            //    this.btnSave.Hide();
            //} 
        }
    }
}