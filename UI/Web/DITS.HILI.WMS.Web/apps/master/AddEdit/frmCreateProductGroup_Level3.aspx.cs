using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateProductGroup_Level3 : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";
        private void getWarehouseType()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "ProductGroupLevel2" }
            };
            StoreProductGroupL2.AutoCompleteProxy(AppDataService, param);
        }

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
                    getData(code);
                }
                else
                {
                    hidProductGroupLevel3ID.Text = code;
                }
            }
        }

        private void getData(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.ProductGroupLevelClient.GetProductGroupLevelAll(id, null, true, null, null).Result;
                List<ProductGroupLevel3> _data = data.Get<List<ProductGroupLevel3>>();
                if (_data == null)
                {
                    return;
                }

                txtName.Text = _data.SingleOrDefault().Name;
                txtDescription.Text = _data.SingleOrDefault().Description;
                txtIsActive.Checked = _data.SingleOrDefault().IsActive;

                cmbProductGroupL2.SetAutoCompleteValue(new List<ProductGroupLevel2> {
                        new ProductGroupLevel2
                        {
                            ProductGroupLevel2ID = _data.SingleOrDefault().ProductGroupLevel2.ProductGroupLevel2ID,
                            Name = _data.SingleOrDefault().ProductGroupLevel2.Name
                        }
                   });

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
                ProductGroupLevel3 _productGroupLevel3 = new ProductGroupLevel3();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _productGroupLevel3.ProductGroupLevel3ID = new Guid(id);
                }

                _productGroupLevel3.ProductGroupLevel2ID = new Guid(cmbProductGroupL2.SelectedItem.Value);
                _productGroupLevel3.Name = txtName.Text;
                _productGroupLevel3.Description = txtDescription.Text;
                _productGroupLevel3.IsActive = txtIsActive.Checked;

                ApiResponseMessage datasave = new ApiResponseMessage();
                if (hidProductGroupLevel3ID.Text == "new")
                {
                    datasave = ClientService.Master.ProductGroupLevelClient.Add(_productGroupLevel3).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                    else
                    {
                        X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    }
                }
                else
                {
                    datasave = ClientService.Master.ProductGroupLevelClient.Modify(_productGroupLevel3).Result;
                    if (datasave.ResponseCode != "0")
                    {
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                    else
                    {
                        X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (hidProductGroupLevel3ID.Text == "new")
            {
                txtDescription.Clear();
                txtName.Clear();
                btnSave.Hide();
            }
            else
            {
                getData(hidProductGroupLevel3ID.Text);
            }
        }
    }
}