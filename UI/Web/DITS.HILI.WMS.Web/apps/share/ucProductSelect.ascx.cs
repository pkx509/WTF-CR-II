using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
//using DITS.WMS.Web.Common.Interfaces;
using HILI.WEB.Commons.Helper;
using System;
using System.Collections.Generic;


namespace DITS.HILI.WMS.Web.apps.share
{
    public partial class ucProductSelect : System.Web.UI.UserControl
    {

        //FormMasterList _FormMasterList = new FormMasterList();

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void FilterHeader(string _product_code)
        {
            _product_code = _product_code.Replace(@"\", @"");
            X.AddScript("loadFilter(App.ucProductSelect_grdProductSelect.filterHeader,'" + _product_code + "');");
        }
        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["oDataKeyId"];
            X.AddScript("App.direct.ucProductCode_Select(" + _recordSelect + ");");

        }

        public GridHelper<ProductModel> GetGridFileter(string action, Dictionary<string, object> extraParams)
        {
            GridHelper<ProductModel> grd = new GridHelper<ProductModel>(grdProductSelect, extraParams);
            return grd;

        }

        public bool GetValueCheckBox()
        {
            return chkfilter1.Checked;
        }

        //public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        //{
        //DataServiceModel dataService = new DataServiceModel();
        //List<SearchModel> _searchModel1 = new List<SearchModel>();

        //GridHelper<ProductModel> grd = new GridHelper<ProductModel>(this.grdProductSelect, extraParams);
        //var _filter = grd.FilterModel;


        //string _product_code = string.Empty;
        //string _product_name = string.Empty;
        //string _brand_name = string.Empty;
        //string _model = string.Empty;
        //string _lot = string.Empty;
        //string _location = string.Empty;
        //foreach (var item in _filter)
        //{
        //    switch (item.DataIndex)
        //    {
        //case "Product_Code":
        //   // _product_code = item.Value;
        //    _searchModel1.Add(new SearchModel()
        //    {
        //        Group = 1,
        //        Key = "sys_product_code.Product_Code",
        //        Operation = "LIKE",
        //        Value = item.Value,
        //        IsCondition = ConditionWhere.AND,
        //    });
        //    dataService.Add<List<SearchModel>>("SearchModel", _searchModel1);
        //    break;
        //case "Product_Name_Full":
        //   // _product_name = item.Value;
        //    _searchModel1.Add(new SearchModel()
        //    {
        //        Group = 1,
        //        Key = "sys_product.Product_Name_Full",
        //        Operation = "LIKE",
        //        Value = item.Value,
        //        IsCondition = ConditionWhere.AND,
        //    });
        //    dataService.Add<List<SearchModel>>("SearchModel", _searchModel1);
        //    break;
        //case "Product_Brand_Name":
        //    //_brand_name = item.Value;
        //    _searchModel1.Add(new SearchModel()
        //    {
        //        Group = 1,
        //        Key = "sys_product_brand.Product_Brand_Name",
        //        Operation = "LIKE",
        //        Value = item.Value,
        //        IsCondition = ConditionWhere.AND,
        //    });
        //    dataService.Add<List<SearchModel>>("SearchModel", _searchModel1);
        //    break;
        //case "Product_Model":
        //   // _model = item.Value;
        //    _searchModel1.Add(new SearchModel()
        //    {
        //        Group = 1,
        //        Key = "sys_product.Product_Model",
        //        Operation = "LIKE",
        //        Value = item.Value,
        //        IsCondition = ConditionWhere.AND,
        //    });
        //    dataService.Add<List<SearchModel>>("SearchModel", _searchModel1);
        //    break;
        //    }
        //}

        //if (this.chkfilter1.Checked)
        //{
        //return _FormMasterList.BindData<ProductModel>(action, extraParams,
        //   "", this.grdProductSelect, "SYS_Get_Product_Receive",
        //       _searchModel1, true, 2);
        //}
        //else {
        //return _FormMasterList.BindData<ProductModel>(action, extraParams,
        //   "", this.grdProductSelect, "SYS_Get_Product_Dispatch",
        //       _searchModel1, true, 2);
        //}


        //return new { data, total };
        //}


        public void Passvalue(string process)
        {
            if (process == "all")
            {
                chkfilter1.Checked = true;
                colReserveBalanceQty.Hide();
                colUOM_Name.Hide();
            }
            else if (process == "balance")
            {
                chkfilter1.Checked = false;
                colReserveBalanceQty.Show();
                colUOM_Name.Show();
            }
            ////hide,show colums
            //if (process == "receive" && Recevice_Code == "new")
            //{
            //    this.colProduct_Brand_Name.Show();
            //    this.colProduct_Model.Show();
            //    this.colProduct_Quantity.Hide();
            //    this.colUOM_Name.Hide();
            //}
            //else if (process == "receive" && Recevice_Code != "new")
            //{
            //    this.colProduct_Brand_Name.Show();
            //    this.colProduct_Model.Show();
            //    this.colProduct_Quantity.Show();
            //    this.colUOM_Name.Show();
            //}
            //else if (process == "buildpallet")
            //{
            //    this.colProduct_Brand_Name.Hide();
            //    this.colProduct_Model.Hide();
            //    this.colProduct_Quantity.Show();
            //    this.colUOM_Name.Show();
            //}
            //else
            //{
            //    this.colProduct_Brand_Name.Show();
            //    this.colProduct_Model.Show();
            //    this.colProduct_Quantity.Hide();
            //    this.colUOM_Name.Hide();
            //}
        }

        public void Show(string _product_code)
        {

            winProductSelect.Show();
            FilterHeader(_product_code);
            headerFilterProduct.Focus(false, 100);
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