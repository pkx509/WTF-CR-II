using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Products;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.tranfer
{
    public partial class frmTransferWarehouseDetail : BaseUIPage
    {
        private readonly string AutoCompleteService = "../../../Common/DataClients/OptDataHandler.ashx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void bindData(string oDataKeyId)
        {
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(new Guid(oDataKeyId)).Result;
            List<TRMTransferMarketingProduct> listData = new List<TRMTransferMarketingProduct>();
            WindowDataDetail.Hidden = true;
            if (apiResp.IsSuccess)
            {
                TRMTransferMarketingProduct _data = apiResp.Get<TRMTransferMarketingProduct>();
                StoreOfDataList.DataSource = _data.TRMTransferMarketingProductDetail;
                StoreOfDataList.DataBind();

                hddTrmId.Text = _data.TRM_Product_ID.ToString();
                txtTrmCode.Text = _data.TRM_CODE;
                txtProductCode.Text = _data.ProductCode.ToString();
                txtProductName.Text = _data.ProductName.ToString();
                txtTotalTransferQty.Text = _data.TransferQty.ToString();

                hddUnitId.Text = _data.TransferUnitID.ToString();

                if (_data.PickStatus == (int)TranferMargetingStatus.Assign || _data.PickStatus == (int)TranferMargetingStatus.Confirm || _data.PickStatus == (int)TranferMargetingStatus.Approve)
                {
                    btnSave.Disable();
                }
            }
            else
            {

                if (apiResp.ResponseCode == "0")
                {
                    listData = apiResp.Get<List<TRMTransferMarketingProduct>>();
                }

                StoreOfDataList.DataSource = listData;
                StoreOfDataList.DataBind();
            }
        }

        protected void btnAddItem_Click(object sender, DirectEventArgs e)
        {
            WindowDataDetail.Show();
            PagingToolbar2.MoveFirst();
        }

        protected void btnConfirm_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStoreDetail"];
            List<TRMTransferMarketingProductDetail> gridCurrentData = JSON.Deserialize<List<TRMTransferMarketingProductDetail>>(e.ExtraParams["ParamStoreData"]);

            List<ProductModel> gridData = JSON.Deserialize<List<ProductModel>>(gridJson);
            List<TRMTransferMarketingProductDetail> _details = new List<TRMTransferMarketingProductDetail>();

            TRMTransferMarketingProduct dataModel = new TRMTransferMarketingProduct();
            Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(new Guid(hddTrmId.Text)).Result;

            if (apigetResp.IsSuccess)
            {
                TRMTransferMarketingProduct c = apigetResp.Get<TRMTransferMarketingProduct>();
                dataModel = c;
            }

            RowSelectionModel sm = GridGetWindows.GetSelectionModel() as RowSelectionModel;
            foreach (SelectedRow row in sm.SelectedRows)
            {
                ProductModel item = gridData[row.RowIndex];
                bool has = gridCurrentData.Any(x => x.ProductID == item.ProductID && x.ProductCode == item.ProductCode && x.Location == item.Location && x.PalletCode == item.PalletCode);
                if (has)
                {
                    X.MessageBox.Show(new MessageBoxConfig
                    {
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Warning",
                        Message = "An existing product [" + item.PalletCode + " ]",
                        Buttons = MessageBox.Button.OK
                    });

                    return;
                }

                if (item != null)
                {
                    _details.Add(new TRMTransferMarketingProductDetail()
                    {
                        PalletCode = item.PalletCode,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        LocationID = item.LocationID,
                        PalletUnitID = item.ProductUnitID,
                        PalletBaseUnitID = item.BaseUnitId,
                        Location = item.Location,
                        TransferQty = dataModel.TransferQty,
                        PalletQty = item.Quantity,
                        PalletBaseQty = item.BaseQuantity,
                        PickQty = 0,
                        ConfirmPickQty = 0,
                        ProductUnitName = item.ProductUnitName,
                        LotNo = item.ProductLot,
                        ProductID = item.ProductID,
                    });
                }
            }

            X.AddScript("App.GridGetWindows.getSelectionModel().deselectAll();");
            IEnumerable<TRMTransferMarketingProductDetail> result = gridCurrentData.Concat(_details);
            if (_details.Count() > 0)
            {
                btnSave.Show();
            }

            btnSave.Disabled = false;

            StoreOfDataList.DataSource = result;
            StoreOfDataList.DataBind();
            WindowDataDetail.Hide();
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oDataKeyDetailId = e.ExtraParams["oDataKeyDetailId"];

            Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(oDataKeyId == "" || oDataKeyId == "null" ? Guid.Empty : new Guid(oDataKeyId)).Result;
            TRMTransferMarketingProduct data = new TRMTransferMarketingProduct();

            if (apigetResp.IsSuccess)
            {
                data = apigetResp.Get<TRMTransferMarketingProduct>();
            }
            if (data == null) data = new TRMTransferMarketingProduct();

            bool yes = data.TRMTransferMarketingProductDetail.Any(x => x.IsActive == true && x.TRM_Product_Detail_ID == new Guid(oDataKeyDetailId));
            if (!yes)
            {
                X.Call("deleteProduct('" + oDataKeyId + "')");
            }
            else
            {
                Core.Domain.ApiResponseMessage ok = InventoryToolsClient.RemoveTransferProductDetail(oDataKeyDetailId).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    bindData(hddTrmId.Text);
                }
                else
                {
                    NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                }
            }
        }

        [DirectMethod(Timeout=180000)]
        public object ValidateProdcut(string product_code, string product_sys_code)
        {
            return new { valid = true, msg = "" };
        }

        [DirectMethod(Timeout=180000)]
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
            string pCode = _productCode == null ? txtProductCode.Text : _productCode.value;
            string pNCode = _productName == null ? "" : _productName.value;
            string lot = _lot == null ? "" : _lot.value;
            string lCode = _lineCode == null ? "" : _lineCode.value;
            string mfg = _mfgDate == null ? "" : _mfgDate.value;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetProductStockByCode(paCode, pCode, pNCode, lot, lCode, mfg, prms.Page, int.Parse(cmbPageList2.SelectedItem.Value)).Result;
            List<ProductModel> data = new List<ProductModel>();
            if (apiResp.ResponseCode == "0")
            {
                //total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>().ToList();
                data = data.Where(x => x.ProductUnitID == new Guid(hddUnitId.Text)).ToList();
                total = data.Count();
            }

            return new { data, total };
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];

            TRMTransferMarketingProduct TransferProductDataModel = new TRMTransferMarketingProduct
            {
                TRMTransferMarketingProductDetail = JSON.Deserialize<List<TRMTransferMarketingProductDetail>>(gridJson),

                TRM_Product_ID = new Guid(hddTrmId.Text)
            };
            decimal _transferQty = Convert.ToDecimal(txtTotalTransferQty.Text);
            decimal _pickQty = TransferProductDataModel.TRMTransferMarketingProductDetail.ToList().Sum(x => x.PickQty.Value);

            if (_pickQty > _transferQty)
            {
                MessageBoxExt.Warning("Please Check Transfer Quantity Over!");
                return;
            }

            if (_pickQty <= 0)
            {
                MessageBoxExt.Warning("Please Check Quantity > 0");
                return;
            }

            Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(new Guid(hddTrmId.Text)).Result;
            List<TRMTransferMarketingProduct> listData = new List<TRMTransferMarketingProduct>();


            int dCount = 0;
            if (apigetResp.IsSuccess)
            {
                TRMTransferMarketingProduct _data = apigetResp.Get<TRMTransferMarketingProduct>();
                dCount = _data.TRMTransferMarketingProductDetail.Count();
            }

            if (dCount == 0)
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.AddTransferByPallet(TransferProductDataModel).Result;

                if (apiResp.IsSuccess)
                {
                    X.Call("parent.App.direct.Reload", apiResp.ResponseMessage);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    X.Call("parent.App.direct.bindData", hddTrmId.Text);
                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }
            else
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.ModifyTransferByPallet(TransferProductDataModel).Result;

                if (apiResp.IsSuccess)
                {
                    X.Call("parent.App.direct.Reload", apiResp.ResponseMessage);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }
        }
    }
}