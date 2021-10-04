using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateTruck : BaseUIPage
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
                    txtTruckTypeCode.Text = code;
                }
            }
        }

        private async void getDataDock(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetTruckTypeID(id, null, null).Result;
                TruckType _data = data.Get<TruckType>();
                if (_data == null)
                {
                    return;
                }

                txtTruckTypeName.Text = _data.TypeName;
                txtDescription.Text = _data.Description;
                txtEstimateTime.SetValue(_data.EsitmateTime.GetValueOrDefault());
                txtIsActive.Value = _data.IsDefault == null ? false : _data.IsDefault.Value;
                chkEquipment.Value = _data.EquipmentFlag == null ? false : _data.EquipmentFlag.Value;
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
                TruckType _truckType = new TruckType();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _truckType.TruckTypeID = new Guid(id);
                }

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                _truckType.TypeName = txtTruckTypeName.Text;
                _truckType.IsDefault = txtIsActive.Checked;
                _truckType.EquipmentFlag = chkEquipment.Checked;
                _truckType.EsitmateTime = int.Parse(txtEstimateTime.Value.ToString());
                _truckType.Description = txtDescription.Text;

                if (txtTruckTypeCode.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.Add(_truckType).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.Modify(_truckType).Result;
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
            if (txtTruckTypeCode.Text == "new")
            {
                txtTruckTypeCode.Text = "new";
                txtTruckTypeName.Clear();
                btnSave.Hide();
            }
            else
            {
                getDataDock(txtTruckTypeCode.Text);
            }
        }
    }
}