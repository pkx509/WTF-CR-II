using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit._usercontrol
{
    public partial class ucProductMultiSelect : System.Web.UI.UserControl
    {
        //FormMasterList _FormMasterList = new FormMasterList();
        private static List<LogicalZoneGroupDetailModel> _DataExist;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(string _product_code, List<LogicalZoneGroupDetailModel> dataExist)
        {
            Session["dataExistPg"] = dataExist;
            _DataExist = new List<LogicalZoneGroupDetailModel>();
            _DataExist = dataExist;

            txtSearch.Text = string.Empty;
            winProductMultiSelect.Show();
            PagingToolbar1.MoveFirst();
        }
        public void ShowOnly()
        {
            winProductMultiSelect.Show();
        }
        public void Close()
        {
            winProductMultiSelect.Close();
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            X.AddScript("App.direct.ucProductCode_MultiSelect(" + gridJson + ");");
        }

        public object BindData(string action, Dictionary<string, object> extraParams)
        {

            if (txtSearch.Text == null)
            {
                txtSearch.Text = "";
            }
            //dataService.Add<string>("search", txtSearch.Text);
            //dataService.Add<List<string>>("dataExist", _DataExist.Select(x => x.Product_System_Code).ToList());

            //List<ProductModel> data = WebServiceHelper.Post<List<ProductModel>>("SYS_Get_Product_By_Limit", dataService.GetObject());

            //return new { data };
            int total = 0;
            Core.Domain.ApiResponseMessage apiResp = ProductClient.GetAll(txtSearch.Text, false, null, null, null, null, null, null).Result;
            List<ProductModel> data = new List<ProductModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>();
            }
            return new { data, total };
        }
    }
}