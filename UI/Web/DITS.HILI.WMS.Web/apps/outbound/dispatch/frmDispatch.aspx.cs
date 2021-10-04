using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Rule;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using static DITS.HILI.WMS.DispatchModel.CustomModel.DispatchOtherModel;

namespace DITS.HILI.WMS.Web.apps.outbound.dispatch
{
    public partial class frmDispatch : BaseUIPage
    {

        #region Initail

        public static string ProgramCode = frmDispatchList.ProgramCode;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!X.IsAjaxRequest && !IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"], true);

            }
        }

        private void BindData(string oDataKeyId, bool isCheckOrder = false)
        {
            txtDispatch_Code.Text = oDataKeyId;
            LoadComboRule();
            if (oDataKeyId == "new")
            {

                btnLinkDispatch.Icon = Icon.Error;
                btnBook.Visible = false;
                btnApprove.Visible = false;
                btnApproveDispatch.Visible = false;
                btnCancel.Visible = false;
                btnPrint.Visible = false;

                dtDispatch_Date_Order.Value = DateTime.Now;

                dtDeliveryDate.SelectedDate = DateTime.Now;
                dtDocumentDate.SelectedDate = DateTime.Now;

                txttotalNetWeight.Value = 0;
                txttotalQTY.Value = 0;
                txttotalGrossWeight.Value = 0;

                lblDispatchStatus.Text = MyEnum.GetDispatchEnumDescription(DispatchStatusEnum.New);

                ApiResponseMessage apiRespcus = ContactClient.GetCustomer("20004431", null, null).Result;
                List<Contact> datacus = new List<Contact>();
                if (apiRespcus.ResponseCode == "0")
                {
                    datacus = apiRespcus.Get<List<Contact>>();
                    if (datacus.Count != 0)
                    {
                        if (datacus[0].ContactID != null)
                        {
                            Contact customer_edit = new Contact
                            {
                                Name = datacus[0].Name,
                                ContactID = datacus[0].ContactID,
                                Code = datacus[0].Code,
                                FullName = datacus[0].Code + ':' + datacus[0].Name,
                            };
                            StoreSubCust_Code.Add(customer_edit);
                            cmbSubCust_Code.SelectedItem.Value = datacus[0].ContactID.ToString();
                            cmbSubCust_Code.UpdateSelectedItems();
                        }
                    }

                }

                ApiResponseMessage apiRespdoc = DocumentTypeClient.GetDispatchType(null, null, null).Result;
                List<DocumentType> datadoc = new List<DocumentType>();
                if (apiRespdoc.ResponseCode == "0")
                {

                    datadoc = apiRespdoc.Get<List<DocumentType>>();
                    DocumentType itemdoc = new DocumentType();
                    itemdoc = datadoc.Where(x => x.IsDefault == true).FirstOrDefault();
                    if (itemdoc != null)
                    {
                        DocumentType document_add = new DocumentType
                        {
                            Code = itemdoc.Code,
                            Name = itemdoc.Name,
                            DocumentTypeID = itemdoc.DocumentTypeID,
                            FullName = itemdoc.Code + ':' + itemdoc.Name,
                        };

                        StoreDispatch_Type.Add(document_add);
                        cmbDispatchType.SelectedItem.Value = itemdoc.DocumentTypeID.ToString();
                        cmbDispatchType.UpdateSelectedItems();

                        ApiResponseMessage apiRespStatus = ProductStatusClient.GetByDocumentTypeID(itemdoc.DocumentTypeID).Result;
                        List<ProductStatus> datastatus = new List<ProductStatus>();
                        if (apiRespStatus.ResponseCode == "0")
                        {
                            datastatus = apiRespStatus.Get<List<ProductStatus>>();

                            if (datastatus.Count == 0)
                            {
                                //MessageBoxExt.ShowError("Product status not map dispatch type! Please map data before use this dispatch type !");
                                MessageBoxExt.ShowError("สถานะสินค้ายังไม่ได้ ตั้งค่ากับ Dispatch Type นี้! กรุณาติดต่อผู้ดูแลระบบ");
                                btnSave.Hidden = true;
                                return;
                            }
                            else
                            {
                                btnSave.Hidden = false;
                            }

                            hidProduct_Status_Code.Text = datastatus[0].ProductStatusID.ToString();
                            hidProduct_Status_Name.Text = datastatus[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductStatus.Name.ToString();

                            if (datastatus[0].ProductStatusMapCollection.Count != 0)
                            {
                                hidProduct_Sub_Status_Code.Text = datastatus[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductSubStatus.ProductSubStatusID.ToString();
                                hidProduct_Sub_Status_Name.Text = datastatus[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductSubStatus.Name.ToString();
                            }
                        }
                        ApiResponseMessage apiRespItf = ItfInterfaceMappingClient.GetByDocument(itemdoc.DocumentTypeID).Result;
                        List<ItfInterfaceMapping> dataItf = new List<ItfInterfaceMapping>();
                        if (apiRespItf.ResponseCode == "0")
                        {
                            dataItf = apiRespItf.Get<List<ItfInterfaceMapping>>();
                            hidIsMarketing.Text = (dataItf[0].IsMarketing != null ? (dataItf[0].IsMarketing == false ? "0" : "1") : "0");
                            X.Js.Call("type_change");
                            DefaultWarehouse();
                            DefaltPOInternal();
                        }

                    }

                }

                ApiResponseMessage apiRespShip = ShipToClient.Get("", null, null).Result;
                List<ShippingTo> datashipto = new List<ShippingTo>();
                if (apiRespShip.ResponseCode == "0")
                {
                    datashipto = apiRespShip.Get<List<ShippingTo>>();
                    if (datashipto != null)
                    {

                        ShippingTo _dataadd = datashipto.Where(x => x.IsDefault == true).FirstOrDefault();
                        if (_dataadd != null)
                        {
                            ShippingTo shipto_add = new ShippingTo
                            {
                                Name = _dataadd.Name,
                                ShipToId = _dataadd.ShipToId,
                            };
                            StoreShipTo.Add(shipto_add);
                            cmbShippingTo.SelectedItem.Value = _dataadd.ShipToId.ToString();
                            cmbShippingTo.UpdateSelectedItems();
                            //this.cmbShippingTo.ReadOnly = true;
                            ShipToCombo();
                        }
                    }
                }




                return;
            }
            fctRevisePo.Hidden = true;
            // this.txtRevisePoNo.Hidden = true;

            cmbDispatchType.SelectOnFocus = true;

            string _status = Request.QueryString["oDataStatus"];

            ApiResponseMessage apiResp = new ApiResponseMessage();



            if (_status == "10")//Type=1
            {
                apiResp = DispatchClient.GetByID(new Guid(oDataKeyId)).Result;
            }
            else if (_status == "20" || _status == "30" || _status == "33" || _status == "35" || _status == "40" || _status == "201")//Type=2
            {
                apiResp = DispatchClient.GetBookingByID(new Guid(oDataKeyId)).Result;
            }
            else if (_status == "50")//Type=3
            {
                apiResp = DispatchClient.GetConsolidateByID(new Guid(oDataKeyId)).Result;
            }
            else if (_status == "100" || _status == "102")//Type= 2,3,4
            {
                apiResp = DispatchClient.GetDispatchCompleteById(new Guid(oDataKeyId)).Result;
            }
            else if (_status == "202" || _status == "203")//Type=4
            {
                apiResp = DispatchClient.GetPackingById(new Guid(oDataKeyId)).Result;
            }
            // var apiResp = (_status == "10"  ? DispatchClient.GetByID(new Guid(oDataKeyId)).Result : ( int.Parse(_status) >= 50 && int.Parse(_status) < 201 ? DispatchClient.GetConsolidateByID(new Guid(oDataKeyId)).Result : (int.Parse(_status) == 202? DispatchClient.GetPackingById(new Guid(oDataKeyId)).Result: DispatchClient.GetBookingByID(new Guid(oDataKeyId)).Result)));  
            DispatchModels dataItem = new DispatchModels();
            if (apiResp.ResponseCode == "0")
            {
                dataItem = apiResp.Get<DispatchModels>();

            }

            if (dataItem != null)
            {

                hidIsMarketing.Text = (dataItem.IsMarketing ? "1" : "0");
                if (hidIsMarketing.Text == "1")
                {//?Market ไม่ให้แก้ PO เพราะทำ Running ให้ 
                    txtDispatch_Refered_1.ReadOnly = true;
                }
                X.Js.Call("type_change");

                txtDispatch_Code.Text = dataItem.DispatchCode;
                txtDispatch_Refered_1.Text = dataItem.Pono;
                txtDispatch_Refered_2.Text = dataItem.OrderNo;


                chkIsUrgent.Checked = (dataItem.IsUrgent ?? false);
                chkIsBackorder.Checked = (dataItem.IsBackOrder ?? false);
                //if (this.chkIsBackorder.Checked)
                //{
                //    this.btnApprove.Disabled = true;
                //}
                //else {
                //    this.btnApprove.Disabled = false;
                //}

                txtDispatch_Remark.Text = dataItem.Remark;

                string _dispatchstatusname = ((!dataItem.IsBackOrder.HasValue || dataItem.IsBackOrder.Value == false) && dataItem.DispatchStatusId == (int)DispatchStatusEnum.InBackOrder ? GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), DispatchStatusEnum.InprogressConfirm.ToString())) : GetDispatchEnumDescription((DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), dataItem.DispatchStatusId.Value.ToString())));
                //this.lblDispatchStatus.Text = dataItem.DispatchStatusName;
                lblDispatchStatus.Text = _dispatchstatusname;

                if (dataItem.OrderDate != null)
                {
                    dtDispatch_Date_Order.SelectedDate = dataItem.OrderDate.Value;
                }

                if (dataItem.DocumentDate != null)
                {
                    dtDocumentDate.SelectedDate = dataItem.DocumentDate.Value;
                }

                if (dataItem.DeliveryDate != null)
                {
                    dtDeliveryDate.SelectedDate = dataItem.DeliveryDate.Value;
                }

                DocumentType document_edit = new DocumentType
                {
                    Name = dataItem.DocumentName,
                    DocumentTypeID = dataItem.DocumentId.Value
                };

                StoreDispatch_Type.Add(document_edit);
                cmbDispatchType.SelectedItem.Value = dataItem.DocumentId.Value.ToString();
                cmbDispatchType.UpdateSelectedItems();
                cmbDispatchType.ReadOnly = true;

                StoreDispatch_Type_All.Add(document_edit);
                cmbDispatchTypeAll.SelectedItem.Value = dataItem.DocumentId.Value.ToString();
                cmbDispatchTypeAll.UpdateSelectedItems();
                cmbDispatchTypeAll.ReadOnly = true;

                ApiResponseMessage apiRespdoc = DocumentTypeClient.GetDispatchType(null, null, null).Result;
                List<DocumentType> datadoc = new List<DocumentType>();
                if (apiRespdoc.ResponseCode == "0")
                {
                    datadoc = apiRespdoc.Get<List<DocumentType>>();
                    DocumentType itemdoc = new DocumentType();
                    itemdoc = datadoc.Where(x => x.DocumentTypeID == dataItem.DocumentId.Value).FirstOrDefault();
                    if (itemdoc == null)
                    {
                        cmbDispatchType.Hidden = true;
                        cmbDispatchTypeAll.Hidden = false;
                    }
                    else
                    {
                        cmbDispatchType.Hidden = false;
                        cmbDispatchTypeAll.Hidden = true;
                    }
                }


                ApiResponseMessage apiRespStatus = ProductStatusClient.GetByDocumentTypeID(new Guid(dataItem.DocumentId.Value.ToString())).Result;
                List<ProductStatus> datastatus = new List<ProductStatus>();
                if (apiRespStatus.ResponseCode == "0")
                {
                    datastatus = apiRespStatus.Get<List<ProductStatus>>();

                    hidProduct_Status_Code.Text = datastatus[0].ProductStatusID.ToString();
                    hidProduct_Status_Name.Text = datastatus[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductStatus.Name.ToString();

                }

                Contact customer_edit = new Contact
                {
                    Name = dataItem.CustomerName,
                    ContactID = dataItem.CustomerId.Value,
                    Code = dataItem.CustomerCode,
                    FullName = dataItem.CustomerCode + ":" + dataItem.CustomerName,
                };
                StoreSubCust_Code.Add(customer_edit);
                cmbSubCust_Code.SelectedItem.Value = dataItem.CustomerId.Value.ToString();
                cmbSubCust_Code.UpdateSelectedItems();

                ShippingTo shipto_edit = new ShippingTo
                {
                    Name = dataItem.ShipptoName,
                    ShipToId = dataItem.ShipptoId.Value,
                };
                StoreShipTo.Add(shipto_edit);
                cmbShippingTo.SelectedItem.Value = dataItem.ShipptoId.ToString();
                cmbShippingTo.UpdateSelectedItems();
                cmbShippingTo.ReadOnly = true;

                if (dataItem.FromwarehouseId != null)
                {

                    Warehouse fromwarehouse_edit = new Warehouse
                    {
                        Name = dataItem.FromwarehouseName,
                        WarehouseID = dataItem.FromwarehouseId.Value,
                    };

                    StoreWarehouseFrom.Add(fromwarehouse_edit);
                    cmbWarehouseFrom.SelectedItem.Value = dataItem.FromwarehouseId.ToString();
                    cmbWarehouseFrom.UpdateSelectedItems();
                }

                if (dataItem.TowarehouseId != null)
                {

                    Warehouse Towarehouse_edit = new Warehouse
                    {
                        Name = dataItem.TowarehouseName,
                        WarehouseID = dataItem.TowarehouseId.Value,
                    };

                    StoreWarehouseTo.Add(Towarehouse_edit);
                    cmbWarehouseTo.SelectedItem.Value = dataItem.TowarehouseId.ToString();
                    cmbWarehouseTo.UpdateSelectedItems();
                }


                txtDispatchStatusId.Text = dataItem.DispatchStatusId.ToString();

                if (dataItem.TypeTotal == 1)
                {
                    decimal? qty = dataItem.DispatchDetailModelsCollection.Sum(x => (x.Quantity));
                    txttotalQTY.Text = qty.Value.ToString("#,##0.00");
                }
                else if (dataItem.TypeTotal == 2)
                {
                    decimal? qty = dataItem.DispatchDetailModelsCollection.Sum(x => (x.Quantity));
                    txttotalQTY.Text = qty.Value.ToString("#,##0.00");
                }
                else if (dataItem.TypeTotal == 3)
                {
                    decimal? qty = dataItem.DispatchDetailModelsCollection.Sum(x => (x.ConsolidateQTY));
                    txttotalQTY.Text = qty.Value.ToString("#,##0.00");
                }
                else if (dataItem.TypeTotal == 4)
                {
                    decimal? qty = dataItem.DispatchDetailModelsCollection.Sum(x => (x.PickQTY));
                    txttotalQTY.Text = qty.Value.ToString("#,##0.00");
                }

                StoreDispatch.DataSource = dataItem.DispatchDetailModelsCollection.OrderBy(e=>e.ProductCode).ToList();
                StoreDispatch.DataBind();

                if (dataItem.DispatchStatusId > (int)DispatchStatusEnum.New || dataItem.ReferenceId != null)
                {
                    ReadOnlyInfo();
                }
                if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.New || dataItem.DispatchStatusId == (int)DispatchStatusEnum.Inprogress || dataItem.DispatchStatusId == (int)DispatchStatusEnum.Complete || dataItem.DispatchStatusId == (int)DispatchStatusEnum.Close)
                {
                    btnCancelAll.Hidden = true;
                }
                else
                {
                    btnCancelAll.Hidden = false;
                }

                if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.New)
                {
                    btnBook.Visible = true;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = true;
                    btnClear.Visible = false;
                    ColRuleEdit.Hidden = false;
                    // this.ColRuleReadOnly.Hidden = true;
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.Inprogress)
                {
                    btnBook.Visible = false;
                    btnCancel.Visible = true;
                    btnApprove.Visible = true;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colProductLot.Hidden = false;
                    colPalletCode.Hidden = false;
                    colLocationCode.Hidden = false;
                    colBookingQTY.Hidden = false;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    fdAddProduct.Hidden = true;
                    colReviseNo.Hidden = false;
                    colOrderNo.Hidden = false;
                    List<DispatchDetailModels> checkbackorder = dataItem.DispatchDetailModelsCollection.Where(x => x.IsBackOrder == false).ToList();
                    if (checkbackorder.Count() == 0 || dataItem.DispatchDetailModelsCollection.Count() == 0)
                    {
                        // this.btnApprove.Visible = false;
                        btnApprove.Hidden = true;
                    }
                    else
                    {
                        //this.btnApprove.Visible = true;
                        btnApprove.Hidden = false;
                    }

                    checkbackorder = dataItem.DispatchDetailModelsCollection.Where(x => x.IsBackOrder == true).ToList();
                    if (checkbackorder.Count() > 0)
                    {
                        // btnAddPallet.Visible = true;
                        btnAddPallet.Hidden = false;
                    }
                    else
                    {
                        //btnAddPallet.Visible = false;
                        btnAddPallet.Hidden = true;
                    }

                    if (isCheckOrder && txtDispatch_Refered_2.Text.Trim() != "")
                    {
                        int iCount = dataItem.DispatchDetailModelsCollection.Where(x => x.OrderNo != txtDispatch_Refered_2.Text && x.IsBackOrder == false).Count();
                        if (iCount > 0)
                        {
                            MessageBoxExt.Warning(GetMessage("MSG00104").MessageValue);
                        }
                    }
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.InBackOrder)
                {
                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colProductLot.Hidden = false;
                    colPalletCode.Hidden = false;
                    colLocationCode.Hidden = false;
                    colBookingQTY.Hidden = false;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    fdAddProduct.Hidden = true;
                    colReviseNo.Hidden = true;

                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.InprogressConfirm || dataItem.DispatchStatusId == (int)DispatchStatusEnum.Register || dataItem.DispatchStatusId == (int)DispatchStatusEnum.InprogressConfirmQA)
                {
                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colProductLot.Hidden = false;
                    colPalletCode.Hidden = false;
                    colLocationCode.Hidden = false;
                    colBookingQTY.Hidden = false;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    fdAddProduct.Hidden = true;
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.WaitingConfirmDispatch)
                {
                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = true;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colProductLot.Hidden = false;
                    colLocationCode.Hidden = false;
                    colPalletCode.Hidden = false;
                    colBookingQTY.Hidden = false;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    dtApproveDispatch.Hidden = false;
                    dtApproveDispatch.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    colPickLocationCode.Hidden = false;
                    colPickQTY.Hidden = false;
                    colPICKUNIT.Hidden = false;
                    colPickPalletCode.Hidden = false;
                    colConsolidateQTY.Hidden = false;
                    colConsolidateQTYUnitName.Hidden = false;
                    colReviseNo.Hidden = true;
                    fdAddProduct.Hidden = true;

                    if (chkIsBackorder.Checked)
                    {
                        dtApproveDispatch.Hidden = true;
                        btnApproveDispatch.Hidden = true;
                    }
                    // this.btnCancelDispatch.Hidden = false;
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.InternalReceive)
                {

                    dtApproveDispatch.Hidden = false;
                    dtApproveDispatch.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colProductLot.Hidden = false;
                    colPalletCode.Hidden = false;
                    colLocationCode.Hidden = false;
                    colBookingQTY.Hidden = false;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    fdAddProduct.Hidden = true;
                    colReviseNo.Hidden = true;
                    btnApproveDispatchInternal.Hidden = false;

                    if (chkIsBackorder.Checked)
                    {
                        dtApproveDispatch.Hidden = true;
                        btnApproveDispatch.Hidden = true;
                    }
                    // this.btnCancelDispatchInternal.Hidden = false;
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.WaitingConfirmDispatchNoneRegister)
                {

                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colUnit.Hidden = true;
                    colProductStatus.Hidden = true;
                    colRemark.Hidden = true;
                    colProductLot.Hidden = true;
                    colLocationCode.Hidden = true;
                    colPalletCode.Hidden = true;
                    colBookingQTY.Hidden = true;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    dtApproveDispatch.Hidden = false;
                    dtApproveDispatch.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    colPickPalletCode.Hidden = false;
                    colPickLocationCode.Hidden = false;
                    colPickProductLot.Hidden = false;
                    colPickQTY.Hidden = false;
                    colPICKUNIT.Hidden = false;
                    colConsolidateQTY.Hidden = true;
                    colConsolidateQTYUnitName.Hidden = true;
                    colReviseNo.Hidden = true;
                    fdAddProduct.Hidden = true;
                    btnApproveDispatchPicking.Hidden = false;
                    // this.btnCancelDispatchPicking.Hidden = false;
                    if (chkIsBackorder.Checked)
                    {
                        dtApproveDispatch.Hidden = true;
                        btnApproveDispatch.Hidden = true;
                    }
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.WaitingConfirmDispatchQA)
                {

                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    colUnit.Hidden = true;
                    colProductStatus.Hidden = true;
                    colRemark.Hidden = true;
                    colProductLot.Hidden = true;
                    colLocationCode.Hidden = true;
                    colPalletCode.Hidden = true;
                    colBookingQTY.Hidden = true;
                    colDispatch_Quantity.Hidden = true;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    dtApproveDispatch.Hidden = false;
                    dtApproveDispatch.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    colPickPalletCode.Hidden = false;
                    colPickLocationCode.Hidden = false;
                    colPickProductLot.Hidden = false;
                    colPickQTY.Hidden = false;
                    colPICKUNIT.Hidden = false;
                    colConsolidateQTY.Hidden = true;
                    colConsolidateQTYUnitName.Hidden = true;
                    colReviseNo.Hidden = true;
                    fdAddProduct.Hidden = true;
                    btnApproveDispatchQA.Hidden = false;
                    // this.btnCancelDispatchPicking.Hidden = false;
                    if (chkIsBackorder.Checked)
                    {
                        dtApproveDispatch.Hidden = true;
                        btnApproveDispatch.Hidden = true;
                    }
                }
                else if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.Complete || dataItem.DispatchStatusId == (int)DispatchStatusEnum.Close)
                {
                    if (dataItem.TypeTotal == 1)
                    {
                        btnBook.Visible = true;
                        btnCancel.Visible = false;
                        btnApprove.Visible = false;
                        btnApproveDispatch.Visible = false;
                        btnPrint.Visible = false;
                        btnSave.Visible = true;
                        btnClear.Visible = false;
                        ColRuleEdit.Hidden = false;
                    }
                    else if (dataItem.TypeTotal == 2)
                    {
                        colProductLot.Hidden = false;
                        colLocationCode.Hidden = false;
                        colPalletCode.Hidden = false;
                        colBookingQTY.Hidden = false;
                        colDispatch_Quantity.Hidden = true;
                        colUnit.Hidden = true;
                    }
                    else if (dataItem.TypeTotal == 3)
                    {
                        colProductLot.Hidden = true;
                        colLocationCode.Hidden = true;
                        colPalletCode.Hidden = true;
                        colBookingQTY.Hidden = true;
                        colDispatch_Quantity.Hidden = true;
                        colUnit.Hidden = true;
                        colRemark.Hidden = true;
                        colPickProductLot.Hidden = false;
                        colPickQTY.Hidden = false;
                        colPICKUNIT.Hidden = false;
                        colPickPalletCode.Hidden = false;
                        colPickLocationCode.Hidden = false;
                        colConsolidateQTY.Hidden = false;
                        colConsolidateQTYUnitName.Hidden = false;
                    }
                    else if (dataItem.TypeTotal == 4)
                    {
                        colProductLot.Hidden = true;
                        colLocationCode.Hidden = true;
                        colPalletCode.Hidden = true;
                        colBookingQTY.Hidden = true;
                        colDispatch_Quantity.Hidden = true;
                        colUnit.Hidden = true;
                        colRemark.Hidden = true;
                        colPickProductLot.Hidden = false;
                        colPickQTY.Hidden = false;
                        colPICKUNIT.Hidden = false;
                        colPickPalletCode.Hidden = false;
                        colPickLocationCode.Hidden = false;
                    }
                    btnBook.Visible = false;
                    btnCancel.Visible = false;
                    btnApprove.Visible = false;
                    btnApproveDispatch.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    colDel.Visible = false;
                    chkIsUrgent.Disabled = true;
                    chkIsBackorder.Disabled = true;
                    if (dataItem.DispatchStatusId == (int)DispatchStatusEnum.Complete)
                    {
                        dtApproveDispatch.Hidden = false;
                        if (dataItem.DocumentApproveDate != null)
                        {
                            dtApproveDispatch.SelectedDate = dataItem.DocumentApproveDate.Value;
                        }
                        btnSaveApproveDate.Hidden = false;
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        txtReason.Hidden = false;
                        txtReason.Text = dataItem.Reason;
                    }
                    colReviseNo.Hidden = true;
                    fdAddProduct.Hidden = true;
                    btnApproveDispatchPicking.Hidden = true;
                }
            }
        }

        public static string GetDispatchEnumDescription(DispatchStatusEnum status)
        {

            Type type = typeof(DispatchStatusEnum);
            System.Reflection.MemberInfo[] memInfo = type.GetMember(status.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = ((DescriptionAttribute)attributes[0]).Description;

            return description;
        }

        private void ReadOnlyInfo()
        {
            cmbSubCust_Code.ReadOnly = true;
            txtDispatch_Remark.ReadOnly = true;
            dtDispatch_Date_Order.ReadOnly = true;
            dtDocumentDate.ReadOnly = true;
            dtDeliveryDate.ReadOnly = true;
            txtDispatch_Refered_1.ReadOnly = true;
            txtDispatch_Refered_2.ReadOnly = true;
            cmbWarehouseFrom.ReadOnly = true;
            cmbWarehouseTo.ReadOnly = true;
        }

        private void LoadComboRule()
        {
            ApiResponseMessage apiRespRule = RulesClient.GetBookingRule("", null, null).Result;
            List<SpecialBookingRule> dataRule = new List<SpecialBookingRule>();
            if (apiRespRule.ResponseCode == "0")
            {
                dataRule = apiRespRule.Get<List<SpecialBookingRule>>();
                StoreComboRule.DataSource = dataRule;
                StoreComboRule.DataBind();
            }
        }

        private void DefaltPOInternal()
        {
            if (hidIsMarketing.Text == "1")
            {
                //DefaultPO
                ApiResponseMessage apiRespPO = DispatchClient.GetDispatchPreFixEnum(DispatchPreFixTypeEnum.PONO_INTERNAL).Result;
                List<string> datapo = new List<string>();
                if (apiRespPO.ResponseCode == "0")
                {
                    txtDispatch_Refered_1.Text = apiRespPO.Data.ToString();
                    txtDispatch_Refered_1.ReadOnly = true;
                }
            }
            else
            {
                txtDispatch_Refered_1.Text = "";
                txtDispatch_Refered_1.ReadOnly = false;
            }
        }

        private void DefaultWarehouse()
        {
            if (hidIsMarketing.Text == "1")
            {

                ApiResponseMessage apiRespwhfrom = WarehouseClient.GetWarehouseAll(null, "W3", false, null, null).Result;
                List<WarehouseModel> datawhffrom = new List<WarehouseModel>();
                if (apiRespwhfrom.ResponseCode == "0")
                {
                    datawhffrom = apiRespwhfrom.Get<List<WarehouseModel>>();
                    if (datawhffrom != null)
                    {
                        if (datawhffrom[0].WarehouseID != null)
                        {

                            Warehouse fromwarehouse_edit = new Warehouse
                            {
                                Name = datawhffrom[0].WarehouseName,
                                WarehouseID = datawhffrom[0].WarehouseID,
                            };

                            StoreWarehouseFrom.Add(fromwarehouse_edit);
                            cmbWarehouseFrom.SelectedItem.Value = datawhffrom[0].WarehouseID.ToString();
                            cmbWarehouseFrom.UpdateSelectedItems();
                        }
                    }

                }

                ApiResponseMessage apiRespwhto = WarehouseClient.GetWarehouseAll(null, "412", false, null, null).Result;
                List<WarehouseModel> datawhfto = new List<WarehouseModel>();
                if (apiRespwhto.ResponseCode == "0")
                {
                    datawhfto = apiRespwhto.Get<List<WarehouseModel>>();

                    if (datawhfto.Count != 0)
                    {
                        if (datawhfto[0].WarehouseID != null)
                        {

                            Warehouse towarehouse_edit = new Warehouse
                            {
                                Name = datawhfto[0].WarehouseName,
                                WarehouseID = datawhfto[0].WarehouseID,
                            };

                            StoreWarehouseTo.Add(towarehouse_edit);
                            cmbWarehouseTo.SelectedItem.Value = datawhfto[0].WarehouseID.ToString();
                            cmbWarehouseTo.UpdateSelectedItems();
                        }

                    }


                }

            }
            else
            {
                ApiResponseMessage apiRespwhall = WarehouseClient.GetWarehouseAll(null, "W3", false, null, null).Result;
                List<WarehouseModel> datawhfall = new List<WarehouseModel>();
                if (apiRespwhall.ResponseCode == "0")
                {
                    datawhfall = apiRespwhall.Get<List<WarehouseModel>>();
                    if (datawhfall != null)
                    {
                        if (datawhfall[0].WarehouseID != null)
                        {

                            Warehouse allwarehouse_edit = new Warehouse
                            {
                                Name = datawhfall[0].WarehouseName,
                                WarehouseID = datawhfall[0].WarehouseID,
                            };

                            StoreWarehouseFrom.Add(allwarehouse_edit);
                            cmbWarehouseFrom.SelectedItem.Value = datawhfall[0].WarehouseID.ToString();
                            cmbWarehouseFrom.UpdateSelectedItems();

                            //this.StoreWarehouseTo.Add(allwarehouse_edit);
                            //this.cmbWarehouseTo.SelectedItem.Value = datawhfall[0].WarehouseID.ToString();
                            //this.cmbWarehouseTo.UpdateSelectedItems();
                        }
                    }

                }

            }
        }
        #endregion

        #region Method
        [DirectMethod(Timeout=180000)]
        public void SetDefault()
        {
            if (txtAddQtyMax.Text != "")
            {
                txtDispatchQty.SetMaxValue(double.Parse(txtAddQtyMax.Text));
            }

        }
        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            BindData(param);
        }
        [DirectMethod(Timeout=180000)]
        public void BookingReload(string msg)
        {
            NotificationExt.Show("Booking", msg);
            BindData(txtDispatch_Code.Text, false);
        }
        [DirectMethod(Timeout=180000)]
        public void ReLoadDataDirectMethod(string param)
        {
            X.Call("parent.App.direct.ReLoadDataDirectMethod", "");
        }
        
        [DirectMethod(Timeout=180000)]
        public void CloseJob(string strReason)
        {
            //string strDispatCode = this.txtDispatch_Code.Text;

            //#region [ Close Job ]

            //DataServiceModel dataService = new DataServiceModel();
            //dataService.Add<string>("Dispatch_Code", strDispatCode);
            //dataService.Add<string>("Reason", strReason);

            //if (this.txtDispatchStatusId.Text.ToInt() >= 22 && this.txtDispatchStatusId.Text.ToInt() < 77)
            //{


            //    bool data1 = WebServiceHelper.Post<bool>("DispatchCloseJob", dataService.GetObject());
            //    if (data1)
            //    {
            //        X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage("MS0101"));
            //        X.Call("parentAutoLoadControl.close()");
            //    }
            //    else
            //    {
            //        MessageBoxExt.ShowError("Error to Close Job");
            //    }

            //}

            //
            //else
            //#region [ Cancel Job ]

            //{

            //    //Delete
            //    Results data1 = WebServiceHelper.Post<Results>("DeletePreDispatch", dataService.GetObject());
            //    if (data1.result)
            //    {
            //        X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage("MS0101"));
            //        X.Call("parentAutoLoadControl.close()");
            //    }
            //    else
            //    {
            //        MessageBoxExt.ShowError(data1.message);
            //    }

            //}

            // [ Cancel Job ]
        }

        [DirectMethod(Timeout=180000)]
        public void DoConfirm()
        {

            ApiResponseMessage apiResp = DispatchClient.OnValidateBooking(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, (hidIsMarketing.Text == "1" ? "412" : "111")).Result;
            List<DPDetailItemBackOrder> dataItem = new List<DPDetailItemBackOrder>();
            if (apiResp.IsSuccess)
            {
                dataItem = apiResp.Get<List<DPDetailItemBackOrder>>();

                if (dataItem.Count > 0)
                {
                    int count = 0;
                    string strovermsg = " อื่น .....";
                    string _strmsg = "";
                    _strmsg = "<center><font color='red'>สินค้าไม่พอสำหรับการจอง</font><br>";
                    dataItem.ForEach(item =>
                    {
                        if (count <= 5)
                        {
                            _strmsg += "สินค้า Code:" + item.ProductCode;
                            // _strmsg += " Booking QTY:" + item.BookingQTY.ToString("0.00");
                            _strmsg += " จองได้: " + item.RemainQTY.ToString("0.00") + " " + item.UnitName + " (ตามกฎ: " + item.RuleName + ")";
                            _strmsg += " <font color='red'>เกิน: " + item.BackOrderQTY.ToString("0.00") + " " + item.UnitName + " </font><br>";

                        }
                        else if (count == 6)
                        {
                            _strmsg += strovermsg;
                            return;
                        }

                        count++;

                    });
                    _strmsg += "ถ้ากดยันข้อมูลจะเกิด BlackOrder!! หรือ  กดยกเลิกแล้วกลับไปแก้ไขจำนวน." + "<br>";
                    _strmsg += "คุณต้องการกดยืนยันหรือยกเลิก ?<center>";

                    X.Msg.Confirm("เตือน", _strmsg, new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoYes()",
                            Text = "Yes"
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoNo()",
                            Text = "No"
                        }
                    }).Show();
                }
                else
                {
                    DoYes();
                    X.Call("parent.App.direct.BookingReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            else
            {
                MessageBoxExt.ShowError("data not found.");
            }


        }

        [DirectMethod(Timeout=180000)]
        public void DoYes()
        {
            try
            {
                bool isSuccess = true;

                ApiResponseMessage apiRespStatus = DispatchClient.OnBooking(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, (hidIsMarketing.Text == "1" ? "412" : "111")).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode == "0")
                {
                    isSuccess = bool.Parse(apiRespStatus.Data.ToString());
                    if (isSuccess)
                    {
                        X.Call("parent.App.direct.BookingReload");
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    }

                }
                else
                {
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }

        [DirectMethod(Timeout=180000)]
        public void DoNo()
        {
            X.Msg.Hide();
        }
        
        [DirectMethod(Timeout=180000)]
        public void DoApproveDispatchConfirm()
        {
            string _dispatchid = Request.QueryString["oDataKeyId"];
            ApiResponseMessage apiResp = DispatchClient.GetConsolidateByID(new Guid(_dispatchid)).Result;
            DispatchModels data = apiResp.Get<DispatchModels>();
            if (data == null)
            {
                return;
            }

            if (apiResp.IsSuccess)
            {
                //var _temp = (from s in data.DispatchDetailModelsCollection
                //             where s.IsActive==true
                //             select new
                //             {
                //                 BookingId = s.BookingId,
                //                 Quantity = s.Quantity,
                //             } into g
                //             group g by new
                //             {
                //                 g.BookingId,
                //                 g.Quantity
                //             } into x
                //             select new
                //             {
                //                 BookingId = x.Key.BookingId,
                //                 Quantity = x.Key.Quantity,
                //             }
                //    ).ToList(); 

                decimal? ditspatchqty = data.DispatchDetailModelsCollection.Sum(x => x.Quantity);
                decimal? consolidateqty = data.DispatchDetailModelsCollection.Where(x=>x.IsActive==true).Sum(x => x.ConsolidateQTY);
                if (ditspatchqty != consolidateqty)
                {
                    X.Msg.Confirm("เตือน", "จำนวนการจ่าย (Dispatch = " + string.Format("{0:#,##0}", ditspatchqty) + ") ไม่เท่ากับจำนวนที่ขึ้นรถ (Consolidate = " + string.Format("{0:#,##0}", consolidateqty) + ") ! คุณแน่ใจที่จะยืนยันตามยอดนี้ใช่หรือไม่ ? ", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoYesApproveDispatch()",
                            Text = "Yes"
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoNoApproveDispatch()",
                            Text = "No"
                        }
                    }).Show();
                }
                else
                {
                    DoYesApproveDispatch();
                }
            }
            else
            {
                MessageBoxExt.ShowError("data not found.");
            }


        }
        [DirectMethod(Timeout = 180000)]
        public void DoApproveDispatchPickingConfirm()
        {
            string _dispatchid = Request.QueryString["oDataKeyId"];
            ApiResponseMessage apiResp = DispatchClient.GetPackingById(new Guid(_dispatchid)).Result;
            DispatchModels data = apiResp.Get<DispatchModels>();
            if (data == null)
            {
                return;
            }

            if (apiResp.IsSuccess)
            {
                decimal ditspatchqty = data.TotalDispatchQty;
                decimal? pickingqty = data.DispatchDetailModelsCollection.Sum(x => x.PickQTY);

                if (ditspatchqty != pickingqty)
                {
                    X.Msg.Confirm("เตือน", "จำนวนการจ่าย (Dispatch = " + string.Format("{0:#,##0}", ditspatchqty) + ") ไม่เท่ากับจำนวนที่หยิบ (Pick = " + string.Format("{0:#,##0}", pickingqty) + ") ! คุณแน่ใจที่จะยืนยันตามยอดนี้ใช่หรือไม่ ? ", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoYesApproveDispatchPicking()",
                            Text = "Yes"
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DoNoApproveDispatchPicking()",
                            Text = "No"
                        }
                    }).Show();
                }
                else
                {
                    DoYesApproveDispatchPicking();
                }
            }
            else
            {
                MessageBoxExt.ShowError("data not found.");
            }


        }

        [DirectMethod(Timeout=180000)]
        public void DoYesApproveDispatch()
        {
            try
            {
                if (dtApproveDispatch.Text == DateTime.MinValue.ToString())
                {
                    MessageBoxExt.ShowError("Please input approve date", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");

                    return;
                }
                Core.Domain.ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(dtApproveDispatch.SelectedDate).Result;
                if (apiresponse.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");
                    return;
                }

                bool isSuccess = true;
                ApiResponseMessage apiRespStatus = DispatchClient.OnApproveDispatch(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, dtApproveDispatch.SelectedDate).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }
                if (isSuccess)
                {
                    X.Call("parent.App.direct.ApproveDispatchReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
       
        [DirectMethod(Timeout=180000)]
        public void DoNoApproveDispatch()
        {
            X.Msg.Hide();
        }

        [DirectMethod(Timeout=180000)]
        public void DoCancelDispatchConfirm()
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Title = "เตือน",
                Message = "กรุณากรอกเหตุผลในการยกเลิก:",
                Width = 450,
                Buttons = MessageBox.Button.OKCANCEL,
                AnimEl = btnCancelAll.ClientID,
                Multiline = true,
                Fn = new JFunction { Fn = "showResultText" }
            });

        }

        [DirectMethod(Timeout=180000)]
        public void DoYesCancelDispatch(string text)
        {
            try
            {
                ApiResponseMessage apiRespStatus = DispatchClient.OnCancelAll(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, text).Result;
                if (apiRespStatus.ResponseCode == "0")
                {
                    X.Call("parent.App.direct.CancelReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
                else
                {
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        [DirectMethod(Timeout=180000)]
        public void DoNoCancelDispatch()
        {
            X.Msg.Hide();
        }

       
        [DirectMethod(Timeout=180000)]
        public void DoYesApproveDispatchPicking()
        {
            try
            {
                if (dtApproveDispatch.Text == DateTime.MinValue.ToString())
                {
                    MessageBoxExt.ShowError("Please input approve date", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");

                    return;
                }
                Core.Domain.ApiResponseMessage apiresponse = ClientService.Master.MonthEndClient.CheckCutoffDate(dtApproveDispatch.SelectedDate).Result;
                if (apiresponse.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");
                    return;
                }


                bool isSuccess = true;

                ApiResponseMessage apiRespStatus = DispatchClient.OnApproveDispatchPicking(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, dtApproveDispatch.SelectedDate).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.ApproveDispatchReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        
        [DirectMethod(Timeout=180000)]
        public void DoNoApproveDispatchPicking()
        {
            X.Msg.Hide();
        }
         
        [DirectMethod(Timeout=180000)]
        public void GetProduct(string _product_code)
        {

            if (hidProduct_Status_Code.Text == "")
            {

                MessageBoxExt.ShowError("กรุณาเลือก Dispatch Type ก่อน เลือกสินค้า", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbDispatchType').focus('', 10); ");

                return;
            }

            if (_product_code == "")
            {
                ucProductDispatchSelect.Show(_product_code, hidProduct_Status_Code.Text, txtDispatch_Refered_2.Text, (hidIsMarketing.Text == "1" ? "412" : "111"));
                return;
            }


            int total = 0;
            // var apiResp = ProductClient.Get(_product_code, null, null, null, null, null).Result;
            ApiResponseMessage apiResp = DispatchClient.GetProductStockAllByCode(_product_code, txtDispatch_Refered_2.Text, new Guid(hidProduct_Status_Code.Text), (hidIsMarketing.Text == "1" ? "412" : "111")).Result;
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

                    if (!string.IsNullOrEmpty(cmbShippingTo.GetValue()))
                    {
                        ApiResponseMessage apiRespShipto = ShipToClient.GetByID(new Guid(cmbShippingTo.SelectedItem.Value)).Result;
                        ShippingTo datashipto = new ShippingTo();
                        if (apiRespShipto.ResponseCode == "0")
                        {
                            datashipto = apiRespShipto.Get<ShippingTo>();

                        }

                        if (datashipto.SpecialBookingRule != null)
                        {
                            hidSpecial_Rul_Name.Value = datashipto.SpecialBookingRule.RuleName;
                            hidSpecial_Rul_ID.Value = datashipto.SpecialBookingRule.RuleId;
                        }
                    }
                }
                else
                {

                    ucProductDispatchSelect.Show(_product_code, hidProduct_Status_Code.Text, txtDispatch_Refered_2.Text, (hidIsMarketing.Text == "1" ? "412" : "111"));
                }

            }


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
            // this.txtAddQty.MaxValue = (double)data.Quantity;
            if (!string.IsNullOrEmpty(cmbShippingTo.GetValue()))
            {
                ApiResponseMessage apiRespShipto = ShipToClient.GetByID(new Guid(cmbShippingTo.SelectedItem.Value)).Result;
                ShippingTo datashipto = new ShippingTo();
                if (apiRespShipto.ResponseCode == "0")
                {
                    datashipto = apiRespShipto.Get<ShippingTo>();

                }

                if (datashipto.SpecialBookingRule != null)
                {
                    hidSpecial_Rul_Name.Value = datashipto.SpecialBookingRule.RuleName;
                    hidSpecial_Rul_ID.Value = datashipto.SpecialBookingRule.RuleId;
                }
            }

            ucProductDispatchSelect.Close();
            txtAddQty.Focus(true, 300);
        }

        [DirectMethod(Timeout=180000)]
        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucProductDispatchSelect.ProductSelectBindData(action, extraParams);
        }

        [DirectMethod(Timeout=180000)]
        public object ValidateProdcut(string product_code, string product_sys_code)
        {

            ApiResponseMessage apiRespShipto = ProductClient.GetAllByID(new Guid(product_sys_code)).Result;
            Product datashipto = new Product();
            if (apiRespShipto.ResponseCode == "0")
            {
                datashipto = apiRespShipto.Get<Product>();

                if (datashipto.CodeCollection.Where(x => x.CodeType == ProductCodeTypeEnum.Stock).FirstOrDefault().Code != product_code)
                {
                    return new { valid = false, msg = "This Product Code Is Not Match With Product Name  " };
                }
            }

            return new { valid = true, msg = "" };
        }
        
        [DirectMethod(Timeout=180000)]
        public void DispatchTypeCombo()
        {
            ApiResponseMessage apiResp = ProductStatusClient.GetByDocumentTypeID(new Guid(cmbDispatchType.SelectedItem.Value)).Result;
            List<ProductStatus> data = new List<ProductStatus>();
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<ProductStatus>>();
                if (data != null)
                {
                    if (data.Count == 0)
                    {
                        MessageBoxExt.ShowError("สถานะสินค้ายังไม่ได้ ตั้งค่ากับ Dispatch Type นี้! กรุณาติดต่อผู้ดูแลระบบ");
                        btnSave.Hidden = true;
                        return;
                    }
                    else
                    {
                        btnSave.Hidden = false;
                    }


                    hidProduct_Status_Code.Text = data[0].ProductStatusID.ToString();
                    hidProduct_Status_Name.Text = data[0].Name;
                    if (data[0].ProductStatusMapCollection.Count != 0)
                    {
                        hidProduct_Sub_Status_Code.Text = data[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductSubStatus.ProductSubStatusID.ToString();
                        hidProduct_Sub_Status_Name.Text = data[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductSubStatus.Name.ToString();
                    }
                }

            }
            ApiResponseMessage apiRespItf = ItfInterfaceMappingClient.GetByDocument(new Guid(cmbDispatchType.SelectedItem.Value)).Result;
            List<ItfInterfaceMapping> dataItf = new List<ItfInterfaceMapping>();
            if (apiRespItf.ResponseCode == "0")
            {
                dataItf = apiRespItf.Get<List<ItfInterfaceMapping>>();
                hidIsMarketing.Text = (dataItf[0].IsMarketing != null ? (dataItf[0].IsMarketing == false ? "0" : "1") : "0");
                X.Js.Call("type_change");
                DefaultWarehouse();
                DefaltPOInternal();
            }



        }

        [DirectMethod(Timeout=180000)]
        public void ShipToCombo()
        {
            if (!string.IsNullOrEmpty(cmbShippingTo.GetValue()))
            {
                ApiResponseMessage apiRespShipto = ShipToClient.GetByID(new Guid(cmbShippingTo.SelectedItem.Value)).Result;
                ShippingTo datashipto = new ShippingTo();
                if (apiRespShipto.ResponseCode == "0")
                {
                    datashipto = apiRespShipto.Get<ShippingTo>();

                }

                if (datashipto.SpecialBookingRule != null)
                {
                    hidSpecial_Rul_Name.Value = datashipto.SpecialBookingRule.RuleName;
                    hidSpecial_Rul_ID.Value = datashipto.SpecialBookingRule.RuleId;
                }
            }
        }

        [DirectMethod(Timeout=180000)]
        public void txtRevisePoNo_Change()
        {

            try
            {

                if (txtRevisePoNo.Text.IndexOf("/") != -1)
                {
                    MessageBoxExt.ShowError("Format Po no incorect can't  input '/'", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('txtRevisePoNo').focus('', 10); ");

                    return;
                }

                // this.txtRevisePoNo.Text = "Testcomplete";
                ApiResponseMessage apiResp = DispatchClient.GetByPoNo(txtRevisePoNo.Text).Result;
                DispatchModels dataItem = new DispatchModels();
                if (apiResp.ResponseCode == "0")
                {
                    dataItem = apiResp.Get<DispatchModels>();
                    if (dataItem != null)
                    {

                        hidIsMarketing.Text = (dataItem.IsMarketing ? "1" : "0");
                        X.Js.Call("type_change");
                        txtRevisePonoId.Text = dataItem.DispatchId.ToString();
                        txtDispatch_Code.Text = dataItem.DispatchCode;
                        txtDispatch_Refered_1.Text = dataItem.Pono;
                        txtDispatch_Refered_2.Text = dataItem.OrderNo;


                        chkIsUrgent.Checked = dataItem.IsUrgent.Value;
                        chkIsBackorder.Checked = (dataItem.IsBackOrder == null ? false : dataItem.IsBackOrder.Value);
                        txtDispatch_Remark.Text = dataItem.Remark;

                        lblDispatchStatus.Text = dataItem.DispatchStatusName;
                        if (dataItem.OrderDate != null)
                        {
                            dtDispatch_Date_Order.SelectedDate = dataItem.OrderDate.Value;
                        }

                        if (dataItem.DocumentDate != null)
                        {
                            dtDocumentDate.SelectedDate = dataItem.DocumentDate.Value;
                        }

                        if (dataItem.DeliveryDate != null)
                        {
                            dtDeliveryDate.SelectedDate = dataItem.DeliveryDate.Value;
                        }

                        DocumentType document_edit = new DocumentType
                        {
                            Name = dataItem.DocumentName,
                            DocumentTypeID = dataItem.DocumentId.Value
                        };

                        StoreDispatch_Type.Add(document_edit);
                        cmbDispatchType.SelectedItem.Value = dataItem.DocumentId.Value.ToString();
                        cmbDispatchType.UpdateSelectedItems();
                        cmbDispatchType.ReadOnly = true;

                        ApiResponseMessage apiRespStatus = ProductStatusClient.GetByDocumentTypeID(new Guid(dataItem.DocumentId.Value.ToString())).Result;
                        List<ProductStatus> datastatus = new List<ProductStatus>();
                        if (apiRespStatus.ResponseCode == "0")
                        {
                            datastatus = apiRespStatus.Get<List<ProductStatus>>();

                            hidProduct_Status_Code.Text = datastatus[0].ProductStatusID.ToString();
                            hidProduct_Status_Name.Text = datastatus[0].ProductStatusMapCollection.Where(x => x.IsDefault).FirstOrDefault().ProductStatus.Name.ToString();

                            //X.Js.Call("type_change");
                        }




                        Contact customer_edit = new Contact
                        {
                            Name = dataItem.CustomerName,
                            ContactID = dataItem.CustomerId.Value,
                            Code = dataItem.CustomerCode,
                            FullName = dataItem.CustomerCode + ':' + dataItem.CustomerName,
                        };
                        StoreSubCust_Code.Add(customer_edit);
                        cmbSubCust_Code.SelectedItem.Value = dataItem.CustomerId.Value.ToString();
                        cmbSubCust_Code.UpdateSelectedItems();



                        ShippingTo shipto_edit = new ShippingTo
                        {
                            Name = dataItem.ShipptoName,
                            ShipToId = dataItem.ShipptoId.Value,
                        };
                        StoreShipTo.Add(shipto_edit);
                        cmbShippingTo.SelectedItem.Value = dataItem.ShipptoId.ToString();
                        cmbShippingTo.UpdateSelectedItems();
                        cmbShippingTo.ReadOnly = true;

                        if (dataItem.FromwarehouseId != null)
                        {

                            Warehouse fromwarehouse_edit = new Warehouse
                            {
                                Name = dataItem.FromwarehouseName,
                                WarehouseID = dataItem.FromwarehouseId.Value,
                            };

                            StoreWarehouseFrom.Add(fromwarehouse_edit);
                            cmbWarehouseFrom.SelectedItem.Value = dataItem.FromwarehouseId.ToString();
                            cmbWarehouseFrom.UpdateSelectedItems();
                        }

                        if (dataItem.TowarehouseId != null)
                        {

                            Warehouse Towarehouse_edit = new Warehouse
                            {
                                Name = dataItem.TowarehouseName,
                                WarehouseID = dataItem.TowarehouseId.Value,
                            };

                            StoreWarehouseTo.Add(Towarehouse_edit);
                            cmbWarehouseTo.SelectedItem.Value = dataItem.TowarehouseId.ToString();
                            cmbWarehouseTo.UpdateSelectedItems();
                        }


                        txtDispatch_Code.ReadOnly = true;
                        txtDispatch_Refered_1.ReadOnly = true;
                        txtDispatch_Refered_2.ReadOnly = true;
                        chkIsUrgent.Disabled = true;
                        chkIsBackorder.Disabled = true;
                        txtDispatch_Remark.ReadOnly = true;
                        dtDispatch_Date_Order.ReadOnly = true;
                        dtDocumentDate.ReadOnly = true;
                        dtDeliveryDate.ReadOnly = true;
                        cmbDispatchType.ReadOnly = true;
                        cmbSubCust_Code.ReadOnly = true;
                        cmbShippingTo.ReadOnly = true;
                        cmbWarehouseFrom.ReadOnly = true;
                        cmbWarehouseTo.ReadOnly = true;
                    }
                }
                else
                {
                    txtRevisePonoId.Reset();
                    txtRevisePoNo.Reset();
                    txtRevisePoNo.Focus();
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        [DirectMethod(Timeout=180000)]
        public void txtRevisePoNo_Clear()
        {
            txtRevisePoNo.Reset();
            txtDispatch_Code.Text = "new";
            ClearData();
            txtDispatch_Code.ReadOnly = false;
            txtDispatch_Refered_1.ReadOnly = false;
            txtDispatch_Refered_2.ReadOnly = false;
            chkIsUrgent.Disabled = false;
            chkIsBackorder.Disabled = false;
            txtDispatch_Remark.ReadOnly = false;
            dtDispatch_Date_Order.ReadOnly = false;
            dtDocumentDate.ReadOnly = false;
            dtDeliveryDate.ReadOnly = false;
            cmbDispatchType.ReadOnly = false;
            cmbSubCust_Code.ReadOnly = false;
            cmbShippingTo.ReadOnly = false;
            cmbWarehouseFrom.ReadOnly = false;
            cmbWarehouseTo.ReadOnly = false;
        }

        private void ClearData()
        {
            chkIsBackorder.Reset();
            cmbDispatchType.Reset();
            cmbSubCust_Code.Reset();
            FormPanelDetail.Reset();
            StoreDispatch.RemoveAll();
            cmbDispatchType.ReadOnly = false;

            if (txtDispatch_Code.Text == "new")
            {
                cmbDispatchType.Focus(false, 100);
            }

            BindData(Request.QueryString["oDataKeyId"]);
        }

        // public void save
        private bool ValidationHeader()
        {
            if (string.IsNullOrWhiteSpace(cmbDispatchType.GetValue()))
            {
                MessageBoxExt.ShowError("กรุณาเลือก Dispatch Type", MessageBox.Button.OK,
                Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbDispatchType').focus('', 10); ");
                return false;
            }
            else
            {
                bool isValid = Guid.TryParse(cmbDispatchType.GetValue(), out Guid newGuid);
                if (isValid == false)
                {

                    MessageBoxExt.ShowError("กรุณาเลือก Dispatch Type ให้ถูกต้อง", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbDispatchType').focus('', 10); ");
                    return false;
                }
            }


            if (string.IsNullOrWhiteSpace(cmbSubCust_Code.GetValue()))
            {

                MessageBoxExt.ShowError("กรุณาเลือก Customer", MessageBox.Button.OK,
               Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbSubCust_Code').focus('', 10); ");
                return false;

            }
            else
            {
                bool isValid = Guid.TryParse(cmbSubCust_Code.GetValue(), out Guid newGuid);
                if (isValid == false)
                {

                    MessageBoxExt.ShowError("กรุณาเลือก Customer ให้ถูกต้อง", MessageBox.Button.OK,
                   Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbSubCust_Code').focus('', 10); ");
                    return false;
                }
            }

            if (dtDispatch_Date_Order.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("กรุณาเลือก Est.Dispatch Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtDispatch_Date_Order').focus('', 10); ");
                return false;
            }

            if (dtDeliveryDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("กรุณาเลือก Delivery Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtDeliveryDate').focus('', 10); ");
                return false;
            }

            if (dtDocumentDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("กรุณาเลือก Document Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtDocumentDate').focus('', 10); ");
                return false;
            }



            if (string.IsNullOrWhiteSpace(txtDispatch_Refered_1.Text))
            {
                MessageBoxExt.ShowError("กรุณากรอก Po no", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('txtDispatch_Refered_1').focus('', 10); ");

                return false;
            }


            if (txtDispatch_Refered_1.Text.IndexOf("/") != -1 && !ckbRevisePoNo.Checked && txtDispatch_Code.Text == "new")
            {
                MessageBoxExt.ShowError("Format Po no incorect can't  input '/'", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('txtDispatch_Refered_1').focus('', 10); ");

                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbShippingTo.GetValue()))
            {
                MessageBoxExt.ShowError("กรุณาเลือก Shipping To", MessageBox.Button.OK,
                Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbShippingTo').focus('', 10); ");

            }
            else
            {
                bool isValid = Guid.TryParse(cmbShippingTo.GetValue(), out Guid newGuid);
                if (isValid == false)
                {
                    MessageBoxExt.ShowError("กรุณาเลือก Shipping To ให้ถูกต้อง", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbShippingTo').focus('', 10); ");
                    return false;
                }
            }

            if (hidIsMarketing.Text == "1")
            {
                if (string.IsNullOrWhiteSpace(cmbWarehouseFrom.GetValue()))
                {
                    MessageBoxExt.ShowError("กรุณาเลือก Warehouse From", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseFrom').focus('', 10); ");
                    return false;
                }
                else
                {
                    bool isValid = Guid.TryParse(cmbWarehouseFrom.GetValue(), out Guid newGuid);
                    if (isValid == false)
                    {
                        MessageBoxExt.ShowError("กรุณาเลือก Warehouse From ให้ถูกต้อง", MessageBox.Button.OK,
                            Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseFrom').focus('', 10); ");
                        return false;

                    }
                }

                if (string.IsNullOrWhiteSpace(cmbWarehouseTo.GetValue()))
                {
                    MessageBoxExt.ShowError("กรุณาเลือก Warehouse To", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseTo').focus('', 10); ");
                    return false;
                }
                else
                {
                    bool isValid = Guid.TryParse(cmbWarehouseFrom.GetValue(), out Guid newGuid);
                    if (isValid == false)
                    {
                        MessageBoxExt.ShowError("กรุณาเลือก Warehouse To ให้ถูกต้อง", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseTo').focus('', 10); ");
                        return false;
                    }
                }

                if (cmbWarehouseFrom.GetValue() == cmbWarehouseTo.GetValue())
                {
                    MessageBoxExt.ShowError("กรุณาเลือก From Warehouse จะต้องไม่เท่ากับ To Warehouse", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseFrom').focus('', 10); ");
                    return false;
                }

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(cmbWarehouseFrom.GetValue()))
                {
                    bool isValid = Guid.TryParse(cmbWarehouseFrom.GetValue(), out Guid newGuid);
                    if (isValid == false)
                    {
                        MessageBoxExt.ShowError("กรุณาเลือก Warehouse From  ให้ถูกต้อง", MessageBox.Button.OK,
                            Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseFrom').focus('', 10); ");
                        return false;

                    }
                }

                if (!string.IsNullOrWhiteSpace(cmbWarehouseTo.GetValue()))
                {
                    bool isValid = Guid.TryParse(cmbWarehouseTo.GetValue(), out Guid newGuid);
                    if (isValid == false)
                    {
                        MessageBoxExt.ShowError("กรุณาเลือก Warehouse To ให้ถูกต้อง", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseTo').focus('', 10); ");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckBackOrder()
        {

            string _msg = "ยังไม่สามารถยืนยันได้ มีสินค้าที่ยังติด Back Order";
            if (chkIsBackorder.Checked)
            {
                MessageBoxExt.ShowError(_msg);
            }

            return chkIsBackorder.Checked;
        }
        private void getProducts(string _dispatchid)
        {
            string AutoCompleteService = "../../../Common/DataClients/OptDataHandler.ashx";
            string WarehouseID = cmbWarehouseName.SelectedItem.Value;


            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "PalletBooking" },
                { "WarehouseID", WarehouseID },
                { "Product", txtProduct.Text },
                { "Pallet", txtPallet.Text },
                { "Lot", txtLot.Text },
                { "OrderNo", txtOrderNo.Text },
                { "DispatchID", new Guid(_dispatchid) }
            };

            StoreStockBalance.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            StoreStockBalance.AutoCompleteProxy(AutoCompleteService, param);
            StoreStockBalance.LoadProxy();
        }

        #endregion

        #region Event Handle

        [DirectMethod(Timeout = 180000)]
        public void btnApproveDispatch_Click(object sender, DirectEventArgs e)
        {
            if (!CheckBackOrder())
            {
                DoApproveDispatchConfirm();
            }

        }
        [DirectMethod(Timeout = 180000)]
        public void btnApproveDispatchInternal_Click(object sender, DirectEventArgs e)
        {
            try
            {
                if (dtApproveDispatch.Text == DateTime.MinValue.ToString())
                {
                    MessageBoxExt.ShowError("กรุณากรอก Approve Date", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");

                    return;
                }
                Core.Domain.ApiResponseMessage apiresponse = ClientService.Master.MonthEndClient.CheckCutoffDate(dtApproveDispatch.SelectedDate).Result;
                if (apiresponse.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");
                    return;
                }

                bool isSuccess = true;
                TimeSpan ts = new TimeSpan(0, DateTime.Now.Hour, DateTime.Now.Minute, 0, 0);
                ApiResponseMessage apiRespStatus = DispatchClient.OnApproveDispatchInternal(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, (hidIsMarketing.Text == "1" ? "412" : "111"), dtApproveDispatch.SelectedDate.Add(ts)).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.ApproveDispatchReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        [DirectMethod(Timeout = 180000)]
        public void btnApproveDispatchQA_Click(object sender, DirectEventArgs e)
        {
            try
            {
                DoApproveDispatchPickingConfirm();

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        [DirectMethod(Timeout = 180000)]
        public void btnApproveDispatchPicking_Click(object sender, DirectEventArgs e)
        {
            try
            {
                if (!CheckBackOrder())
                {
                    DoApproveDispatchPickingConfirm();
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        [DirectMethod(Timeout = 180000)]
        public void btnCancelDispatch_Click(object sender, DirectEventArgs e)
        {
            try
            {

                bool isSuccess = true;

                ApiResponseMessage apiRespStatus = DispatchClient.OnCancelDispatch(txtDispatch_Code.Text, txtDispatch_Refered_1.Text).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.CancelDispatchReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        [DirectMethod(Timeout = 180000)]
        public void btnCancelDispatchInternal_Click(object sender, DirectEventArgs e)
        {
            try
            {

                bool isSuccess = true;

                ApiResponseMessage apiRespStatus = DispatchClient.OnCancelDispatchInternal(txtDispatch_Code.Text, txtDispatch_Refered_1.Text).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.CancelDispatchReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        [DirectMethod(Timeout = 180000)]
        public void btnCancelDispatchPicking_Click(object sender, DirectEventArgs e)
        {
            try
            {

                bool isSuccess = true;

                ApiResponseMessage apiRespStatus = DispatchClient.OnCancelDispatchPicking(txtDispatch_Code.Text, txtDispatch_Refered_1.Text).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.CancelDispatchReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        [DirectMethod(Timeout = 180000)]
        public void btnBooking_Click(object sender, DirectEventArgs e)
        {
            DoConfirm();
        }
        [DirectMethod(Timeout = 180000)]
        public void btnApprove_Click(object sender, DirectEventArgs e)
        {
            try
            {
                bool isSuccess = true;

                ApiResponseMessage apiRespStatus = DispatchClient.OnApproveBooking(txtDispatch_Code.Text, txtDispatch_Refered_1.Text, (hidIsMarketing.Text == "1" ? "412" : "111")).Result;
                List<string> datastatus = new List<string>();
                if (apiRespStatus.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.ApproveReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        [DirectMethod(Timeout = 180000)]
        public void btnCancel_Click(object sender, DirectEventArgs e)
        {
            try
            {
                ApiResponseMessage apiRespStatus = DispatchClient.OnCancel(txtDispatch_Code.Text, txtDispatch_Refered_1.Text).Result;
                if (apiRespStatus.ResponseCode == "0")
                {
                    X.Call("parent.App.direct.CancelReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
                else
                {
                    MessageBoxExt.ShowError(apiRespStatus.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        [DirectMethod(Timeout = 180000)]
        public void btnCancelAll_Click(object sender, DirectEventArgs e)
        {
            DoCancelDispatchConfirm();
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnSelectList_Click(object sender, DirectEventArgs e)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            string strTitle = GetResource("EDIT") + " " + GetResource("DISPATCH");
            WindowShow.ShowNewPage(this, strTitle, "CreateDispatach", "frmDispatch.aspx?IsPopup=1", iconWindows);
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnPrint_Click(object sender, DirectEventArgs e)
        {
            X.Call("popitup", $"../../Report/frmReportViewer.aspx?reportName=RPT_DispatchForm&DispatchCode={txtDispatch_Code.Text}&PONo={txtDispatch_Refered_1.Text}");

        }
        [DirectMethod(Timeout = 180000)]
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                if (!ValidationHeader())
                {
                    return;
                }
                string gridJson;

                gridJson = e.ExtraParams["ParamStorePages"];


                Dispatch saveModel = new Dispatch();

                List<DispatchDetailModels> gridData = JSON.Deserialize<List<DispatchDetailModels>>(gridJson);


                if (gridData.Count == 0)
                {
                    MessageBoxExt.ShowError("กรุณาเพิ่มรายการสินค้าก่อน", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('btnAddItem').focus('', 10); ");

                    return;
                }


                #region [ Get Data to Model ]
                //Header Template Name

                string id = Request.QueryString["oDataKeyId"];

                if (id != "new" && ckbRevisePoNo.Checked == false)
                {
                    saveModel.DispatchId = new Guid(id);
                    saveModel.DispatchStatus = (DispatchStatusEnum)int.Parse(txtDispatchStatusId.Text);
                }
                else
                {
                    saveModel.DispatchId = Guid.NewGuid();
                    saveModel.DispatchStatus = DispatchStatusEnum.New;
                    if (ckbRevisePoNo.Checked)
                    {
                        saveModel.ReferenceId = new Guid(txtRevisePonoId.Text);
                    }
                }


                saveModel.DispatchCode = txtDispatch_Code.Text;
                saveModel.DocumentId = new Guid(cmbDispatchType.SelectedItem.Value);
                saveModel.CustomerId = new Guid(cmbSubCust_Code.SelectedItem.Value);
                saveModel.Remark = txtDispatch_Remark.Text;
                saveModel.OrderDate = dtDispatch_Date_Order.SelectedDate;
                saveModel.DocumentDate = dtDocumentDate.SelectedDate;
                saveModel.DeliveryDate = dtDeliveryDate.SelectedDate;
                saveModel.ShipptoId = new Guid(cmbShippingTo.SelectedItem.Value);

                if (!string.IsNullOrEmpty(cmbWarehouseFrom.GetValue()))
                {
                    saveModel.FromwarehouseId = new Guid(cmbWarehouseFrom.SelectedItem.Value);
                }

                if (!string.IsNullOrEmpty(cmbWarehouseTo.GetValue()))
                {
                    saveModel.TowarehouseId = new Guid(cmbWarehouseTo.SelectedItem.Value);
                }

                saveModel.IsUrgent = chkIsUrgent.Checked;
                saveModel.IsBackOrder = chkIsBackorder.Checked;
                saveModel.Pono = txtDispatch_Refered_1.Text;
                saveModel.OrderNo = txtDispatch_Refered_2.Text;
                saveModel.IsActive = true;

                #region [ Detail Model ]
                saveModel.DispatchDetailCollection = new List<DispatchDetail>();

                DispatchDetail itemModel;
                int seq = 1;
                foreach (DispatchDetailModels item in gridData)
                {
                    itemModel = new DispatchDetail
                    {
                        DispatchId = saveModel.DispatchId
                    };

                    if (item.DispatchDetailId != null)
                    {
                        itemModel.DispatchDetailId = item.DispatchDetailId.Value;
                        itemModel.DispatchDetailStatus =(DispatchDetailStatusEnum) item.DispatchDetailStatusId.Value;
                    }
                    else
                    {
                        itemModel.DispatchDetailId = Guid.Empty;
                        itemModel.DispatchDetailStatus = DispatchDetailStatusEnum.New;
                    }
                    itemModel.ProductId = item.ProductId;
                    itemModel.Sequence = seq;
                    itemModel.StockUnitId = item.StockUnitId;
                    itemModel.Quantity = item.Quantity;
                    itemModel.ConversionQty = item.ConversionQty;
                    itemModel.DispatchDetailProductHeight = item.DispatchDetailProductHeight;
                    itemModel.DispatchDetailProductLength = item.DispatchDetailProductLength;
                    itemModel.DispatchDetailProductWidth = item.DispatchDetailProductWidth;
                    itemModel.DispatchPriceUnitId = item.DispatchPriceUnitId;
                    itemModel.DispatchPrice = item.DispatchPrice;
                    itemModel.BaseQuantity = itemModel.ConversionQty * itemModel.Quantity;
                    itemModel.BaseUnitId = item.BaseUnitId;
                    itemModel.Remark = item.Remark;
                    itemModel.RuleId = item.RuleId;
                    itemModel.ProductStatusId = item.ProductStatusId;
                    itemModel.ProductSubStatusId = item.ProductSubStatusId;
                    itemModel.ProductOwnerId = item.ProductOwnerId.Value;
                    itemModel.IsActive = true;
                    seq++;
                    saveModel.DispatchDetailCollection.Add(itemModel);
                }

                #endregion [ Detail Model ]

                #endregion [ Get Data to Model ]


                bool isSuccess = true;

                if (id == "new")
                {
                    ApiResponseMessage datasave = DispatchClient.Add(saveModel).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                        DefaltPOInternal();
                    }
                }
                else
                {
                    ApiResponseMessage datamodify = DispatchClient.Modify(saveModel).Result;
                    if (datamodify.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datamodify.ResponseMessage);
                    }
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.SaveReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        [DirectMethod(Timeout = 180000)]
        protected void btnSaveApproveDate_Click(object sender, DirectEventArgs e)
        {
            try
            {

                if (dtApproveDispatch.Text == DateTime.MinValue.ToString())
                {
                    MessageBoxExt.ShowError("Please input approve date", MessageBox.Button.OK,
                        Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDispatch').focus('', 10); ");

                    return;
                }
                ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(dtApproveDispatch.SelectedDate).Result;
                if (apiresponse.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(GetMessage("MEND005").MessageValue);
                    return;
                }

                Dispatch saveModel = new Dispatch();
                string id = Request.QueryString["oDataKeyId"];
                saveModel.DispatchId = new Guid(id);
                saveModel.DispatchStatus =(DispatchStatusEnum) int.Parse(txtDispatchStatusId.Text);
                saveModel.DocumentApproveDate = dtApproveDispatch.SelectedDate;
                saveModel.DispatchCode = txtDispatch_Code.Text;
                saveModel.DocumentId = new Guid(cmbDispatchType.SelectedItem.Value);
                saveModel.CustomerId = new Guid(cmbSubCust_Code.SelectedItem.Value);
                saveModel.Remark = txtDispatch_Remark.Text;
                saveModel.OrderDate = dtDispatch_Date_Order.SelectedDate;
                saveModel.DocumentDate = dtDocumentDate.SelectedDate;
                saveModel.DeliveryDate = dtDeliveryDate.SelectedDate;
                saveModel.ShipptoId = new Guid(cmbShippingTo.SelectedItem.Value);

                if (!string.IsNullOrEmpty(cmbWarehouseFrom.GetValue()))
                {
                    saveModel.FromwarehouseId = new Guid(cmbWarehouseFrom.SelectedItem.Value);
                }

                if (!string.IsNullOrEmpty(cmbWarehouseTo.GetValue()))
                {
                    saveModel.TowarehouseId = new Guid(cmbWarehouseTo.SelectedItem.Value);
                }

                saveModel.IsUrgent = chkIsUrgent.Checked;
                saveModel.IsBackOrder = chkIsBackorder.Checked;
                saveModel.Pono = txtDispatch_Refered_1.Text;
                saveModel.OrderNo = txtDispatch_Refered_2.Text;
                saveModel.IsActive = true;
                saveModel.DispatchDetailCollection = new List<DispatchDetail>();

                bool isSuccess = true;

                ApiResponseMessage datamodify = DispatchClient.ModifyHeader(saveModel).Result;
                if (datamodify.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datamodify.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.SaveReload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        [DirectMethod(Timeout = 180000)]
        protected void btnClose_Click(object sender, DirectEventArgs e)
        {
            X.Call("parentAutoLoadControl.close()");
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnClear_Click(object sender, DirectEventArgs e)
        {
            ClearData();
        }
        [DirectMethod(Timeout = 180000)]
        public void btnConfirmDo_Click(object sender, DirectEventArgs e)
        {
            //try
            //{
            //    dataService = new DataServiceModel();
            //    dataService.Add<string>("Dispatch_Code", this.txtDispatch_Code.Text);
            //    dataService.Add<DateTime>("Dispatch_Date_Confirm", this.dtConfirmDo.SelectedDate);
            //    dataService.Add<string>("Dispatch_CloseJob_Reason", this.txtDispatch_Code.Text);

            //    var result = WebServiceHelper.Post<Results>("OnConfirmDo", dataService.GetObject());

            //    if (result.result)
            //    {
            //        X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage(result.message));
            //        X.Call("parentAutoLoadControl.close()");
            //    }
            //    else
            //    {
            //        MessageBoxExt.Show("Warning", result.message);
            //    }
            //}
            //catch
            //{
            //    MessageBoxExt.Show("Warning", "Booking not complete.");
            //}
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnAddPallet_Click(object sender, DirectEventArgs e)
        {
            string _dispatchid = Request.QueryString["oDataKeyId"];
            if (_dispatchid != "new")
            {
                getProducts(_dispatchid);
            }
            WindowDataDetail.Show();
            //this.PagingToolbar2.MoveFirst();
        }

        [DirectMethod(Timeout = 180000)]
        protected void btnSubCust_Code_Click(object sender, EventArgs e)
        {
            Icon iconWindows = Icon.ApplicationFormAdd;
            string strTitle = GetResource("ADD_NEW") + " " + GetResource("CONTACT");
            WindowShow.Show(this, strTitle, "CreateContact", "~/apps/master/AddEdit/frmCreateContacts.aspx?oDataKeyId=new&IsPopup=1", iconWindows, 700, 500);
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnAddDispatchType_Click(object sender, EventArgs e)
        {

            Icon iconWindows = Icon.ApplicationFormAdd;
            string strTitle = GetResource("ADD_NEW") + " " + GetResource("DOCUMENTTYPE");
            WindowShow.Show(this, strTitle, "CreateDocument", "~/apps/master/AddEdit/frmCreateDocument.aspx?oDataKeyId=new&IsPopup=1", iconWindows, 700, 500);

        }
        [DirectMethod(Timeout = 180000)]
        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            if (command == "Delete")
            {
                string unitId = e.ExtraParams["unitId"];
                X.Call("deleteProduct('" + oDataKeyId + "','" + unitId + "')");

            }
            else if (command == "Revise")
            {
                // X.Call("deleterevise('" + oDataKeyId + "')");
                ApiResponseMessage ok = ClientService.Outbound.DispatchClient.RemoveBookingAdjustToBackOrder(new Guid(oDataKeyId)).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);


                }
                else
                {
                    MessageBoxExt.ShowError(ok.ResponseMessage);
                }

                BindData(Request.QueryString["oDataKeyId"]);

                //var _status = Request.QueryString["oDataStatus"];

                //var dataItem = new DispatchModels();
                //ApiResponseMessage apiResp = new ApiResponseMessage();
                //apiResp = DispatchClient.GetBookingByID(new Guid(_DispatchId)).Result;
                //if (apiResp.ResponseCode == "0")
                //{
                //    dataItem = apiResp.Get<DispatchModels>();
                //    StoreDispatch.DataSource = dataItem.DispatchDetailModelsCollection;
                //    StoreDispatch.DataBind();
                //}

            }
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnSearchProduct_Click(object sender, DirectEventArgs e)
        {
            string _dispatchid = Request.QueryString["oDataKeyId"];
            if (_dispatchid != "new")
            {
                getProducts(_dispatchid);
            }
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnSelectItem_Click(object sender, DirectEventArgs e)
        {
            string dispatchId = Request.QueryString["oDataKeyId"];
            List<PalletModel> product = JSON.Deserialize<List<PalletModel>>(e.ExtraParams["StoreStockBalance"]);

            if (product == null || product.Count == 0)
            {
                NotificationExt.Show(GetMessage("MSG00081").MessageTitle, GetMessage("MSG00081").MessageValue);
                //MessageBoxExt.ShowError(ok.ResponseMessage);
                return;
            }
            string pallets = string.Join(",", product.Select(x => x.PalletCode));


            ApiResponseMessage ok = ClientService.Outbound.DispatchClient.ManualBooking(new Guid(dispatchId), pallets).Result;

            if (ok.ResponseCode == "0")
            {

                X.AddScript("App.GridGetWindows.getSelectionModel().deselectAll();");
                WindowDataDetail.Hide();
                BindData(dispatchId, true);
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }


        #endregion 
    }
}