using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllProduct : BaseUIPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("new");
        }

        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["DataKeyId"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }

            GetAddEditForm(oDataKeyId);
        }

        protected void gvdDataListCenter_CellClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["DataKeyId"];

            ProductModel gridData = JSON.Deserialize<ProductModel>(oDataKeyId);

            txtProductCode.Text = gridData.ProductCode;
            txtProductName.Text = gridData.ProductName;
            txtBrandName.Text = gridData.ProductBrandName;
            txtProGroupLevel3.Text = gridData.ProductGroupLevel3Name;
            txtProductSharp.Text = gridData.ProductShapeName;
        }

        private void GetAddEditForm(string oDataKeyId)
        {
            GridPanelExt.ClearSelection(grdDataList);
            FormPanel1.Reset();

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("PRODUCT");
            WindowShow.Show(this, strTitle, "CreateProduct", "AddEdit/frmCreateProduct.aspx?oDataKeyId=" + oDataKeyId, Icon.Add, 680, 440);
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            FormPanel1.Reset();
            PagingToolbar1.MoveFirst();
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId);
            }
            if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.Master.ProductClient.RemoveProduct(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                }
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = ProductClient.GetAll(txtSearch.Text, ckbIsActive.Checked, null, null, null, null, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<ProductModel> data = new List<ProductModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>();
            }

            return new { data, total };
        }

        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }

            PagingToolbar1.MoveFirst();
        }

    }
}