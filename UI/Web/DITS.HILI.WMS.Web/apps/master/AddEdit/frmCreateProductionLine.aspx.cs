using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;


namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreateProductionLine : BaseUIPage
    {

        //private string AppDataService = "~/Common/DataClients/MsDataHandler.ashx";
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
                    getDataLine(code);
                }
                else
                {
                    txtCode.Text = code;
                }

                //getWarehouseType();
                //getSite();
            }
        }

        private void getDataLine(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.DailyPlan.ProductionLineClient.GetLineByID(id).Result;
                Line _data = data.Get<Line>();
                if (_data == null)
                {
                    return;
                }

                txtLine_Code.Text = _data.LineCode;
                txtBoiCard.Text = _data.BoiCard;

                cmbWarehouseName.SetAutoCompleteValue(new List<Warehouse> {
                        new Warehouse
                        {
                            WarehouseID = _data.WarehouseID.Value,
                            Name = _data.WarehouseName
                        }
                   });

                //this.cmbLineType.SetAutoCompleteValue(new List<Line> {
                //        new Line
                //        {
                //            LineType = _data.LineType
                //        }
                //   });


                cmbLineType.SelectedItem.Text = _data.LineType;

                SetButton("Edit");
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
                // MessageBoxExt.ShowError(ex);
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
            ApiResponseMessage Warehouse = WarehouseClient.GetZone(Guid.Empty, prms.Query, prms.Page, prms.Limit).Result;
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
                Line _line = new Line();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _line.LineID = new Guid(id);
                }

                _line.LineCode = txtLine_Code.Text;
                _line.BoiCard = txtBoiCard.Text;
                _line.LineType = cmbLineType.SelectedItem.Value;
                _line.WarehouseID = new Guid(cmbWarehouseName.SelectedItem.Value);
                _line.IsActive = chkIsActive.Checked;

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtCode.Text == "new")
                {
                    datasave = ClientService.DailyPlan.ProductionLineClient.Add(_line).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.DailyPlan.ProductionLineClient.Modify(_line).Result;
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
            if (txtCode.Text == "new")
            {
                txtBoiCard.Clear();
                txtLine_Code.Clear();
                btnSave.Hide();
                cmbLineType.Reset();
                cmbWarehouseName.Reset();
            }
            else
            {
                getDataLine(txtCode.Text);
            }
        }
    }
}