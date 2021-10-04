using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmCreateWarehouseType : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";
        //private void getWarehouseType()
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("Method", "WarehouseType");
        //    StoreWarehouseTypeName.AutoCompleteProxy(AppDataService, param);
        //}
        //private void getSite()
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("Method", "Site");
        //    StoreSiteName.AutoCompleteProxy(AppDataService, param);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsPostBack)
                {
                    return;
                }

                string code = Request.QueryString["oDataKeyId"];

                if (code != "new")
                {
                    getDataWarehouseType(code);
                }
                else
                {
                    WarehouseTypeKey.Text = code;
                }

                //getWarehouseType();
                //getSite();
            }
        }

        private void getDataWarehouseType(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetWarehouseTypeByID(id).Result;
                WarehouseType _data = data.Get<WarehouseType>();
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

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            ApiResponseMessage Warehouse = ClientService.Master.WarehouseClient.GetZone(Guid.Empty, prms.Query, prms.Page, prms.Limit).Result;
            List<Zone> data = new List<Zone>();
            if (Warehouse.IsSuccess)
            {
                total = Warehouse.Totals;
                data = Warehouse.Get<List<Zone>>();
            }

            return new { data, total };
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                WarehouseType _warehouseType = new WarehouseType();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _warehouseType.WarehouseTypeID = new Guid(id);
                }

                _warehouseType.Name = txtName.Text;
                _warehouseType.Description = txtDescription.Text;
                _warehouseType.IsActive = txtIsActive.Checked;

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (WarehouseTypeKey.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.AddWarehouseType(_warehouseType).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.ModifyWarehouseType(_warehouseType).Result;
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
            if (WarehouseTypeKey.Text == "new")
            {
                txtDescription.Clear();
                txtName.Clear();
                btnSave.Hide();
            }
            else
            {
                getDataWarehouseType(WarehouseTypeKey.Text);
            }
        }
    }
}