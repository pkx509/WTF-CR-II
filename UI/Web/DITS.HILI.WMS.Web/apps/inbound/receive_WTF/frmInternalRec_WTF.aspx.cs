using DITS.HILI.WMS.ClientService.Inbound;
using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.ReceiveModel;
using DITS.WMS.Data.CustomModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.inbound.receive_WTF
{
    public partial class frmInternalRec_WTF : BaseUIPage
    {
        [DirectMethod(Timeout=180000)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData(Request.QueryString["receiveID"]);
            X.Js.Call("sumTotal");
        }
        [DirectMethod(Timeout=180000)]
        private void BindData(string receiveID)
        {

            if (!string.IsNullOrWhiteSpace(receiveID))
            {
                if (!Guid.TryParse(receiveID, out Guid _ReceiveGUID))
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00005"));
                    return;
                }

                hdReceiveID.SetValue(receiveID);

                Core.Domain.ApiResponseMessage apiResp = ReceiveClient.GetReceiveByID(_ReceiveGUID).Result;

                if (apiResp.IsSuccess)
                {
                    ReceiveHeaderModel data = apiResp.Get<ReceiveHeaderModel>();

                    if (data == null)
                    {
                        MessageBoxExt.ShowError(GetMessage("MSG00006"));
                        return;
                    }

                    BindtoControl(data);
                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }
            else
            {
                txtReceiveCode.Text = "New";
                btnConfirm.Disabled = true;
                dtEstReceiveDate.SelectedDate = DateTime.Now;
                txtReceiveStatus.Text = GetResource(ReceiveStatusEnum.New.ToString());
                hdReceiveStatus.Text = ReceiveStatusEnum.New.ToString();
            }

        }
        [DirectMethod(Timeout=180000)]
        private void BindtoControl(ReceiveHeaderModel data)
        {
            txtSearchProductCode.Text = " ";
            txtReceiveCode.Text = data.ReceiveCode;
            txtOrderNo.Text = data.OrderNo;
            //txtInvoiceNo.Text = data.InvoiceNo;
            txtReceiveStatus.Text = GetResource(data.ReceiveStatus.ToString());
            txtRemark.Text = data.Remark;

            hdReceiveStatus.Text = data.ReceiveStatus.ToString();
            hdToReprocess.Text = data.ToReprocess.ToString();
            hdFromReprocess.Text = data.FromReprocess.ToString();
            hdIsCreditNote.Text = data.IsCreditNote.ToString();
            hdIsNormal.Text = data.IsNormal.ToString();
            hdIsItemChange.Text = data.IsItemChange.ToString();
            hdIsWithoutGoods.Text = data.IsWithoutGoods.ToString();
            hdProductStatus.Text = data.ProductStatus.Code;
            hdProductStatusID.SetValue(data.ProductStatus.ProductStatusID);

            dtEstReceiveDate.SelectedDate = data.ESTReceiveDate.Value;

            #region bindCombo

            if (data.PONo != null)
            {
                cmbPONo.InsertItem(0, data.PONo, data.PONo);
                cmbPONo.Select(data.PONo);
            }

            if (data.ReceiveTypeID != null)
            {
                cmbReceiveType.InsertItem(0, data.ReceiveType, data.ReceiveTypeID);
                cmbReceiveType.Select(data.ReceiveTypeID);
            }

            if (data.DispatchTypeID != null)
            {
                cmbDispatchType.InsertItem(0, data.DispatchType, data.DispatchTypeID);
                cmbDispatchType.Select(data.DispatchTypeID);
            }

            if (data.LineID != null)
            {
                cmbLine.InsertItem(0, data.LineCode, data.LineID);
                cmbLine.Select(data.LineID);
            }

            if (data.ReceiveDetails != null && data.ReceiveDetails.Count() > 0)
            {
                StoreOfDataList.Data = data.ReceiveDetails.ToList();
            }

            if (!string.IsNullOrWhiteSpace(data.OrderType))
            {
                cmbOrderType.Select(data.OrderType);
            }

            #endregion

            #region Handle Case

            if (data.ReceiveStatus == ReceiveStatusEnum.Complete || data.ReceiveStatus == ReceiveStatusEnum.Check || data.ReceiveStatus == ReceiveStatusEnum.GenDispatch)
            {
                btnSave.Hidden = btnConfirm.Hidden = dtEstReceiveDate.ReadOnly = btnAddProduct.Hidden = btnProductCode.Disabled = true;
            }

            if (data.DispatchTypeID != null)
            {
                if (data.ReceiveStatus == ReceiveStatusEnum.Complete)
                {
                    btnGenDispatch.Enable();

                }
                else if (data.ReceiveStatus == ReceiveStatusEnum.GenDispatch)
                {
                    btnGenDispatch.Disable();
                }
            }

            cmbReceiveType.ReadOnly = cmbLine.ReadOnly = true;

            #endregion
        }
        [DirectMethod(Timeout=180000)]
        private ReceiveHeaderModel GetToModel(DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);

            Guid.TryParse(hdReceiveID.Text, out Guid receiveGUID);

            ReceiveHeaderModel rHeader = new ReceiveHeaderModel()
            {
                PONo = cmbPONo.SelectedItem.Text,
                OrderNo = txtOrderNo.Text.Trim(' '),
                //InvoiceNo = txtInvoiceNo.Text,
                OrderType = cmbOrderType.SelectedItem.Text,
                ContainerNo = "",
                IsUrgent = false,

                Remark = txtRemark.Text,

                ESTReceiveDate = dtEstReceiveDate.SelectedDate,

                ReceiveStatus = (ReceiveStatusEnum)Enum.Parse(typeof(ReceiveStatusEnum), hdReceiveStatus.Text),

                ReceiveID = receiveGUID,
                LineID = Guid.TryParse(cmbLine.SelectedItem.Value, out Guid lineGUID) ? lineGUID : (Guid?)null,
                ReceiveTypeID = Guid.TryParse(cmbReceiveType.SelectedItem.Value, out Guid receiveTypeGUID) ? receiveTypeGUID : (Guid?)null,

            };

            rHeader.ReceiveDetails = gridData;

            return rHeader;
        }
        [DirectMethod(Timeout=180000)]
        private List<ProductCustomModel> GetProductList(ProductSearchModel productSearch)
        {
            int total = 0;
            List<ProductCustomModel> data = new List<ProductCustomModel>();

            if (productSearch == null)
            {
                MessageBoxExt.Warning(GetMessage("MSG00006").MessageValue);
                return data;
            }

            Core.Domain.ApiResponseMessage apiResp = ProductClient.GetProductForInternalRec(productSearch.PONo, productSearch.ProductCode, null
                                                                    , productSearch.IsCreditNote, productSearch.IsNormal, productSearch.ToReprocess
                                                                    , productSearch.FromReprocess, productSearch.IsItemChange
                                                                    , productSearch.IsWithoutGoods, productSearch.ReferenceDispatchTypeID).Result;
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ProductCustomModel>>();
            }

            return data;
        }
        [DirectMethod(Timeout=180000)]
        protected void btnGenDispatch_Click(object sender, DirectEventArgs e)
        {
            Guid.TryParse(hdReceiveID.Text, out Guid receiveGUID);

            Core.Domain.ApiResponseMessage apiResult = ReceiveClient.GenerateDispatch(receiveGUID).Result;
            if (apiResult.IsSuccess)
            {
                X.Call("parent.App.direct.Reload", apiResult.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(apiResult.ResponseMessage == string.Empty ? GetMessage("MSG00004").MessageValue : apiResult.ResponseMessage);
            }
        }
        [DirectMethod(Timeout=180000)]
        protected void btnConfirm_Click(object sender, DirectEventArgs e)
        {
            ReceiveHeaderModel receive = GetToModel(e);

            if (receive.ReceiveDetails.Count() == 0 || receive.ReceiveDetails == null)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00084").MessageValue);
                return;
            }
            if (!receive.ESTReceiveDate.HasValue || receive.ESTReceiveDate.Value== DateTime.MinValue)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00004").MessageValue, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }

            Core.Domain.ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(receive.ESTReceiveDate.Value).Result;
            if (apiresponse.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }

            Core.Domain.ApiResponseMessage apiResult = ReceiveClient.ConfirmInternalReceive(receive).Result;
            if (apiResult.IsSuccess)
            {
                X.Call("parent.App.direct.Reload", apiResult.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(apiResult.ResponseMessage == string.Empty ? GetMessage("MSG00004").MessageValue : apiResult.ResponseMessage);
            }
        }
        [DirectMethod(Timeout=180000)]
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            ReceiveHeaderModel receive = GetToModel(e);

            if (receive.ReceiveDetails == null || receive.ReceiveDetails.Count() == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00084").MessageValue);
                return;
            }
            if (!receive.ESTReceiveDate.HasValue || receive.ESTReceiveDate.Value == DateTime.MinValue)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00004").MessageValue, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }

            Core.Domain.ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(receive.ESTReceiveDate.Value).Result;
            if (apiresponse.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }
            Core.Domain.ApiResponseMessage apiResult = ReceiveClient.SaveInternalReceive(receive).Result;
            if (apiResult.IsSuccess)
            {
                X.Call("parent.App.direct.Reload", apiResult.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(apiResult.ResponseMessage == string.Empty ? GetMessage("MSG00004").MessageValue : apiResult.ResponseMessage);
            }
        }
        [DirectMethod(Timeout=180000)]
        protected void btnAddProduct_Click(object sender, DirectEventArgs e)
        {
            Guid productGUID, productUnitGUID;
            productGUID = productUnitGUID = Guid.Empty;
            List<ProductUnit> productUnits = new List<ProductUnit>();
            string resMsg = string.Empty;
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);
            CultureInfo cultureinfo = new CultureInfo("en-US");

            #region Validate

            if (!Guid.TryParse(hdProductID.Text, out productGUID))
            {
                // Invalid searchID format
                MessageBoxExt.Warning(GetMessage("MSG00029").MessageValue);
                return;
            }

            if (nbQTY.Number < 1)
            {
                // Number must be greater than 0
                MessageBoxExt.Warning(GetMessage("MSG00058").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage apiResforUnit = ProductUnitsClient.GetProductUnits(null, productGUID, "").Result;
            if (apiResforUnit == null || apiResforUnit.IsSuccess == false)
            {
                // Data not found
                MessageBoxExt.Warning(GetMessage("MSG00006").MessageValue);
                return;
            }

            #endregion

            #region Date & LotNo

            DateTime recDate = dtEstReceiveDate.SelectedDate;
            if (!string.IsNullOrWhiteSpace(hdMFGDate.Text))
            {
                recDate = DateTime.Parse(hdMFGDate.Text);
            }

            string day = recDate.ToString("dd");
            string month = recDate.ToString("MM");
            string year = recDate.ToString("yyyy", cultureinfo);
            string lotNo = year + month + day;

            #endregion

            #region Validate Product Duplicate 

            productUnits = apiResforUnit.Get<List<ProductUnit>>();

            IEnumerable<ReceiveDetailModel> existingProduct = gridData.Where(x => x.ProductID == productGUID && x.LotNo == lotNo);
            if (existingProduct != null && existingProduct.Count() > 0)
            {
                IEnumerable<Guid> existUnitIDs = existingProduct.Select(x => x.UnitID.Value);
                IEnumerable<Guid> unitIDs = productUnits.Select(x => x.ProductUnitID);
                productUnitGUID = unitIDs.Except(existUnitIDs).FirstOrDefault();

                if (productUnitGUID == null || productUnitGUID == Guid.Empty)
                {
                    // Can't add product in same unit
                    MessageBoxExt.Warning(GetMessage("MSG00059").MessageValue);
                    return;
                }
            }

            #endregion

            ReceiveDetailModel receiveDetail = new ReceiveDetailModel()
            {
                ProductID = productGUID,
                ProductCode = txtSearchProductCode.Text,
                ProductName = txtSearchProductName.Text,
                StatusID = Guid.Parse(hdProductStatusID.Text),
                Status = hdProductStatus.Text,
                QTY = (decimal)nbQTY.Number,
                ConfirmQTY = (decimal)nbQTY.Number,
                MFGDate = recDate,
                LotNo = lotNo,
                Width = 1,
                Length = 1,
                Height = 1,

                UnitID = productUnitGUID == Guid.Empty ? productUnits.FirstOrDefault(x => x.IsBaseUOM)?.ProductUnitID : productUnitGUID,
                Unit = productUnitGUID == Guid.Empty ? productUnits.FirstOrDefault(x => x.IsBaseUOM)?.UnitAndPalletQTY : productUnits.Where(x => x.ProductUnitID == productUnitGUID).Select(x => x.UnitAndPalletQTY).FirstOrDefault()

            };

            StoreOfDataList.Add(receiveDetail);
            X.Js.Call("sumTotal");
        }
        [DirectMethod(Timeout=180000)]
        public void LoadDispatchType(string refDocID)
        {
            Guid refDocGUID = Guid.Empty;
            Guid.TryParse(refDocID, out refDocGUID);

            try
            {
                string productCode = txtSearchProductCode.Text;
                bool toReprocess = bool.Parse(hdToReprocess.Text);
                bool fromReprocess = bool.Parse(hdFromReprocess.Text);
                bool isCreditNote = bool.Parse(hdIsCreditNote.Text);
                bool isNormal = bool.Parse(hdIsNormal.Text);
                bool isItemChange = bool.Parse(hdIsItemChange.Text);
                bool isWithoutGoods = bool.Parse(hdIsWithoutGoods.Text);

                if (isWithoutGoods)
                {
                    txtRemark.Text = "Without Goods";
                }
                else if (toReprocess == true && fromReprocess == false && isNormal == false)
                {
                    txtRemark.Text = "Inspection Goods Return";
                }
                else if (toReprocess == false && fromReprocess == true && isNormal == false && isItemChange == false)
                {
                    txtRemark.Text = "FromReprocess";
                }
                else if (toReprocess == false && fromReprocess == false && isNormal == false && refDocGUID != Guid.Empty)
                {
                    txtRemark.Text = "QA Inspection";
                }
                else if (isItemChange)
                {
                    txtRemark.Text = "Repack";
                }
            }
            catch (Exception)
            {
                MessageBoxExt.Warning(GetMessage("MSG00069").MessageValue); // Configuration Not found Please contact Administrator
                return;
            }

            if (refDocGUID != Guid.Empty)
            {
                Core.Domain.ApiResponseMessage api = DocumentTypeClient.GetRefDispatchType(refDocGUID).Result;
                if (api.IsSuccess)
                {
                    DocumentType data = api.Get<List<DocumentType>>().FirstOrDefault();

                    if (data.DocumentTypeID != null)
                    {
                        cmbDispatchType.InsertItem(0, data.Name, data.DocumentTypeID);
                        cmbDispatchType.Select(data.DocumentTypeID);
                    }

                }
            }
        }
        [DirectMethod(Timeout=180000)]
        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucProductforInternalRec.BindData(action, extraParams);
        }
        [DirectMethod(Timeout=180000)]
        public void ucProduct_Select(string record)
        {
            ProductCustomModel data = JSON.Deserialize<ProductCustomModel>(record);

            hdProductID.SetValue(data.ProductID);
            hdMFGDate.SetValue(data.MFGDate != null ? data.MFGDate.Value.ToString("yyyy-MM-dd", new CultureInfo("en-US")) : string.Empty);
            txtSearchProductCode.Text = data.ProductCode;
            txtSearchProductName.Text = data.ProductName;

            ucProductforInternalRec.Close();
        }
        [DirectMethod(Timeout=180000)]
        public void btnBrowseProduct_Click()
        {
            if (string.IsNullOrWhiteSpace(cmbReceiveType.SelectedItem.Text))
            {
                MessageBoxExt.Warning(GetMessage("MSG00067").MessageValue); //Please select Receive Type
                return;
            }

            ProductSearchModel productSearch = new ProductSearchModel();

            try
            {
                productSearch.PONo = cmbPONo.SelectedItem.Text ?? string.Empty;
                productSearch.ProductCode = txtSearchProductCode.Text;
                productSearch.ToReprocess = bool.Parse(hdToReprocess.Text);
                productSearch.FromReprocess = bool.Parse(hdFromReprocess.Text);
                productSearch.IsCreditNote = bool.Parse(hdIsCreditNote.Text);
                productSearch.IsNormal = bool.Parse(hdIsNormal.Text);
                productSearch.IsItemChange = bool.Parse(hdIsItemChange.Text);
                productSearch.IsWithoutGoods = bool.Parse(hdIsWithoutGoods.Text);

                if ((productSearch.ToReprocess == true || productSearch.FromReprocess == true) && string.IsNullOrWhiteSpace(cmbPONo.SelectedItem.Text))
                {
                    // Almost Cases except QA Inspection must select PONO
                    MessageBoxExt.Warning(GetMessage("MSG00051").MessageValue); // PO No cannot be null
                    return;
                }
                else
                {

                    if (!Guid.TryParse(cmbLine.SelectedItem.Value, out Guid lineID))
                    {
                        MessageBoxExt.Warning(GetMessage("MSG00070").MessageValue); // Please Select Line
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(hdReferenceDispatchTypeID.Text))
                    {
                        productSearch.ReferenceDispatchTypeID = Guid.Parse(hdReferenceDispatchTypeID.Text);
                    }
                }
            }
            catch (Exception)
            {
                MessageBoxExt.Warning(GetMessage("MSG00069").MessageValue); // Configuration Not found Please contact Administrator
                return;
            }

            List<ProductCustomModel> listData = GetProductList(productSearch);

            if (listData.Count == 1)
            {
                ProductCustomModel data = listData.FirstOrDefault();

                if (data != null)
                {
                    hdProductID.SetValue(data.ProductID);
                    txtSearchProductCode.Text = data.ProductCode;
                    txtSearchProductName.Text = data.ProductName;
                    hdMFGDate.SetValue(data.MFGDate != null ? data.MFGDate.Value.ToString("yyyy-MM-dd", new CultureInfo("en-US")) : string.Empty);
                }
                else
                {
                    MessageBoxExt.Warning(GetMessage("MSG00006").MessageValue);
                }
            }
            else
            {
                ucProductforInternalRec.Show(productSearch);
            }
        }
        [DirectMethod(Timeout=180000)]
        public bool ValidateProduct(string gridJson, Guid productUnitGUID, Guid productGUID, string lotNo)
        {
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);

            IEnumerable<ReceiveDetailModel> existingProduct = gridData.Where(x => x.ProductID == productGUID && x.LotNo == lotNo);
            if (existingProduct != null && existingProduct.Count() > 0)
            {
                if (existingProduct.Where(x => x.UnitID == productUnitGUID).Count() > 1)
                {
                    // Can't add product in same unit
                    MessageBoxExt.Warning(GetMessage("MSG00059").MessageValue);
                    return false;
                }
            }

            return true;
        }
        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);

            string command = e.ExtraParams["command"];
            string receiveDetailID = e.ExtraParams["receiveDetailID"];
            string productID = e.ExtraParams["productID"];
            string unitID = e.ExtraParams["unitID"];

            Guid.TryParse(receiveDetailID, out Guid receiveDetailGUID);
            Guid.TryParse(productID, out Guid productGUID);
            Guid.TryParse(unitID, out Guid unitGUID);

            if (productGUID == Guid.Empty || unitGUID == Guid.Empty)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            IEnumerable<ReceiveDetailModel> removeItem = gridData.Where(x => x.ProductID != productGUID || x.UnitID != unitGUID);
            //if (removeItem != null) gridData.ToList().Remove(removeItem);

            StoreOfDataList.DataSource = removeItem;
            StoreOfDataList.DataBind();
        }
    }
}