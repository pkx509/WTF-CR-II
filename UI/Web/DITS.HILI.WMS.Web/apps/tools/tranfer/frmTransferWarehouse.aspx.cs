using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.tools.tranfer
{
    public partial class frmTransferWarehouse : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindData(Request.QueryString["oDataKeyId"]);
            }

            TransferDate.SelectedDate = DateTime.Now;
        }
        [DirectMethod(Timeout=180000)]
        private void bindData(string oDataKeyId)
        {
            if (oDataKeyId == "new")
            {
                hidTrmID.Text = oDataKeyId;
                txtTrmCode.Text = oDataKeyId;
                txtTransferStatus.Text = TranferMargetingStatus.New.Description();
                btnApprove.Visible = false;
                btnAssign.Visible = false;
                dtApproveDate.ReadOnly = true;
            }
            else
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetTransferMargetingDetail(new Guid(oDataKeyId)).Result;
                List<TRMTransferMarketing> listData = new List<TRMTransferMarketing>();

                if (apiResp.IsSuccess)
                {
                    TRMTransferMarketing _data = apiResp.Get<TRMTransferMarketing>();
                    StoreOfDataList.DataSource = _data.TRMTransferMarketingProduct;
                    StoreOfDataList.DataBind();

                    Guid? TRMProductID = Guid.Empty;
                    if (_data.TRMTransferMarketingProduct.Count() > 0)
                    {
                        TRMProductID = _data.TRMTransferMarketingProduct.FirstOrDefault().TRM_Product_ID;
                    }

                    hidTrmID.Text = oDataKeyId;
                    hddTrmId.Text = _data.TRM_ID.ToString();
                    txtTrmCode.Text = _data.TRM_CODE;
                    txtTransferStatus.Text = CheckStatus(_data.TransferStatus.Value);
                    dtApproveDate.Text = _data.ApproveDate == null ? "" : _data.ApproveDate.Value.ToString("dd/MM/yyyy");
                    TransferDate.Text = _data.TransferDate == null ? "" : _data.TransferDate.ToString("dd/MM/yyyy");
                    txtDesc.Text = _data.Description;

                    Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(TRMProductID).Result;
                    int dCount = 0;

                    if (apigetResp.IsSuccess)
                    {
                        TRMTransferMarketingProduct c = apigetResp.Get<TRMTransferMarketingProduct>();
                        dCount = c == null ? 0 : c.TRMTransferMarketingProductDetail.Count();
                    }

                    if (dCount == 0 && _data.TransferStatus == (int)TranferMargetingStatus.New)
                    {

                        dtApproveDate.ReadOnly = true;
                        btnApprove.Disable();
                        btnAssign.Disable();
                        btnSave.Enable();
                        colDel.Enable();
                        colEdit.Enable();
                    }
                    if (dCount > 0 && _data.TransferStatus == (int)TranferMargetingStatus.New)
                    {
                        dtApproveDate.ReadOnly = true;
                        btnApprove.Disable();
                        btnAssign.Enable();
                        btnSave.Enable();
                    }
                    if (dCount > 0 && _data.TransferStatus == (int)TranferMargetingStatus.Assign)
                    {
                        dtApproveDate.ReadOnly = false;
                        btnApprove.Disable();
                        btnAssign.Disable();
                        btnSave.Disable();
                    }
                    if (dCount > 0 && _data.TransferStatus == (int)TranferMargetingStatus.Confirm)
                    {
                        dtApproveDate.ReadOnly = false;
                        btnApprove.Enable();
                        btnAssign.Disable();
                        btnSave.Disable();
                    }
                    if (dCount > 0 && _data.TransferStatus == (int)TranferMargetingStatus.Approve)
                    {
                        dtApproveDate.ReadOnly = false;
                        btnApprove.Disable();
                        btnAssign.Disable();
                        btnSave.Disable();
                    }
                    if (dCount > 0 && _data.TransferStatus == (int)TranferMargetingStatus.Cancel)
                    {
                        dtApproveDate.ReadOnly = false;
                        btnApprove.Disable();
                        btnAssign.Disable();
                        btnSave.Disable();
                    }
                    if (dCount == 0 && _data.TransferStatus == (int)TranferMargetingStatus.Cancel)
                    {
                        dtApproveDate.ReadOnly = false;
                        btnApprove.Disable();
                        btnAssign.Disable();
                        btnSave.Disable();
                    }
                }
                else
                {

                    if (apiResp.ResponseCode == "0")
                    {
                        listData = apiResp.Get<List<TRMTransferMarketing>>();
                    }

                    StoreOfDataList.DataSource = listData;
                    StoreOfDataList.DataBind();
                }
            }
        }
        private string CheckStatus(int status)
        {
            string statusName = string.Empty;
            switch (status)
            {
                case 10:
                    statusName = TranferMargetingStatus.New.ToString();
                    break;
                case 20:
                    statusName = TranferMargetingStatus.Assign.ToString();
                    break;
                case 30:
                    statusName = TranferMargetingStatus.Confirm.ToString();
                    break;
                case 100:
                    statusName = TranferMargetingStatus.Approve.ToString();
                    break;
                case 102:
                    statusName = TranferMargetingStatus.Cancel.ToString();
                    break;
                default:
                    break;
            }

            return statusName;
        }

        [DirectMethod(Timeout=180000)]
        public void GetProduct(string _product_code)
        {

            if (_product_code == "")
            {
                ucTransferProductSelect.Show(_product_code, hidProduct_Status_Code.Text, "", "111");
                return;
            }


            int total = 0;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetProductStockByCode("", _product_code, "", "", "", "111", 0, 20).Result;
            if (apiResp.ResponseCode == "0")
            {
                List<ProductModel> data = new List<ProductModel>();

                total = apiResp.Totals;
                data = apiResp.Get<List<ProductModel>>();

                if (data.Count == 1)
                {
                    hidAddProductOwnerId.Text = data.FirstOrDefault().ProductOwnerId.ToString();
                    txtProduct_System_Code.Text = data.FirstOrDefault().ProductCode;
                    hidAddProduct_System_Code.Text = data.FirstOrDefault().ProductID.ToString();
                    hidAddProduct_Code.Text = data.FirstOrDefault().ProductCode;
                    txtAddProduct_Name_Full.Text = data.FirstOrDefault().ProductName;
                    hidAddUomID.Text = data.FirstOrDefault().ProductUnitID.ToString();
                    hidAddUomName.Text = data.FirstOrDefault().ProductUnitName;
                    if (data.FirstOrDefault().PriceUnitId != Guid.Empty)
                    {
                        hidAddPriceUnitId.Text = data.FirstOrDefault().PriceUnitId.ToString();
                        hidAddPriceUnitName.Text = data.FirstOrDefault().PriceUnitName.ToString();
                    }
                    hidAddPrice.Text = data.FirstOrDefault().Price.ToString();
                    txtUOM.Text = hidAddUomName.Text;
                    hidAddUomSKU.Text = data.FirstOrDefault().BaseUnitId.ToString();
                    hidAddUomHeight.Text = data.FirstOrDefault().ProductHeight.ToString();
                    hidAddUomLength.Text = data.FirstOrDefault().ProductLength.ToString();
                    hidAddUomWidth.Text = data.FirstOrDefault().ProductWidth.ToString();
                    hidAddUomQty.Text = data.FirstOrDefault().ConversionQty.ToString();
                    hidAddBaseQty.Text = data.FirstOrDefault().BaseQuantity.ToString();
                    hidAddWeight.Text = data.FirstOrDefault().ProductWeight.ToString();
                    hidAddPackWeight.Text = data.FirstOrDefault().PackageWeight.ToString();
                    txtAddQty.Text = data.FirstOrDefault().Quantity.ToString();
                    // this.txtAddQty.MaxValue = (double)data.FirstOrDefault().Quantity;
                    txtAddQty.Focus(true, 100);

                    //if (!string.IsNullOrEmpty(cmbShippingTo.GetValue()))
                    //{
                    //    var apiRespShipto = ShipToClient.GetByID(new Guid(this.cmbShippingTo.SelectedItem.Value)).Result;
                    //    var datashipto = new Shipto();
                    //    if (apiRespShipto.ResponseCode == "0")
                    //    {
                    //        datashipto = apiRespShipto.Get<Shipto>();

                    //    }

                    //    if (datashipto.SpecialBookingRule != null)
                    //    {
                    //        hidSpecial_Rul_Name.Value = datashipto.SpecialBookingRule.RuleName;
                    //        hidSpecial_Rul_ID.Value = datashipto.SpecialBookingRule.RuleId;
                    //    }
                    //}
                }
                else
                {

                    ucTransferProductSelect.Show(_product_code, "", "", "111");
                }

            }


        }


        [DirectMethod(Timeout=180000)]
        public void ucPalletTag_MultiSelect(string record)
        {
            List<TRMTransferMarketingProductDetail> pallets = JSON.Deserialize<List<TRMTransferMarketingProductDetail>>(record);

            List<TRMTransferMarketingProductDetail> listData = new List<TRMTransferMarketingProductDetail>();

            foreach (TRMTransferMarketingProductDetail pallet in pallets)
            {
                listData.Add(new TRMTransferMarketingProductDetail()
                {

                    PalletCode = pallet.PalletCode,
                    ProductName = pallet.ProductName,
                    PalletQty = pallet.PickQty,
                    PickQty = pallet.PickQty,
                    ProductUnitName = pallet.ProductUnitName,
                    ProductCode = pallet.ProductCode,
                    LotNo = pallet.LotNo,
                    //MFGDate = pallet.MFGDate,
                    //LineCode = pallet.LineCode, 
                    ProductStatusID = pallet.ProductStatusID,
                    ProductID = pallet.ProductID,
                    //ReclassifiedDetailID = Guid.NewGuid()
                });
            }

            StoreOfDataList.Add(listData);
            StoreOfDataList.CommitChanges();

            ucTransferProductSelect.Close();
        }

        [DirectMethod(Timeout=180000)]
        public void ucProductCode_Select(string record)
        {
            ProductModel data = JSON.Deserialize<ProductModel>(record);

            hidAddProductOwnerId.Text = data.ProductOwnerId.ToString();
            txtProduct_System_Code.Text = data.ProductCode;
            hidAddProduct_System_Code.Text = data.ProductID.ToString();
            hidAddProduct_Code.Text = data.ProductCode;
            txtAddProduct_Name_Full.Text = data.ProductName;
            hidAddUomID.Text = data.ProductUnitID.ToString();
            hidAddUomName.Text = data.ProductUnitName;
            hidAddProductStatusId.Text = data.ProductStatusId.ToString();
            if (data.PriceUnitName != null)
            {
                hidAddPriceUnitId.Text = data.PriceUnitId.ToString();
                hidAddPriceUnitName.Text = data.PriceUnitName.ToString();
                hidAddPrice.Text = data.Price.ToString();
            }

            txtUOM.Text = hidAddUomName.Text;
            hidAddUomSKU.Text = data.BaseUnitId.ToString();
            hidAddUomHeight.Text = data.ProductHeight.ToString();
            hidAddUomLength.Text = data.ProductLength.ToString();
            hidAddUomWidth.Text = data.ProductWidth.ToString();
            hidAddUomQty.Text = data.ConversionQty.ToString();
            hidAddBaseQty.Text = data.BaseQuantity.ToString();
            hidAddWeight.Text = data.ProductWeight.ToString();
            hidAddPackWeight.Text = data.PackageWeight.ToString();
            txtAddQty.Text = data.Quantity.ToString();

            ucTransferProductSelect.Close();

            txtAddQty.Focus(true, 300);
        }
        [DirectMethod(Timeout=180000)]
        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string TransferUnitID = e.ExtraParams["uId"];

            if (command == "Delete")
            {
                Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(oDataKeyId == "" ? Guid.Empty : new Guid(oDataKeyId)).Result;
                TRMTransferMarketingProduct data = new TRMTransferMarketingProduct();

                if (apigetResp.IsSuccess)
                {
                    data = apigetResp.Get<TRMTransferMarketingProduct>();
                }
                if (oDataKeyId == "")
                {
                    X.Call("deleteProduct2('" + oDataKeyId + "','" + TransferUnitID + "')");
                }
                if (!apigetResp.IsSuccess)
                {
                    X.Call("deleteProduct('" + oDataKeyId + "')");
                }
                else
                {
                    Core.Domain.ApiResponseMessage ok = InventoryToolsClient.RemoveTransferProduct(oDataKeyId).Result;
                    if (ok.ResponseCode == "0")
                    {
                        NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                        bindData(hidTrmID.Text);
                    }
                    else
                    {
                        NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                    }
                }
            }
            else
            {
                GetAddEditForm(oDataKeyId);
            }
        }


        private void GetAddEditForm(string oDataKeyId)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("TRANSFERDETAL");
            WindowShow.ShowNewPage(this, strTitle, "ProductDetail", "frmTransferWarehouseDetail.aspx?oDataKeyId=" + oDataKeyId, iconWindows);
        }

        [DirectMethod(Timeout=180000)]
        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucTransferProductSelect.ProductSelectBindData(action, extraParams);
        }

        [DirectMethod(Timeout=180000)]
        public object ValidateProdcut(string product_code, string product_sys_code)
        {
            return new { valid = true, msg = "" };
        }
        [DirectMethod(Timeout=180000)]
        protected void btnTransferlist_Click(object sender, DirectEventArgs e)
        {

            X.Call("popitup", $"../../Report/frmReportViewer.aspx?reportName=RPT_TransferMaketingForm&Silent={false}&TrmCode={txtTrmCode.Text}");
        }
        [DirectMethod(Timeout=180000)]
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            if (TransferDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("Please Select Transfer Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('TransferDate').focus('', 10); ");
                return;
            }

            string gridJson = e.ExtraParams["ParamStorePages"];
            TRMTransferMarketing TransferDataModel = new TRMTransferMarketing
            {
                TRMTransferMarketingProduct = JSON.Deserialize<List<TRMTransferMarketingProduct>>(gridJson),

                TransferDate = TransferDate.SelectedDate,
                ApproveDate = dtApproveDate.SelectedDate,
                Description = txtDesc.Text
            };

            if (TransferDataModel.TRMTransferMarketingProduct.Count() == 0)
            {
                MessageBoxExt.Warning("กรุณาเลือกสินค้า");
                return;
            }

            if (txtTrmCode.Text == "new")
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.AddTransfer(TransferDataModel).Result;

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
            else
            {
                TRMTransferMarketing TransferModel = new TRMTransferMarketing
                {
                    TRM_ID = new Guid(hddTrmId.Text),
                    TRM_CODE = hddTrmId.Text,
                    ApproveDate = dtApproveDate.SelectedDate,
                    Description = txtDesc.Text
                };
                TransferModel.TRMTransferMarketingProduct = TransferDataModel.TRMTransferMarketingProduct;

                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.ModifyByProduct(TransferModel).Result;

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
        [DirectMethod(Timeout=180000)]
        protected void btnAssign_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<TRMTransferMarketingProduct> TransferDataModel = JSON.Deserialize<List<TRMTransferMarketingProduct>>(gridJson);
            if (!ValidateBasic(TransferDataModel))
            {
                return;
            }

            Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(TransferDataModel.FirstOrDefault().TRM_Product_ID).Result;
            List<TRMTransferMarketingProduct> listData = new List<TRMTransferMarketingProduct>();
            int dCount = 0;

            if (apigetResp.IsSuccess)
            {
                TRMTransferMarketingProduct _data = apigetResp.Get<TRMTransferMarketingProduct>();
#pragma warning disable S2971 // "IEnumerable" LINQs should be simplified
                dCount = _data.TRMTransferMarketingProductDetail.Count();
#pragma warning restore S2971 // "IEnumerable" LINQs should be simplified
            }

            if (dCount > 0)
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.OnAssignPick(TransferDataModel).Result;
                if (apiResp.IsSuccess)
                {

                    List<TRMTransferMarketingProduct> dataItem = new List<TRMTransferMarketingProduct>();
                    dataItem = apiResp.Get<List<TRMTransferMarketingProduct>>();
                    if (dataItem.Count() > 0)
                    {
                        string _strmsg = "";
                        _strmsg = "<center><font color='red'>สินค้าไม่พอสำหรับ Transfer</font><br>";
                        dataItem.ForEach(item =>
                        {
                            _strmsg += "สินค้า Code:" + item.ProductCode;
                            _strmsg += " Transfer ได้: " + item.RemainQTY.ToString("0.00") + " " + item.PriceUnitName;
                            _strmsg += " <font color='red'>เกิน: " + item.OverQTY.ToString("0.00") + " " + item.PriceUnitName + " </font><br>";

                        });

                        X.Msg.Confirm("เตือน", _strmsg, new MessageBoxButtonsConfig
                        {
                            No = new MessageBoxButtonConfig
                            {
                                Handler = "App.direct.DoNo()",
                                Text = "ยกเลิก"
                            }
                        }).Show();
                    }
                    else
                    {

                        X.Call("parent.App.direct.Reload", apiResp.ResponseMessage);
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");

                    }

                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }
        }

        [DirectMethod(Timeout=180000)]
        public void DoNo()
        {
            X.Msg.Hide();
        }

        [DirectMethod(Timeout=180000)]
        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {
            if (dtApproveDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("Please Select Approve Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDate').focus('', 10); ");
                return;
            }
            Core.Domain.ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(dtApproveDate.SelectedDate).Result;
            if (apiresponse.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(apiresponse.ResponseMessage);
                return;
            }

            string gridJson = e.ExtraParams["ParamStorePages"];
            List<TRMTransferMarketingProduct> TransferDataModel = new List<TRMTransferMarketingProduct>();
            List<TRMTransferMarketingProduct> DatalistModel = new List<TRMTransferMarketingProduct>();

            TransferDataModel = JSON.Deserialize<List<TRMTransferMarketingProduct>>(gridJson);

            foreach (TRMTransferMarketingProduct item in TransferDataModel)
            {
                item.ApproveDate = dtApproveDate.SelectedDate;
                DatalistModel.Add(item);
            }

            Core.Domain.ApiResponseMessage apigetResp = InventoryToolsClient.GetTransferMargetingDetailByPallet(TransferDataModel.FirstOrDefault().TRM_Product_ID).Result;
            List<TRMTransferMarketingProduct> listData = new List<TRMTransferMarketingProduct>();
            int dCount = 0;

            if (apigetResp.IsSuccess)
            {
                TRMTransferMarketingProduct _data = apigetResp.Get<TRMTransferMarketingProduct>();
                dCount = _data.TRMTransferMarketingProductDetail.Count();
            }

            if (dCount > 0)
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.OnApprove(DatalistModel).Result;

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

        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }

            bindData(hddTrmId.Text);
        }

        protected void btnExit_Click(object sender, DirectEventArgs e)
        {
            X.Call("parent.App.direct.Reload", "");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }
        protected bool ValidateBasic(List<TRMTransferMarketingProduct> TransferDataModel)
        {
            string _strmsg = string.Empty;
            foreach (TRMTransferMarketingProduct checkdata in TransferDataModel)
            {
                _strmsg = "สินค้า : " + checkdata.ProductName + " " + "Pick Qty = 0";
                if (checkdata.PickQty == 0)
                {

                    MessageBoxExt.ShowError(_strmsg);
                    return false;
                }
            }

            return true;
        }
    }
}