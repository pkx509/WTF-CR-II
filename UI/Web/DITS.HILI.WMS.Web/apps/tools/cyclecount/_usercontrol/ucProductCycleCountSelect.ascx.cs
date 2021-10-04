using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.tools.cyclecount._usercontrol
{
    public partial class ucProductCycleCountSelect : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void FilterHeader(string _product_code)
        {
            _product_code = _product_code.Replace(@"\", @"");
            X.AddScript("loadFilter(App.ucProductCycleCountnSelect_grdProductSelect.filterHeader,'" + _product_code + "');");
        }
        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["oDataKeyId"];
            X.AddScript("App.direct.ucProductCode_Select(" + _recordSelect + ");");

        }

        //public GridHelper<ProductModel> GetGridFileter(string action, Dictionary<string, object> extraParams)
        //{
        //    GridHelper<ProductModel> grd = new GridHelper<ProductModel>(this.grdProductSelect, extraParams);
        //    return grd;

        //}

        public object ProductSelectBindData(object data, int total)
        {
            return new { data, total };
        }
        public void Show()
        {

            winProductSelect.Show();
            FilterHeader("");
            headerFilterProduct.Focus(false, 100);
        }
        public void Close()
        {
            winProductSelect.Close();
        }

        [DirectMethod(Timeout=180000)]
        public void ReloadData(object sender, DirectEventArgs e)
        {
            StoreProductSelect.Reload();
        }
    }
}