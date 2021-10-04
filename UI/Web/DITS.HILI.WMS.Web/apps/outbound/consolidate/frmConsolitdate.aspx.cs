using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.RegisterTruckModel;
using DITS.HILI.WMS.RegisterTruckModel.CustomModel;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.outbound.consolidate
{
    public partial class frmConsolitdate : BaseUIPage
    {

        private readonly string AppDataService = "../../../Common/DataClients/MsDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                byte[] bytes = Convert.FromBase64String(Request.QueryString["oDataKeyId"]);
                string oKey = Encoding.UTF8.GetString(bytes);

                txtConsolidateStatusId.Text = Request.QueryString["oDataStatus"];
                BindData(oKey, Request.QueryString["oDataStatus"], Request.QueryString["oDataDocumentNo"]);

                SetButton("Edit");
            }

        }
        [DirectMethod(Timeout=180000)]
        protected void btnConsolidateDo_Click(object sender, DirectEventArgs e)
        {
            X.Call("popitup", $"../../Report/frmReportViewer.aspx?reportName=RPT_ConsolidateForm&Silent={false}&PONo={txtPONo.Text}");
        }
        [DirectMethod(Timeout=180000)]
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {

                string gridJson = e.ExtraParams["ParamStorePages"];
                List<RegisterTruckConsolidateDeatilModel> gridData = JSON.Deserialize<List<RegisterTruckConsolidateDeatilModel>>(gridJson);

                if (!Is_Validation_Save(gridData))
                {
                    return;
                }



                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();

                datasave = ConsolidateClient.ModifyConsolidate(gridData).Result;
                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
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
        [DirectMethod(Timeout=180000)]
        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {

            DoConfirm();
            //try
            //{

            //    string gridJson = e.ExtraParams["ParamStorePages"];
            //List<RegisterTruckConsolidateDeatilModel> gridData = JSON.Deserialize<List<RegisterTruckConsolidateDeatilModel>>(gridJson);

            //if (!Is_Validation_Save(gridData))
            //{
            //    return;
            //}



            //bool isSuccess = true;
            //ApiResponseMessage datasave = new ApiResponseMessage();

            //datasave = ClientService.Outbound.ConsolidateClient.ApproveConsolidate(gridData).Result;
            //if (datasave.ResponseCode != "0")
            //{
            //    isSuccess = false;
            //    MessageBoxExt.ShowError(datasave.ResponseMessage);
            //}


            //if (isSuccess)
            //{
            //    X.Call("parent.App.direct.ApproveReload");
            //    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            //}

            //}
            //catch (Exception ex)
            //{
            //    MessageBoxExt.ShowError(ex);
            //}
        }
        [DirectMethod(Timeout=180000)]
        public void DoConfirm()
        {
            byte[] bytes = Convert.FromBase64String(Request.QueryString["oDataKeyId"]);
            string oKey = Encoding.UTF8.GetString(bytes);

            ApiResponseMessage apiResp = ConsolidateClient.GetConsolidateByPO(oKey, Request.QueryString["oDataDocumentNo"]).Result;
            RegisterTruckConsolidateHeaderModel data = apiResp.Get<RegisterTruckConsolidateHeaderModel>();
            if (data == null)
            {
                return;
            }

            if (apiResp.IsSuccess)
            {
                RegisterTruckConsolidateDeatilModel campareqty = data.RegisterTruckConsolidateDeatilModels.Where(x => x.PickStockQty != x.ConsolidateQty).FirstOrDefault();
                if (campareqty != null)
                {
                    X.Msg.Confirm("เตือน", "จำนวนที่ขนขึ้นรถไม่เท่ากับจำนวนที่ยิบ!คุณแน่ใจที่จะยืนยันตามจำนวนนี้หรือไม่? ", new MessageBoxButtonsConfig
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
                }
                //  List<RegisterTruckConsolidateDeatilModel> datadetail = data.RegisterTruckConsolidateDeatilModels;
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
                byte[] bytes = Convert.FromBase64String(Request.QueryString["oDataKeyId"]);
                string oKey = Encoding.UTF8.GetString(bytes);

                ApiResponseMessage apiResp = ConsolidateClient.GetConsolidateByPO(oKey, Request.QueryString["oDataDocumentNo"]).Result;
                RegisterTruckConsolidateHeaderModel data = apiResp.Get<RegisterTruckConsolidateHeaderModel>();
                if (data == null)
                {
                    MessageBoxExt.ShowError("data not found.");
                }

                if (!apiResp.IsSuccess)
                {
                    MessageBoxExt.ShowError("data not success.");
                }

                List<RegisterTruckConsolidateDeatilModel> gridData = data.RegisterTruckConsolidateDeatilModels.ToList();//JSON.Deserialize<List<RegisterTruckConsolidateDeatilModel>>(gridJson);

                if (!Is_Validation_Save(gridData))
                {
                    return;
                }



                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();

                datasave = ConsolidateClient.ApproveConsolidate(gridData).Result;
                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }


                if (isSuccess)
                {
                    X.Call("parent.App.direct.ApproveReload", txtPONo.Text);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
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
            //string oDataKeyId = e.ExtraParams["oDataKeyId"];
            //string gridJson = e.ExtraParams["ParamStorePages"];

            //var gridDataList = JSON.Deserialize<List<RegisTruckDetailModel>>(gridJson);
            //var girdData = gridDataList.Where(x => x.DispatchDetail_ID == oDataKeyId.ToInt()).SingleOrDefault();
            //gridDataList.Remove(girdData);

            //StoreData.Data = gridDataList;
            //StoreData.DataBind();
        }

        #region DirectMethod

        [DirectMethod(Timeout=180000)]
        public void LoadComboDock()
        {
            string strSearch = cmbTruckType.SelectedItem.Value;
            string wh = cmbWarehouseName.SelectedItem.Value;

            //DataServiceModel dataService = new DataServiceModel();
            //dataService.Add<string>("TruckType", strSearch);
            //dataService.Add<string>("warehouse", wh);

            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "TruckType" },
                { "TruckTypeId", cmbTruckType.SelectedItem.Value }
            };
            StoreDock.AutoCompleteProxy(AppDataService, param);

        }


        #endregion

        #region Private Method

        private void SetParameterProxy()
        {
            //StoreWarehouseName.AutoCompleteProxy(AutoCompleteService,
            //                                      new string[] { "Methode" },
            //                                      new string[] { "warehousename" });



        }

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

        private bool Is_Validation_Save(List<RegisterTruckConsolidateDeatilModel> gridData = null)
        {
            if (gridData != null)
            {
                //if (gridData.Any(x => x.ConsolidateQty == 0))
                //{
                //    MessageBoxExt.ShowError("consilidate Qty cannot be 0", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                //    return false;
                //}
                // else 
                if (gridData.Count() == 0)
                {
                    MessageBoxExt.ShowError("ไม่มีข้อมูลใน Consolidate", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                    return false;
                }
            }
            else
            {
                MessageBoxExt.ShowError("ไม่มีข้อมูลใน Consolidate", MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return false;
            }

            return true;
        }

        //private void BindDatatoControl(DispatchAllModel dispatch)
        //{
        //    txtPONo.Text = dispatch.PoNo;
        //    //txtSearchShipto.Text = dispatch.ShipptoName;
        //    txtOrderNo.Text = dispatch.OrderNo;
        //    txtDispatchCode.Text = dispatch.DispatchCode;

        //    if (dispatch.DispatchDetails.Count() > 0)
        //    {
        //        var regisTruckDetails = new List<RegisterTruckDetailModel>();

        //        foreach (var dispatchDetail in dispatch.DispatchDetails)
        //        {

        //            if (dispatchDetail.BookingID != null)
        //            {
        //                var tmpRemainQTY = (dispatchDetail.BookingQty ?? 0) - (dispatchDetail.ShippingQuantity ?? 0);

        //                var regisTruckDetail = new RegisterTruckDetailModel
        //                {
        //                    DispatchId = dispatchDetail.DispatchId,
        //                    ReferenceID = dispatchDetail.ReferenceID.Value,
        //                    ProductCode = dispatchDetail.ProductCode,
        //                    ProductName = dispatchDetail.ProductName,
        //                    Order_Qty = dispatchDetail.DispatchDetail_Product_Quantity ?? 0,
        //                    Remain_Qty = tmpRemainQTY,
        //                    ProductUnitName = dispatchDetail.ProductUnitName,
        //                    ShippingID = dispatchDetail.ShippingID,
        //                    ProductID = dispatchDetail.ProductID.Value,

        //                    ShippingQuantity = dispatchDetail.ShippingQuantity ?? 0, //Delivery Qty
        //                    ShippingUnitID = dispatchDetail.ShippingUnitID.Value,
        //                    //BasicQuantity = dispatchDetail.BasicQuantity.Value,

        //                    BookingQty = dispatchDetail.BookingQty ?? 0,   //Order Qty
        //                    BookingStockUnitId = dispatchDetail.BookingStockUnitId.Value,

        //                    BookingBaseQty = dispatchDetail.BookingBaseQty ?? 0,
        //                    BookingBaseUnitId = dispatchDetail.BookingBaseUnitId.Value,

        //                    //BasicUnitID = dispatchDetail.BasicUnitID.Value,
        //                    ConversionQty = dispatchDetail.ConversionQty ?? 0,
        //                    BookingID = dispatchDetail.BookingID.Value,
        //                    TransactionTypeID = dispatchDetail.TransactionTypeID.Value,
        //                    Shipping_DT = DateTime.Now,
        //                    ShiptoID = dispatchDetail.ShiptoID,
        //                    //DeliveryUnit = dispatchDetail.ProductUnitID ?? 0
        //                };
        //                regisTruckDetails.Add(regisTruckDetail);
        //            }
        //        }

        //        StoreData.Data = regisTruckDetails;
        //        StoreData.DataBind();
        //    }

        //    cmbShippingTo.SetAutoCompleteValue(
        //    new List<Shipto>
        //    {
        //                    new Shipto
        //                    {
        //                         ShipToId = dispatch.ShipToId,
        //                         Name = dispatch.ShipptoName
        //                    }
        //    }


        // );
        //    //cmbShippingTo.SetValue(Name);
        //}


        private void LoadComboRegisterType()
        {
            #region [ Load Register Type( ]

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

            #endregion
        }


        [DirectMethod(Timeout=180000)]
        private void BindData(string oKey, string status, string documentNo)
        {
            try
            {


                LoadComboRegisterType();

                txtShipping_Code.Text = oKey;

                if (oKey == "new")
                {
                    cmbRegisterType.SelectedItem.Index = 1;
                    dtDate.SelectedDate = DateTime.Now;
                }
                else
                {


                    ApiResponseMessage apiResp = ConsolidateClient.GetConsolidateByPO(oKey, documentNo).Result;
                    RegisterTruckConsolidateHeaderModel data = apiResp.Get<RegisterTruckConsolidateHeaderModel>();
                    if (data == null)
                    {
                        return;
                    }

                    if (apiResp.IsSuccess)
                    {
                        Bind_Text(data);

                        btnSave.Disabled = data.ShippingStatus == 20 ? true : false; ;//data.IsActive;

                        StoreData.Data = data.RegisterTruckConsolidateDeatilModels;
                        StoreData.DataBind();

                    }
                    else
                    {
                        MessageBoxExt.ShowError("data not found.");
                    }


                    SetButton("Edit");

                    if (int.Parse(status) == (int)ConsolidateStatusEnum.WaitingConfirm)
                    {
                        btnApprove.Visible = true;
                        btnSave.Visible = true;
                    }
                    else
                    {
                        btnApprove.Visible = false;
                        btnSave.Visible = false;
                        colDel.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }
        }

        private void Bind_Text(RegisterTruckConsolidateHeaderModel item)
        {
            txtShipping_Code.Text = item.ShippingCode;
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
            txtDispatchCode.Text = item.Dispatchcode;
            txtRemark.Text = item.Remark;
            cmbRegisterType.SelectedItem.Value = item.RegisterTypeID == 1 ? RegisterTruckEnum.Local.ToString() : RegisterTruckEnum.Export.ToString();
            #region [ Combo ]



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
                                 Name = item.ShiptoName
                            }
                    }
               );


            #endregion

        }



        #endregion
    }
}