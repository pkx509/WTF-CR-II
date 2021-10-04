using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateDock : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";
        private void getWarehouse()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "Warehouse" }
            };
            StoreWarehouseName.AutoCompleteProxy(AppDataService, param);
        }
        private void getTruckType()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "TruckType" }
            };
            StoreTruckType.AutoCompleteProxy(AppDataService, param);
        }

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
                    txtDockCode.Text = code;
                    txtDockCode.Hidden = true;
                    labDockCode.Hidden = true;
                }
            }
        }

        private async void getDataDock(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetDockID(id, null, null).Result;
                DockConfig _data = data.Get<DockConfig>();
                if (_data == null)
                {
                    return;
                }

                txtDockCode.Text = _data.Barcode;
                txtDockName.Text = _data.DockName;

                cmbWarehouseName.SetAutoCompleteValue(new List<Warehouse>
                       {
                               new Warehouse
                        {
                            WarehouseID = _data.Warehouse.WarehouseID,
                            Name = _data.Warehouse.Name
                        }
                       }
                 );

                cmbTruckType.SetAutoCompleteValue(new List<TruckType>
                        {
                           new TruckType
                            {
                                TruckTypeID = _data.TruckTypeID.Value,
                                TypeName = _data.TruckType.TypeName,
                            }
                        }
                 );


                labDockCode.Hidden = true;
                txtDockCode.Hidden = true;

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
                DockConfig _dock = new DockConfig();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _dock.DockConfigID = new Guid(id);
                }

                _dock.Barcode = "*" + txtDockName.Text + "*";
                _dock.TruckTypeID = new Guid(cmbTruckType.SelectedItem.Value);
                _dock.WarehouseID = new Guid(cmbWarehouseName.SelectedItem.Value);
                _dock.DockName = txtDockName.Text;
                _dock.IsActive = txtIsActive.Checked;

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (id == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.Add(_dock).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.Modify(_dock).Result;
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
            if (txtDockCode.Text == "new")
            {
                txtDockCode.Text = "new";
                txtDockCode.Clear();
                txtDockName.Clear();
                btnSave.Hide();
                cmbWarehouseName.Reset();
                cmbTruckType.Reset();
                //SetButton(0);
            }
            else
            {
                getDataDock(txtDockCode.Text);
            }
        }
    }
}