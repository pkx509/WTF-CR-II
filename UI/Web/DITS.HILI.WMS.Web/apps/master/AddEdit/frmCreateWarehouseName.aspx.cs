using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Companies;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateWarehouseName : BaseUIPage
    {
        private readonly string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";
        private void getWarehouseType()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "WarehouseType" }
            };
            StoreWarehouseTypeName.AutoCompleteProxy(AppDataService, param);
        }
        private void getSite()
        {
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "Site" }
            };
            StoreSiteName.AutoCompleteProxy(AppDataService, param);
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
                    getDataWarehouse(code);
                }
                else
                {
                    txtWhCode.Text = code;
                }
            }
        }

        private void getDataWarehouse(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetWarehouseByID(id, null, null).Result;
                Warehouse _data = data.Get<Warehouse>();
                if (_data == null)
                {
                    return;
                }

                txtCode.Text = _data.Code;
                txtName.Text = _data.Name;
                txtShortName.Text = _data.ShortName;
                txtDescription.Text = _data.Description;
                cmbWhCode.SelectedItem.Value = _data.ReferenceCode;
                txtSeqno.Text = _data.Seqno.ToString();
                cmdWarehouseType.SetAutoCompleteValue(new List<WarehouseType> {
                        new WarehouseType
                        {
                            WarehouseTypeID = _data.WarehouseType.WarehouseTypeID,
                            Name = _data.WarehouseType.Name
                        }
                   });

                if (_data.SiteID != null)
                {
                    cmdSite.SetAutoCompleteValue(new List<Site>
                        {
                           new Site
                            {
                                SiteID = _data.Site.SiteID,
                                SiteName = _data.Site.SiteName
                            }
                        }
                     );
                }

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
                Warehouse _warehouse = new Warehouse();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _warehouse.WarehouseID = new Guid(id);
                }

                _warehouse.Code = txtCode.Text;
                _warehouse.SiteID = new Guid(cmdSite.SelectedItem.Value);
                _warehouse.WarehouseTypeID = new Guid(cmdWarehouseType.SelectedItem.Value);
                _warehouse.Name = txtName.Text;
                _warehouse.ShortName = txtShortName.Text;
                _warehouse.ReferenceCode = cmbWhCode.SelectedItem.Value;
                _warehouse.Description = txtDescription.Text;
                _warehouse.IsActive = txtIsActive.Checked;
                _warehouse.Seqno = int.Parse(txtSeqno.Text);
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtWhCode.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.AddWarehouse(_warehouse).Result;

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
                    datasave = ClientService.Master.WarehouseClient.ModifyWarehouse(_warehouse).Result;
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
            if (txtCode.Text == "new")
            {
                //this.txtJobNo.Text = "new";
                txtDescription.Clear();
                txtName.Clear();
                txtShortName.Clear();
                btnSave.Hide();
                txtWhCode.Clear();
                txtLocation.Clear();
            }
            else
            {
                getDataWarehouse(txtCode.Text);
            }
        }
    }
}