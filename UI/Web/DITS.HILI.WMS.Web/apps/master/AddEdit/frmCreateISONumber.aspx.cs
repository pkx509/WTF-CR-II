using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmCreateISONumber : BaseUIPage
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
                    getDataISONumber(code);
                }
                else
                {
                    ISOId.Text = code;
                }

                //getWarehouseType();
                //getSite();
            }
        }

        private void getDataISONumber(string code)
        {
            try
            {
                Guid id = new Guid(code);
                ApiResponseMessage data = ClientService.Master.WarehouseClient.GetISONumberByID(id).Result;
                ISONumber _data = data.Get<ISONumber>();
                if (_data == null)
                {
                    return;
                }

                txtDocumentName.Text = _data.DocumentName;
                txtIso.Text = _data.ISO_Number;
                EffectiveDate.Text = _data.ISO_EffectiveDate;
                CheckReport.Value = _data.IsReport;
                CheckFrom.Value = _data.IsForm;
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
                ISONumber _ISONumber = new ISONumber();
                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _ISONumber.ISO_Id = new Guid(id);
                }

                _ISONumber.DocumentName = txtDocumentName.Text;
                _ISONumber.ISO_Number = txtIso.Text;
                _ISONumber.ISO_EffectiveDate = EffectiveDate.SelectedDate.ToString("dd/MM/yyyy", new CultureInfo("en-US"));
                _ISONumber.IsReport = CheckReport.Checked;
                _ISONumber.IsForm = CheckFrom.Checked;
                _ISONumber.IsActive = txtIsActive.Checked;

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (ISOId.Text == "new")
                {
                    datasave = ClientService.Master.WarehouseClient.AddIso(_ISONumber).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                }
                else
                {
                    datasave = ClientService.Master.WarehouseClient.ModifyIso(_ISONumber).Result;
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
            if (ISOId.Text == "new")
            {
                txtDocumentName.Clear();
                txtIso.Clear();
                EffectiveDate.Clear();
                btnSave.Hide();
            }
            else
            {
                getDataISONumber(ISOId.Text);
            }
        }
    }
}