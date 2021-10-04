using DITS.HILI.WMS.ClientService.Master;
using DITS.WMS.Data.CustomModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace DITS.HILI.WMS.Web.apps.inbound.receive_WTF._usercontrol
{
    public partial class _ucProductforInternalRec : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(ProductSearchModel productSearch)
        {
            winProductSelect.Show();
            hdPONo.Text = productSearch.PONo;
            hfProductCode.Text = productSearch.ProductCode;
            hfProductCode.Focus(false, 100);
            hdFromReprocess.SetValue(productSearch.FromReprocess);
            hdToReprocess.SetValue(productSearch.ToReprocess);
            hdIsNormal.SetValue(productSearch.IsNormal);
            hdIsCreditNote.SetValue(productSearch.IsCreditNote);
            hdIsItemChange.SetValue(productSearch.IsItemChange);
            hdIsWithoutGoods.SetValue(productSearch.IsWithoutGoods);
            hdReferenceDispatchTypeID.SetValue(productSearch.ReferenceDispatchTypeID);
            PagingToolbar1.MoveFirst();
        }

        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            List<ProductCustomModel> data = new List<ProductCustomModel>();

            bool isCreditNote, isNormal, fromReprocess, toReprocess, isItemChange, isWithoutGoods;
            Guid? referenceDispatchTypeID = null;

            isCreditNote = isNormal = fromReprocess = toReprocess = false;

            try
            {
                isCreditNote = bool.Parse(hdIsCreditNote.Text);
                isNormal = bool.Parse(hdIsNormal.Text);
                fromReprocess = bool.Parse(hdFromReprocess.Text);
                toReprocess = bool.Parse(hdToReprocess.Text);
                isItemChange = bool.Parse(hdIsItemChange.Text);
                isWithoutGoods = bool.Parse(hdIsWithoutGoods.Text);

                if (Guid.TryParse(hdReferenceDispatchTypeID.Text, out Guid tmpGuid))
                {
                    referenceDispatchTypeID = tmpGuid;
                }
            }
            catch (Exception)
            {
                MessageBoxExt.Warning("Configuration Not found Please contact Administrator");
                return data;
            }

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            Core.Domain.ApiResponseMessage apiResp = ProductClient.GetProductForInternalRec(hdPONo.Text
                                            , hfProductCode.Text
                                            , hfProductName.Text
                                            , isCreditNote
                                            , isNormal
                                            , toReprocess
                                            , fromReprocess
                                            , isItemChange
                                            , isWithoutGoods
                                            , referenceDispatchTypeID
                                            , prms.Page
                                            , int.Parse(cmbPageList.SelectedItem.Value)).Result;

            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductCustomModel>>();
            }

            return new { data, total };
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["DataKey"];
            X.AddScript("App.direct.ucProduct_Select(" + _recordSelect + ");");
        }

        public void Close()
        {
            winProductSelect.Close();
        }
    }
}