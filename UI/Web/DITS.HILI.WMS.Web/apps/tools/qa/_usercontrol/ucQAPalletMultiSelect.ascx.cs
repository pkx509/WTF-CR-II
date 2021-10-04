using DITS.HILI.WMS.ClientService;
using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa._usercontrol
{
    public partial class ucQAPalletMultiSelect : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void FilterHeader(string _product_code)
        {
            _product_code = _product_code.Replace(@"\", @"");
            X.AddScript("loadFilter(App.ucProductDispatchSelect_grdProductSelect.filterHeader,'" + _product_code + "');");
        }
        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["oDataKeyId"];
            X.AddScript("App.direct.ucProductCode_Select(" + _recordSelect + ");");

        }

        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingProductSelect.MoveFirst();
        }

        protected void btnConfirm_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStoreData"];

            List<PalletTagModel> pallets = JSON.Deserialize<List<PalletTagModel>>(gridJson);

            if (pallets.Count() > 0)
            {
                var groups = pallets.GroupBy(g => new
                {
                    g.MFGDate,
                    g.LineCode,
                    g.LotNo,
                    g.ProductID
                }).ToList();


                if (groups.Count > 1)
                {
                    MessageBoxExt.Warning(GetMessage("MSG00080").MessageValue);
                    // MessageBoxExt.Warning("Please Select same Lot Number");
                    return;
                }
                else
                {
                    X.AddScript("App.direct.ucPalletTag_MultiSelect(" + gridJson + ");");
                }
            }
            else
            {
                MessageBoxExt.Warning(GetMessage("MSG00081").MessageValue);
                //MessageBoxExt.Warning("Please Select some item");
                return;
            }

        } //        [DirectMethod(Timeout=180000)]
        public static CustomMessage GetMessage(string key)
        {
            CustomMessage defaultMsg = new CustomMessage { MessageCode = "SYS99999", MessageValue = "System error. Please contact your system administrator." };
            Core.Domain.ApiResponseMessage resp = WMSProperty.GetMessage(key).Result;
            if (resp.IsSuccess)
            {
                CustomMessage c = resp.Get<CustomMessage>();
                if (c == null)
                {
                    return defaultMsg;
                }
                return c;
            }
            return defaultMsg;
        }

        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            Dictionary<string, object> _filter = JSON.Deserialize<Dictionary<string, object>>(extraParams["filterheader"].ToString());
            //var apiResp = ProductClient.GetAll(this.txtSearch.Text, null, null, null, null, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;

            if (!_filter.TryGetValue("PalletCode", out object palletNo))
            {
                palletNo = "";
            }

            if (!_filter.TryGetValue("ProductCode", out object productCode))
            {
                productCode = "";
            }

            if (!_filter.TryGetValue("ProductName", out object productName))
            {
                productName = "";
            }

            if (!_filter.TryGetValue("LotNo", out object Lot))
            {
                Lot = "";
            }


            if (!_filter.TryGetValue("LineCode", out object Line))
            {
                Line = "";
            }

            if (!_filter.TryGetValue("MFGDate", out object mfgDate))
            {
                mfgDate = "";
            }

            FilterheaderModel _pallets = JSON.Deserialize<FilterheaderModel>(palletNo.ToString());
            FilterheaderModel _productCode = JSON.Deserialize<FilterheaderModel>(productCode.ToString());
            FilterheaderModel _productName = JSON.Deserialize<FilterheaderModel>(productName.ToString());
            FilterheaderModel _lot = JSON.Deserialize<FilterheaderModel>(Lot.ToString());
            FilterheaderModel _lineCode = JSON.Deserialize<FilterheaderModel>(Line.ToString());
            FilterheaderModel _mfgDate = JSON.Deserialize<FilterheaderModel>(mfgDate.ToString());

            string paCode = _pallets == null ? "" : _pallets.value;
            string pCode = _productCode == null ? "" : _productCode.value;
            string pNCode = _productName == null ? "" : _productName.value;
            string lot = _lot == null ? "" : _lot.value;
            string lCode = _lineCode == null ? "" : _lineCode.value;
            string mfg = _mfgDate == null ? "" : _mfgDate.value;


            string whRefCode = System.Configuration.ConfigurationManager.AppSettings["WHReferenceCode"];
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetPalletTag(paCode, pCode, pNCode, lot, lCode, mfg, new Guid(hidProduct_Status_Code.Text), prms.Page, int.Parse(cmbPageList.SelectedItem.Value), whRefCode).Result;
            List<PalletTagModel> data = new List<PalletTagModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<PalletTagModel>>().ToList();
            }
            return new { data, total };
        }


        public void Show(string _product_code, string _product_status_id)
        {
            winProductSelect.Show();
            //this.txtProductCode.Text = _product_code;
            hidProduct_Status_Code.Text = _product_status_id;
            PagingProductSelect.MoveFirst();
        }
        public void Close()
        {
            winProductSelect.Close();
        }

        public GridPanel GetGrid()
        {
            return grdProductSelect;
        }

        [DirectMethod(Timeout=180000)]
        public void ReloadData(object sender, DirectEventArgs e)
        {
            StoreProductSelect.Reload();
        }



    }

}