using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.outbound.dispatch._usercontrol
{
    public partial class ucTransferPalletMultiSelect : System.Web.UI.UserControl
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
                    g.ProductID,
                    g.MFGDate,
                    g.LineCode,
                    g.ProductStatusID,
                    g.LotNo
                }).ToList();


                if (groups.Count > 1)
                {
                    MessageBoxExt.Warning("Please Select same Lot Number");
                    return;
                }
                else
                {
                    X.AddScript("App.direct.ucPalletTag_MultiSelect(" + gridJson + ");");
                }
            }
            else
            {
                MessageBoxExt.Warning("Please Select some item");
                return;
            }

        }

        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            Dictionary<string, object> _filter = JSON.Deserialize<Dictionary<string, object>>(extraParams["filterheader"].ToString());

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

            if (!_filter.TryGetValue("ProductLot", out object Lot))
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

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetProductStockByCode(paCode, pCode, pNCode, lot, lCode, mfg, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<ProductModel> data = new List<ProductModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>().ToList();
            }

            return new { data, total };
        }

        public void Show(string _product_code, string _product_status_id, string _orderno, string refcode)
        {
            winProductSelect.Show();
            //this.txtSearch.Text = _product_code;
            hidProduct_Status_Code.Text = _product_status_id;
            hidOrderNo.Text = _orderno;
            hidRefCode.Text = refcode;
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