using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateTruckNo : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string code = Request.QueryString["oDataKeyId"];

                if (code != "new")
                {
                    getDataDock(code);
                }
                else
                {
                    txtTruck.Text = code;
                }
            }
        }

        private async void getDataDock(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetTruckID(id).Result;
                TruckNoModel _data = data.Get<TruckNoModel>();
                if (_data == null)
                {
                    return;
                }

                txtTruckNo.Text = _data.TruckNo;
                txtIsActive.Value = _data.IsActive;

                cmbTruckType.SetAutoCompleteValue(new List<TruckType> {
                        new TruckType
                        {
                            TruckTypeID = _data.TruckTypeID.Value,
                            TypeName = _data.TypeName
                        }
                   });

                SetButton("Edit");
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
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
                Truck _truck = new Truck();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _truck.TruckID = new Guid(id);
                }

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                _truck.TruckNo = txtTruckNo.Text;
                _truck.TruckTypeID = new Guid(cmbTruckType.SelectedItem.Value);
                _truck.IsActive = txtIsActive.Checked;

                if (txtTruck.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.AddTruckNo(_truck).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.ModifyTruckNo(_truck).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
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
            X.Call("parent.App.direct.Reload", "");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (txtTruckNo.Text == "new")
            {
                txtTruckNo.Text = "new";
                cmbTruckType.Clear();
                btnSave.Hide();
            }
            else
            {
                getDataDock(txtTruckNo.Text);
            }
        }
    }
}