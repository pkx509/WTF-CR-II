using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllProductUOMTemplate : BaseUIPage
    {
        public static string ProgramCode = "P-0070";

        protected void Page_Load(object sender, EventArgs e)
        {

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }

        //protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        //{
        //    string oDataKeyId = e.ExtraParams["oDataKeyId"];
        //    if (string.IsNullOrEmpty(oDataKeyId))
        //        return;
        //    this.GetAddEditForm(oDataKeyId);
        //}

        private void GetAddEditForm(string oDataKeyId)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("UOMTEMPLATE");
            WindowShow.Show(this, strTitle, "CreateUOMT", "AddEdit/frmCreateTemplate_Product_UOM.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 680, 440);

        }


        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("new");
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }


        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = ProductTemplateUOMClient.GetProductTemplateUom(txtSearch.Text, ckbIsActive.Checked, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<ProductTemplateUom> data = new List<ProductTemplateUom>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductTemplateUom>>();
            }

            return new { data, total };
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
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
                Core.Domain.ApiResponseMessage ok = ClientService.Master.ProductTemplateUOMClient.Remove(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    //NotificationExt.Show("Delete", "Delete complete");
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
            }
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            try
            {
                PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.ToString());
            }
        }

        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void Reload()
        {
            NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            PagingToolbar1.MoveFirst();
        }



    }
}