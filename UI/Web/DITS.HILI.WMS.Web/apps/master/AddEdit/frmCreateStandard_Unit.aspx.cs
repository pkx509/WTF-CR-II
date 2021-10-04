using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateStandard_Unit : BaseUIPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["IsPopup"] != null)
                {
                    hidIsPopup.Text = "true";
                }
                else
                {
                    getDataUnit(Request.QueryString["oDataKeyId"]);
                }

            }
        }

        private void getDataUnit(string code)
        {
            try
            {
                if (code == "new")
                {
                    return;
                }

                Guid id = new Guid(code);
                Core.Domain.ApiResponseMessage data = ClientService.Master.UnitsClient.GetByID(id).Result;
                Units _data = data.Get<Units>();
                if (_data == null)
                {
                    return;
                }

                txtStdUnit_Name.Text = _data.Name;
                txtStdUnit_ShortName.Text = _data.ShortName;
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
                Units _units = new Units();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new" && id != null)
                {
                    _units.UnitID = new Guid(id);
                    txtStdUnit_ID.Text = _units.UnitID.ToString();
                }
                else
                {
                    txtStdUnit_ID.Text = "new";
                }


                _units.Name = txtStdUnit_Name.Text;
                _units.ShortName = txtStdUnit_ShortName.Text;

                bool isSuccess = true;
                if (txtStdUnit_ID.Text == "new")
                {
                    Core.Domain.ApiResponseMessage datasave = ClientService.Master.UnitsClient.AddUnit(_units).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    Core.Domain.ApiResponseMessage datamodify = ClientService.Master.UnitsClient.ModifyUnit(_units).Result;
                    if (datamodify.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datamodify.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    if (hidIsPopup.Text != "")
                    {
                        X.Call("parent.App.direct.ReloadUOMMethod", txtStdUnit_Name.Text);
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    }
                    else
                    {
                        X.Call("parent.App.direct.Reload", txtStdUnit_ID.Text);
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    }


                }
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.Reload", "");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

    }
}