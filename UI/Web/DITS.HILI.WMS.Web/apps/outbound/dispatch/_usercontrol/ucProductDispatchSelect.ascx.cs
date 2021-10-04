using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa.usercontrol
{
    public partial class ucProductDispatchSelect : System.Web.UI.UserControl
    {
        private readonly string AutoCompleteService = "../../../Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void FilterHeader(string _product_code)
        {
            _product_code = _product_code.Replace(@"\", @"");
            X.AddScript("loadFilter(App.ucProductDispatchSelect_grdProductSelect.filterHeader,'" + _product_code + "');");
        }
        //protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        //{
        //    var _recordSelect = e.ExtraParams["oDataKeyId"];
        //    X.AddScript("App.direct.ucProductCode_Select(" + _recordSelect + ");");

        //}
        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["oDataKeyId"];
            X.AddScript("App.direct.ucProductCode_Select(" + _recordSelect + ");");

        }

        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingProductSelect.MoveFirst();
        }



        //public GridHelper<ProductModel> GetGridFileter(string action, Dictionary<string, object> extraParams)
        //{
        //    GridHelper<ProductModel> grd = new GridHelper<ProductModel>(this.grdProductSelect, extraParams);
        //    return grd;

        //}

        //public bool GetValueCheckBox()
        //{
        //    return this.chkfilter1.Checked;
        //}

        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            //var apiResp = ProductClient.GetAll(this.txtSearch.Text, null, null, null, null, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            Core.Domain.ApiResponseMessage apiResp = (ckbIsStock.Checked ? 
                DispatchClient.GetProductStock(txtSearch.Text, hidOrderNo.Text, new Guid(hidProduct_Status_Code.Text), hidRefCode.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result : 
                DispatchClient.GetProductNoneStock(txtSearch.Text, new Guid(hidProduct_Status_Code.Text), prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result);
            List<ProductModel> data = new List<ProductModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>().Where(x => x.ProductUnitName != "").ToList();
            }

            colQuantity.Hidden = !ckbIsStock.Checked;
            colOrderType.Hidden = !ckbIsStock.Checked;
            colOrderNo.Hidden = !ckbIsStock.Checked;

            return new { data, total };
        }


        public void Show(string _product_code, string _product_status_id, string _orderno, string refcode)
        {
            winProductSelect.Show();
            txtSearch.Text = _product_code;
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