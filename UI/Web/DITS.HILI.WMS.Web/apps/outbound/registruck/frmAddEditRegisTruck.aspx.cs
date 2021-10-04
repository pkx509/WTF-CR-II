using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.RegisterTruckModel;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.outbound.registruck
{
    public partial class frmAddEditRegisTruck : BaseUIPage
    {

        private readonly string AppDataService = "../../../Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack && DynamicMenu.ToolbarControlOperation(ProgramCode, btnSave, null, null, null, null, null, null, null, null, null))
            //{
            //    BindData(Request.QueryString["oDataKeyId"]);
            //    DynamicMenu.ToolbarControl(ProgramCode, null, btnSave, null);
            //}

            if (!IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }

            // SetButton("Edit");
        }
        [DirectMethod(Timeout = 180000)]
        protected void Assign_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<RegisterTruckDetailModel> gridData = JSON.Deserialize<List<RegisterTruckDetailModel>>(gridJson);

            if (!Is_Validation_Save(gridData))
            {
                return;
            }

            RegisTruckModel regisTruck = GetDataModel();
            regisTruck.ShippingID = new Guid(txtShippingID.Text);
            regisTruck.ShippingStatus = (int)ShippingStatusEnum.Assign;
            regisTruck.RegisTruckDetail = gridData.ToList();

            bool isSuccess = true;
            ApiResponseMessage datasave = new ApiResponseMessage();
            if (txtShipping_Code.Text != "new")
            {
                datasave = ClientService.Outbound.RegisterTruckClient.AssignModify(regisTruck).Result;
                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }
            }

            if (isSuccess)
            {
                X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(datasave.ResponseMessage == string.Empty ? GetMessage("MSG00004").MessageValue : datasave.ResponseMessage);
            }

        }
        [DirectMethod(Timeout = 180000)]
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {

            string gridJson = e.ExtraParams["ParamStorePages"];
            List<RegisterTruckDetailModel> gridData = JSON.Deserialize<List<RegisterTruckDetailModel>>(gridJson);

            if (!Is_Validation_Save(gridData))
            {
                return;
            }

            RegisTruckModel regisTruck = GetDataModel();

            regisTruck.RegisTruckDetail = gridData.Where(x => x.ConfirmQuantity > 0).ToList();

            bool isSuccess = true;
            ApiResponseMessage datasave = new ApiResponseMessage();
            if (txtShipping_Code.Text == "new")
            {
                datasave = ClientService.Outbound.RegisterTruckClient.Add(regisTruck).Result;

                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }
            }
            else
            {
                datasave = ClientService.Outbound.RegisterTruckClient.Modify(regisTruck).Result;
                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }
            }

            if (isSuccess)
            {
                X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(datasave.ResponseMessage == string.Empty ? GetMessage("MSG00004").MessageValue : datasave.ResponseMessage);
            }
        }

        [DirectMethod(Timeout = 180000)]
        protected void btnConsolidateList_Click(object sender, DirectEventArgs e)
        {

            #region Obsolet
            //List<DataKeyValue> listParameter = new List<DataKeyValue>();

            //listParameter.Add(new DataKeyValue()
            //{
            //    Key = "PONo",
            //    Value = txtPONo.Text,
            //});

            //listParameter.Add(new DataKeyValue()
            //{
            //    Key = "OrderNo",
            //    Value = txtOrderNo.Text,
            //});

            //listParameter.Add(new DataKeyValue()
            //{
            //    Key = "Dock",
            //    Value = cmbDock.SelectedItem.Text,
            //});

            //listParameter.Add(new DataKeyValue()
            //{
            //    Key = "LicensePlate",
            //    Value = txtTruckNo.Text,
            //});

            //listParameter.Add(new DataKeyValue()
            //{
            //    Key = "SealNo",
            //    Value = txtSeal_No.Text,
            //});

            //listParameter.Add(new DataKeyValue()
            //{
            //    Key = "ShipName",
            //    Value = txtCompany.Text,
            //});

            //Session["Report_ConsolidateList_Parameter"] = listParameter;

            //if (Session["Report_ConsolidateList_Parameter"] != null)
            //{
            //    if (_RegisTruckModel != null)
            //    {
            //        Session["Report_ConsolidateList"] = _RegisTruckModel;

            //        X.Js.Call("parent.parent.openPrintWindow", "reportid",
            //        "Report/frmReportViewer.aspx?rpt=Report_ConsolidateList",
            //        "Consolidate DO Report", "icon-report", 1000, 600);
            //    }
            //    else
            //    {
            //        MessageBoxExt.ShowError(Resources.Langauge.NotFoundData);
            //    }
            //}
            #endregion



            //X.Js.Call("parent.parent.openPrintWindow", "reportid",
            //        "Report/frmReportViewerSSRS.aspx?reportName=RPT_ConsolidateList&Shipping_Code=" + txtShipping_Code.Text,
            //        "Consolidate DO List", "icon-report", 1000, 600);



        }

        protected void OnSelect_DataGrids(object sender, DirectEventArgs e)
        {
            //RowSelectionModel sm = this.gridData.GetSelectionModel() as RowSelectionModel;

            //string gridJson = e.ExtraParams["ParamStorePages"];
            //List<RegisTruckDetailModel> gridData = JSON.Deserialize<List<RegisTruckDetailModel>>(gridJson);
            //List<RegisTruckDetailModel> _selectObj = new List<RegisTruckDetailModel>();
            //foreach (SelectedRow row in sm.SelectedRows)
            //{
            //    var objSelect = gridData.Where(x => x.DispatchDetail_ID.ToString() == row.RecordID).FirstOrDefault();
            //    if (objSelect != null)
            //    {
            //        _selectObj.Add(objSelect);
            //    }
            //}

            //if (_selectObj.Count > 0)
            //{
            //    btnDeleteAll.Disabled = false;
            //}
            //else
            //{
            //    btnDeleteAll.Disabled = true;
            //}

        }

        private void SetButton(string code)
        {
            switch (code)
            {
                case "Add":
                    btnSave.Disable();
                    break;
                case "Edit":
                    btnSave.Enable();
                    break;
                default: break;
            }
        }
        [DirectMethod(Timeout = 180000)]
        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            //RowSelectionModel sm = this.gridData.GetSelectionModel() as RowSelectionModel;

            //string gridJson = e.ExtraParams["ParamStorePages"];
            //List<RegisTruckDetailModel> gridData = JSON.Deserialize<List<RegisTruckDetailModel>>(gridJson);
            //List<RegisTruckDetailModel> _selectDetail = new List<RegisTruckDetailModel>();
            //foreach (SelectedRow row in sm.SelectedRows)
            //{
            //    var objSelect = gridData.Where(x => x.DispatchDetail_ID.ToString() == row.RecordID).FirstOrDefault();
            //    if (objSelect != null)
            //    {
            //        _selectDetail.Add(objSelect);
            //    }
            //}

            //if (_selectDetail.Count == 0)
            //{
            //    MessageBoxExt.Warning("Please Select Item Detail.");
            //}


            //StoreData.Data = gridData.Where(w => !_selectDetail.Contains(w));
            //StoreData.DataBind();

            //DataServiceModel dataService = new DataServiceModel();
            //dataService.Add<List<RegisTruckDetailModel>>("RegisTruckDetailModel", _selectDetail);

            //Results res = WebServiceHelper.Post<Results>("DeleteAll_Shipping", dataService.GetObject());

            //if (res.result)
            //{
            //    NotificationExt.Show("Delete", "Delete Complete");
            //    BindData(txtSeal_No.Text);
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(res.message);
            //}
        }

        protected void OrderType_SeleteChange(object sender, EventArgs e)
        {
            if (cmbRegisterType.GetValue() == "Export")
            {
                txtOrderNo.AllowBlank = false;
                txtContainerNo.AllowBlank = false;
                txtSeal_No.AllowBlank = false;
                txtBookingNo.AllowBlank = false;
            }
            else
            {
                txtOrderNo.AllowBlank = true;
                txtContainerNo.AllowBlank = true;
                txtSeal_No.AllowBlank = true;
                txtBookingNo.AllowBlank = true;
            }

            txtOrderNo.Reset();
            txtContainerNo.Reset();
            txtSeal_No.Reset();
            txtBookingNo.Reset();
            //btnSave
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string gridJson = e.ExtraParams["ParamStorePages"];

            List<RegisterTruckDetailModel> gridDataList = JSON.Deserialize<List<RegisterTruckDetailModel>>(gridJson);
            RegisterTruckDetailModel girdData = gridDataList.Where(x => x.BookingID == new Guid(oDataKeyId)).SingleOrDefault();
            gridDataList.Remove(girdData);

            StoreData.Data = gridDataList;
            StoreData.DataBind();
        }

        #region DirectMethod

        [DirectMethod(Timeout=180000)]
        public void LoadComboDock()
        {
            string strSearch = cmbTruckType.SelectedItem.Value;
            string wh = cmbWarehouseName.SelectedItem.Value;
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "TruckType" },
                { "TruckTypeId", cmbTruckType.SelectedItem.Value }
            };
            StoreDock.AutoCompleteProxy(AppDataService, param);

        }

        [DirectMethod(Timeout=180000)]
        public object DispatchSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucDispatchSelect.DispatchSelectBindData(action, extraParams, "Get_DispatchForRegisTruck");
        }

        [DirectMethod(Timeout=180000)]
        public void ucDispatch_Select(string record)
        {
            DispatchAllModel data = JSON.Deserialize<DispatchAllModel>(record);

            txtSearchPO.Text = data.PoNo;

            DispatchAllModel dispatchList = GetDispatchList(data.WarehouseID);
            DispatchAllModel dispatch = dispatchList;

            if (dispatch != null)
            {
                BindDatatoControl(dispatch);
            }
            else
            {
                MessageBoxExt.Warning("PO not found");
            }

            ucDispatchSelect.Close();
        }

        [DirectMethod(Timeout=180000)]
        public void btnBrowsePO_Click()
        {
            List<DispatchAllModel> listData = GetBrowseDispatchList();

            if (listData.Count == 1)
            {
                DispatchAllModel dispatch = listData.FirstOrDefault();
                DispatchAllModel dispatchList = GetDispatchList(dispatch.WarehouseID, dispatch.PoNo);
                BindDatatoControl(dispatchList);
            }
            else
            {
                ucDispatchSelect.Show(txtSearchPO.Text);
            }
        }

        #endregion

        #region Private Method

        //private void LoadProduct(bool isNew, List<RegisTruckDetailModel> items)
        //{
        //    txtOrderNo.Text = "";
        //    //tring strSearch = cmbDispatchCode.Text;
        //    DataServiceModel dataService = new DataServiceModel();

        //    var data = WebServiceHelper.Post<List<RegisTruckDetailModel>>("Get_Dispatch_Shipping", dataService.GetObject());

        //    if (data != null)
        //    {
        //        if (data.Count > 0)
        //        {
        //            txtOrderNo.Text = data[0].OrderNo;
        //        }

        //        if (items != null)
        //        {
        //            var query = from c in data where !(from o in items select o.DispatchDetail_ID).Contains(c.DispatchDetail_ID) select c;
        //            items.AddRange(query);
        //        }
        //        else
        //            items = data;
        //    }
        //    StoreData.Data = items == null ? new List<RegisTruckDetailModel>() : items;
        //    StoreData.DataBind();
        //}

        private bool Is_Validation_Save(List<RegisterTruckDetailModel> gridData = null)
        {
            if (gridData != null)
            {
                if (gridData.ToList().Sum(s => s.ConfirmQuantity) == 0)
                {
                    MessageBoxExt.ShowError("delivery Qty cannot be 0", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                    return false;
                }
                else if (gridData.Count() == 0)
                {
                    MessageBoxExt.ShowError("no item to register", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                    return false;
                }
            }
            else
            {
                MessageBoxExt.ShowError("no item to register", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return false;
            }

            if (string.IsNullOrEmpty(txtTruckNo.Text))
            {
                MessageBoxExt.ShowError("กรุณากรอกทะเบียนรถ", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('txtTruckNo').focus('', 10); ");
                return false;
            }

            if (string.IsNullOrEmpty(txtDocNo.Text))
            {
                MessageBoxExt.ShowError("Please Insert Document No", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('txtDocNo').focus('', 10); ");
                return false;
            }

            if (txtDocNo.Text == txtTruckNo.Text)
            {
                MessageBoxExt.ShowError("Please Check Truck No and Document No ", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('txtDocNo').focus('', 10); ");
                return false;
            }

            return true;
        }

        private RegisTruckModel GetDataModel()
        {
            RegisTruckModel item = new RegisTruckModel
            {
                ShippingCode = txtShipping_Code.Text,
                DocumentDate = dtDate.SelectedDate,
                RegisterTypeID = cmbRegisterType.SelectedItem.Text == "Export" ? 0 : 1,
                TruckType = cmbTruckType.GetValue(),
                DockTypeID = new Guid(cmbDock.GetValue()),
                WarehouseName = cmbWarehouseName.GetValue(),
                TruckTypeID = new Guid(cmbTruckType.GetValue()),
                ShippingTruckNo = txtTruckNo.Text,
                DriverName = txtDriverName.Text,
                LogisticCompany = txtCompany.Text,
                OrderNo = txtOrderNo.Text,
                Container_No = txtContainerNo.Text,
                SealNo = txtSeal_No.Text,
                BookingNo = txtBookingNo.Text,
                PoNo = txtPONo.Text,
                DispatchCode = txtDispatchCode.Text,
                ShipptoCode = "",
                ShipptoName = cmbShippingTo.GetValue(),//cmbShippingTo.GetValue()
                DocumentNo = txtDocNo.Text,
                Remark = txtRemark.Text
            };

            return item;
        }

        private void BindDatatoControl(DispatchAllModel dispatch)
        {
            txtPONo.Text = dispatch.PoNo;
            txtSearchShipto.Text = dispatch.ShipptoName;
            txtOrderNo.Text = dispatch.OrderNo;
            txtDispatchCode.Text = dispatch.DispatchCode;
            LoadComboDock();

            if (dispatch.DispatchDetails.Count() > 0)
            {
                List<RegisterTruckDetailModel> regisTruckDetails = new List<RegisterTruckDetailModel>();

                foreach (RegisterTruckDetailModel dispatchDetail in dispatch.DispatchDetails)
                {

                    if (dispatchDetail.BookingID != null)
                    {
                        decimal tmpRemainQTY = (dispatchDetail.BookingQty ?? 0) - (dispatchDetail.ConfirmQuantity ?? 0);

                        RegisterTruckDetailModel regisTruckDetail = new RegisterTruckDetailModel
                        {
                            DispatchId = dispatchDetail.DispatchId,
                            ReferenceID = dispatchDetail.ReferenceID.Value,
                            ProductCode = dispatchDetail.ProductCode,
                            ProductName = dispatchDetail.ProductName,
                            Order_Qty = dispatchDetail.DispatchDetail_Product_Quantity ?? 0,
                            Remain_Qty = tmpRemainQTY,
                            ProductUnitName = dispatchDetail.ProductUnitName,
                            ShippingID = dispatchDetail.ShippingID,
                            ProductID = dispatchDetail.ProductID.Value,
                            ShippingQuantity = dispatchDetail.ShippingQuantity ?? 0,
                            ConfirmQuantity = tmpRemainQTY,
                            ShippingUnitID = dispatchDetail.ShippingUnitID.Value,
                            //BasicQuantity = dispatchDetail.BasicQuantity.Value,

                            BookingQty = dispatchDetail.BookingQty ?? 0,   //Order Qty
                            BookingStockUnitId = dispatchDetail.BookingStockUnitId.Value,

                            BookingBaseQty = dispatchDetail.BookingBaseQty ?? 0,
                            BookingBaseUnitId = dispatchDetail.BookingBaseUnitId.Value,

                            //BasicUnitID = dispatchDetail.BasicUnitID.Value,
                            ConversionQty = dispatchDetail.ConversionQty,
                            BookingID = dispatchDetail.BookingID.Value,
                            TransactionTypeID = dispatchDetail.TransactionTypeID.Value,
                            Shipping_DT = DateTime.Now,
                            ShiptoID = dispatchDetail.ShiptoID,
                            //DeliveryUnit = dispatchDetail.ProductUnitID ?? 0
                        };

                        regisTruckDetails.Add(regisTruckDetail);
                    }
                }

                StoreData.Data = regisTruckDetails.Where(x => x.Remain_Qty > 0).ToList();
                StoreData.DataBind();
            }

            cmbShippingTo.SetValue(dispatch.ShipptoName);
            cmbWarehouseName.SetValue(dispatch.WarehouseID);

            X.Call("loadComboDock");

        }

        private void LoadComboRegisterType()
        {
            #region [ Load Register Type( ]

            //DataServiceModel dataService = new DataServiceModel();

            //List<SearchModel> _searchModel = new List<SearchModel>();

            //_searchModel.Add(new SearchModel()
            //{
            //    Group = 1,
            //    Key = "IsActive",
            //    Operation = "=",
            //    Value = "1",
            //    IsCondition = ConditionWhere.AND,
            //});

            //_searchModel.Add(new SearchModel()
            //{
            //    Group = 1,
            //    Key = "Name",
            //    Operation = "=",
            //    Value = "Production_Order_Type",
            //    IsCondition = ConditionWhere.OR,
            //});

            //dataService.Add<List<SearchModel>>("SearchModel", _searchModel);

            //List<CodeLookupModel> dataState = WebServiceHelper.Post<List<CodeLookupModel>>("SYS_Get_CodeLoockup", dataService.GetObject());

            Array values = Enum.GetValues(typeof(RegisterTruckEnum));

            List<Ext.Net.ListItem> items = new List<Ext.Net.ListItem>(values.Length);

            foreach (object i in values)
            {
                Ext.Net.ListItem l = new Ext.Net.ListItem
                {
                    Text = Enum.GetName(typeof(RegisterTruckEnum), i),
                    Value = i.ToString()
                };
                cmbRegisterType.Items.Add(l);
            }

            //StoreRegisterType.Data = dataState;
            //StoreRegisterType.DataBind();

            #endregion
        }

        private void BindData(string oKey)
        {
            try
            {
                LoadComboRegisterType();
                txtShipping_Code.Text = oKey;

                if (oKey == "new")
                {
                    txtDocNo.Text = oKey;
                    btnSave.Disabled = false;
                    btnAssign.Disabled = true;
                    cmbRegisterType.SelectedItem.Index = 1;
                    dtDate.SelectedDate = DateTime.Now;
                }
                else
                {

                    Guid id = new Guid(oKey);
                    ApiResponseMessage apiResp = RegisterTruckClient.GetByDetailID(id).Result;
                    RegisTruckModel data = apiResp.Get<RegisTruckModel>();
                    if (data == null)
                    {
                        return;
                    }

                    if (apiResp.IsSuccess)
                    {
                        Bind_Text(data);
                        btnAssign.Disabled = data.ShippingStatus == 20 ? true : false; //data.IsActive;
                        btnSave.Disabled = data.ShippingStatus == 20 ? true : false; ;//data.IsActive;
                        StoreData.Data = data.RegisTruckDetail.OrderBy(e=>e.ProductCode).ToList();
                        StoreData.DataBind();
                    }
                    else
                    {
                        MessageBoxExt.ShowError("data not found.");
                    }

                    SetButton("Edit");
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }
        }

        private void Bind_Text(RegisTruckModel item)
        {
            txtShipping_Code.Text = item.ShippingCode;
            txtShippingID.Text = item.ShippingID.ToString();
            btnAssign.Disabled = item.IsActive;
            btnSave.Visible = !item.IsActive;
            dtDate.SelectedDate = item.DocumentDate;
            txtTruckNo.Text = item.ShippingTruckNo;
            txtDriverName.Text = item.DriverName;
            txtCompany.Text = item.LogisticCompany;
            txtOrderNo.Text = item.OrderNo;
            txtContainerNo.Text = item.Container_No;
            txtSeal_No.Text = item.SealNo;
            txtBookingNo.Text = item.BookingNo;
            txtPONo.Text = item.PoNo;
            txtDocNo.Text = item.DocumentNo;
            txtDispatchCode.Text = item.DispatchCode;
            txtRemark.Text = item.Remark;
            cmbRegisterType.SelectedItem.Value = item.RegisterTypeID == 1 ? RegisterTruckEnum.Local.ToString() : RegisterTruckEnum.Export.ToString();
            #region [ Combo ]

            //cmbRegisterType.SetAutoCompleteValue(
            //           new List<CodeLookupModel>
            //                {
            //                    new CodeLookupModel
            //                    {
            //                        Code =item.Register_Type,
            //                        ValueTH = item.Register_Type
            //                    }
            //                },
            //                item.RegisterType.ToString()
            //       );

            cmbWarehouseName.SetAutoCompleteValue(
                        new List<Warehouse>
                        {
                            new Warehouse
                            {
                                 WarehouseID = item.WarehouseID.Value,
                                 Name = item.WarehouseName
                            }
                        }//,
                         //item.WarehouseCode.ToString()

                );
            cmbTruckType.SetAutoCompleteValue(
                        new List<TruckType>
                        {
                            new TruckType
                            {
                                TruckTypeID = item.TruckTypeID,
                                TypeName = item.TruckTypeName
                            }
                        }//,
                         //item.TruckTypeName.ToString()

                );
            LoadComboDock();

            cmbDock.SetAutoCompleteValue(
                        new List<DockConfig>
                        {
                            new DockConfig
                            {
                                DockConfigID = item.DockTypeID.Value,
                                DockName = item.DockTypeName
                            }
                        },
                            item.DockTypeID.ToString()

                );

            cmbShippingTo.SetAutoCompleteValue(
                new List<ShippingTo>
                    {
                            new ShippingTo
                            {
                                 ShipToId = item.ShiptoID,
                                 Name = item.ShipptoName
                            }
                    }
               );

            //string Name = item.ShipptoName;
            //cmbShippingTo.SetValue(Name);

            #endregion

        }

        private DispatchAllModel GetDispatchList(Guid? warehouseID, string Pono = "")
        {
            int total = 0;
            ApiResponseMessage apiResp = RegisterTruckClient.getdispatchForRegisTruckById(warehouseID, txtSearchPO.Text != "" ? txtSearchPO.Text : Pono).Result;
            DispatchAllModel data = new DispatchAllModel();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<DispatchAllModel>();
            }
            return data;
        }

        private List<DispatchAllModel> GetBrowseDispatchList()
        {
            int total = 0;
            ApiResponseMessage apiResp = RegisterTruckClient.getdispatchForRegisTrucklistAll(null, txtSearchPO.Text, 0, 20).Result;
            List<DispatchAllModel> data = new List<DispatchAllModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<DispatchAllModel>>();
            }
            return data;
        }


        #endregion
    }
}