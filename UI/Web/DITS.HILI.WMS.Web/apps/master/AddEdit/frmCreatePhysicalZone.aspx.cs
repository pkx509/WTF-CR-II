using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreatePhysicalZone : BaseUIPage
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
        private void getZoneType()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "ZoneType" }
            };
            StoreZoneType.AutoCompleteProxy(AppDataService, param);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                //if (IsPostBack)
                //    return;

                string code = Request.QueryString["oDataKeyId"];

                if (code != "new")
                {
                    getDataZone(code);
                }
                else
                {
                    txtPhysicalZone_Code_Key.Text = code;
                }

                //getWarehouse();
                //getZoneType();
            }
        }

        private async void getDataZone(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetZoneByID(id, null, null).Result;
                Zone _data = data.Get<Zone>();
                if (_data == null)
                {
                    return;
                }

                txtPhysicalZone_Code.Text = _data.Code;
                txtName.Text = _data.Name;
                txtShortName.Text = _data.ShortName;

                cmbWarehouseName.SetAutoCompleteValue(new List<Warehouse>
                       {
                               new Warehouse
                        {
                            WarehouseID = _data.Warehouse.WarehouseID,
                            Code = _data.Code,
                            Name = _data.Warehouse.Name
                        }
                       }
                 );

                cmbZoneType.SetAutoCompleteValue(new List<Zone>
                        {
                           new Zone
                            {
                                ZoneTypeID = _data.ZoneType.ZoneTypeID,
                                Code = _data.Code,
                                Name = _data.ZoneType.Name
                            }
                        }
                 );

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
                Zone _zone = new Zone();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _zone.ZoneID = new Guid(id);
                }

                _zone.Code = txtPhysicalZone_Code.Text;
                _zone.Name = txtName.Text;
                _zone.ShortName = txtShortName.Text;
                _zone.IsActive = txtIsActive.Checked;

                if (!string.IsNullOrWhiteSpace(cmbWarehouseName.SelectedItem.Text))
                {
                    _zone.WarehouseID = new Guid(cmbWarehouseName.SelectedItem.Value);
                }
                else
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00060").MessageValue);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(cmbZoneType.SelectedItem.Text))
                {
                    _zone.ZoneTypeID = new Guid(cmbZoneType.SelectedItem.Value);
                }
                else
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00061").MessageValue);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txtShortName.Text))
                {
                    _zone.ShortName = txtShortName.Text;
                }
                else
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00062").MessageValue);
                    return;
                }

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtPhysicalZone_Code_Key.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.Add(_zone).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.Modify(_zone).Result;
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
            if (Request.QueryString["oDataKeyId"] == "new")
            {
                //this.txtJobNo.Text = "new";
                txtPhysicalZone_Code.Clear();
                txtName.Clear();
                txtShortName.Clear();
                //this.btnSave.Hide();
                //this.txtIsActive.Clear();
                cmbWarehouseName.Reset();
                cmbZoneType.Reset();
            }
            else
            {
                getDataZone(Request.QueryString["oDataKeyId"]);
            }
        }


    }
}