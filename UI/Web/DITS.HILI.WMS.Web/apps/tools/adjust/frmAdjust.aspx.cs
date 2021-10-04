using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.InventoryToolsModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.adjust
{
    public partial class frmAdjust : BaseUIPage
    {
        private readonly string AutoCompleteService = "../../../Common/DataClients/OptDataHandler.ashx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string code = Request.QueryString["oDataKeyId"];
                dateCreateJob.Value = DateTime.Now;
                if (code != "new")
                {
                    getDataAdjust(code);
                }

                //getWarehouse();
                getAdjustType();
                //getProductOwner();
                //SetButton(0);


                WindowDataDetail.Hide();


            }

        }

        protected void btnAddItem_Click(object sender, DirectEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbAdjustType.Text))
                {
                    MessageBoxExt.Warning("Please select Adjust type.");
                    return;
                }

                getProducts();
                WindowDataDetail.Show();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }
        protected void btnSearchProduct_Click(object sender, DirectEventArgs e)
        {
            getProducts();
        }
        protected void btnSelectItem_Click(object sender, DirectEventArgs e)
        {

            List<AdjustModel> product = JSON.Deserialize<List<AdjustModel>>(e.ExtraParams["ParamStoreWProduct"]);
            List<AdjustModelDetails> data = JSON.Deserialize<List<AdjustModelDetails>>(e.ExtraParams["ParamStoreWData"]);
            RowSelectionModel sm = GridGetWindows.GetSelectionModel() as RowSelectionModel;
            List<AdjustModelDetails> _details = new List<AdjustModelDetails>();
            foreach (SelectedRow row in sm.SelectedRows)
            {
                AdjustModel item = product[row.RowIndex];

                bool has = data.Any(x => x.PalletCode == item.PalletCode);
                if (has)
                {
                    X.MessageBox.Show(new MessageBoxConfig

                    {
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Warning",
                        Message = "An existing pallet [" + item.PalletCode + " ]",
                        Buttons = MessageBox.Button.OK
                    });
                    return;
                }

                if (item != null)
                {
                    _details.Add(new AdjustModelDetails
                    {
                        LocationID = item.LocationID,
                        //Customer_Code = item.ProductOwnerCode,
                        LocationNo = item.LocationNo,
                        ConversionQty = item.ConversionQty,
                        PalletCode = item.PalletCode,
                        //Price = item.Price,
                        ProductCode = item.ProductCode,
                        //Product_EXP = item.Product_EXP,
                        ProductLot = item.Lot,
                        MFGDate = item.MFGDate,
                        ProductName = item.ProductName,
                        ProductID = item.ProductID,
                        ProductStatusID = item.ProductStatusID,
                        //Product_Sub_Status_Code = item.Product_Sub_Status_Code,
                        //Product_Sub_Status_Name = item.Product_Sub_Status_Name,
                        //Product_System_Code = item.Product_System_Code,
                        ProductUnitID = item.ProductUnitID,
                        ProductUnitName = item.ProductUnitName,
                        AdjustBaseUnitID = item.AdjustBaseUnitID,
                        AdjustBaseQty = item.RemainBaseQTY,
                        //Warehouse_Name = item.Warehouse_Name,
                        AdjustStockQty = 0,
                        AdjustStockUnitID = item.AdjustStockUnitID

                    });
                }

            }

            X.AddScript("App.GridGetWindows.getSelectionModel().deselectAll();");
            IEnumerable<AdjustModelDetails> result = data.Concat(_details);
            StoreOfDataList.DataSource = result;
            StoreOfDataList.DataBind();
            btnSave.Enable();
            WindowDataDetail.Hide();
        }
        [DirectMethod(Timeout = 180000)]
        private void getProducts()
        {
            string whCode = cmbWarehouseName.SelectedItem.Value;
            Dictionary<string, object> param = new Dictionary<string, object>();
            if (cmbAdjustType.SelectedItem.Value == AdjustTypeStatusEnum.AddStock.ToString())
            {
                getCycleCountNoAdjust();
            }

            if (cmbAdjustType.SelectedItem.Value == AdjustTypeStatusEnum.ReduceStock.ToString())
            {
                getCycleCountNoAdjust();
            }

            if (cmbAdjustType.SelectedItem.Value == AdjustTypeStatusEnum.AddOther.ToString() || cmbAdjustType.SelectedItem.Value == AdjustTypeStatusEnum.ReduceOther.ToString())
            {
                getStockBalanceAdjust();
            }
        }
        [DirectMethod(Timeout = 180000)]
        private async void getDataAdjust(string code)
        {
            try
            {
                //var data = await HttpService.Get<AdjustmentModel>("Adjust/get?code=" + code);
                ApiResponseMessage apiResp = InventoryToolsClient.GetAdjustDetail(code).Result;
                AdjustModel _adjust = apiResp.Get<AdjustModel>();
                if (_adjust == null)
                {
                    return;
                }

                txtJobNo.Text = _adjust.AdjustCode;
                dateCreateJob.Value = _adjust.AdjustStartDate;
                txtRemark.Text = _adjust.Remark;
                txtRefered.Text = _adjust.ReferenceDoc;
                cmbAdjustType.SelectedItem.Value = _adjust.AdjustTypeName.ToString();

                StoreOfDataList.DataSource = _adjust.AdjustModelDetails.ToList();
                StoreOfDataList.DataBind();

                cmbAdjustType.ReadOnly = true;

                SetButton(_adjust.AdjustStatus);
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        [DirectMethod(Timeout = 180000)]
        private void getStockBalanceAdjust()
        {
            string WarehouseID = cmbWarehouseName.SelectedItem.Value;

            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "AdjustStock" },
                { "WarehouseID", WarehouseID },
                { "Product", txtProduct.Text },
                { "Pallet", txtPallet.Text },
                { "Lot", txtLot.Text },
                { "state", cmbAdjustType.SelectedItem.Value }
            };

            StoreStockBalance.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            StoreStockBalance.AutoCompleteProxy(AutoCompleteService, param);
            StoreStockBalance.LoadProxy();
        }
        [DirectMethod(Timeout = 180000)]
        private void getCycleCountNoAdjust()
        {
            string WarehouseID = cmbWarehouseName.SelectedItem.Value;

            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "Method", "CycleCountWithOutAdjust" },
                { "WarehouseID", WarehouseID },
                { "Product", txtProduct.Text },
                { "Pallet", txtPallet.Text },
                { "Lot", txtLot.Text },
                { "state", cmbAdjustType.SelectedItem.Value }
            };


            //getWarehouse();
            //getProductOwner();

            StoreStockBalance.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            StoreStockBalance.AutoCompleteProxy(AutoCompleteService, param);
            StoreStockBalance.LoadProxy();
        }
        [DirectMethod(Timeout = 180000)]
        protected async void getAdjustType()
        {
            try
            {
                cmbAdjustType.Items.Clear();
                Array values = Enum.GetValues(typeof(AdjustTypeStatusEnum));

                List<Ext.Net.ListItem> items = new List<Ext.Net.ListItem>(values.Length);

                foreach (object i in values)
                {
                    Ext.Net.ListItem l = new Ext.Net.ListItem
                    {
                        Text = Enum.GetName(typeof(AdjustTypeStatusEnum), i),
                        Value = i.ToString()
                    };
                    cmbAdjustType.Items.Add(l);
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }
        protected async void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtJobNo.Text == "new")
                {
                    AdjustModel h = new AdjustModel
                    {
                        IsActive = true,
                        AdjustStartDate = Convert.ToDateTime(dateCreateJob.Value),
                        AdjustTypeName = cmbAdjustType.SelectedItem.Value,
                        ReferenceDoc = txtRefered.Text,
                        Remark = txtRemark.Text
                    };

                    List<AdjustModelDetails> details = JSON.Deserialize<List<AdjustModelDetails>>(e.ExtraParams["ParamStorePages"]);
                    if (details.Any(x => x.AdjustStockQty == null || x.AdjustStockQty == 0))
                    {
                        MessageBoxExt.Warning("Adjust QTY shouldn't equal 0");
                        return;
                    }

                    h.AdjustModelDetails = details;
                    //var result = await HttpService.Post<AdjustmentModel>("Adjust/add", h);
                    //X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage("Save complete"));

                    if (isSuccess)
                    {
                        X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                    }

                    datasave = InventoryToolsClient.Add(h).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }

                    txtJobNo.Text = "new";
                    dateCreateJob.Value = DateTime.Now;
                    txtRemark.Text = "";
                    txtRefered.Text = "";
                    cmbAdjustType.Reset();
                    StoreOfDataList.Reload();
                }
                else
                {
                    //var _adjust = await HttpService.Get<AdjustmentModel>("Adjust/get?code=" + this.txtJobNo.Text);

                    ApiResponseMessage apiResp = InventoryToolsClient.GetAdjustDetail(txtJobNo.Text).Result;
                    AdjustModel _adjust = apiResp.Get<AdjustModel>();
                    if (_adjust == null)
                    {
                        return;
                    }

                    if (_adjust == null)
                    {
                        throw new Exception("Not found job adjustment");
                    }

                    _adjust.Remark = txtRemark.Text;
                    _adjust.ReferenceDoc = txtRefered.Text;
                    _adjust.AdjustStartDate = Convert.ToDateTime(dateCreateJob.Value);

                    List<AdjustModelDetails> details = JSON.Deserialize<List<AdjustModelDetails>>(e.ExtraParams["ParamStorePages"]);
                    if (details.Any(x => x.AdjustStockQty == null || x.AdjustStockQty == 0))
                    {
                        MessageBoxExt.Warning("Adjust QTY shouldn't equal 0");
                        return;
                    }

                    details.ForEach(item =>
                    {
                        item.AdjustCode = txtJobNo.Text;
                    });

                    _adjust.AdjustModelDetails = details;

                    datasave = InventoryToolsClient.Modify(_adjust).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                    if (isSuccess)
                    {
                        X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    }

                    //await HttpService.Post("Adjust/modify", _adjust);
                    //this.cmbAdjustType.Reset();
                    //X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage("Save complete"));
                    //X.Call("parentAutoLoadControl.close()");
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }

        }
        protected async void btnApprove_Click(object sender, DirectEventArgs e)
        {
            try
            {
                //var _adjust = await HttpService.Get<AdjustmentModel>("Adjust/get?code=" + this.txtJobNo.Text);
                //if (_adjust == null)
                //    throw new Exception("Not found job adjustment");

                //_adjust.Update_User = AppsInfo.UserName;
                //var details = JSON.Deserialize<List<AdjustmentDetail>>(e.ExtraParams["ParamStorePages"]);
                //_adjust.AdjustmentDetail = details;

                //await HttpService.Post("Adjust/approve", _adjust);

                //X.Call("parent.App.direct.SomeDirectMethod", MyToolkit.CustomMessage("Approve complete"));
                //X.Call("parentAutoLoadControl.close()");

                ApiResponseMessage apiResp = InventoryToolsClient.GetAdjustDetail(txtJobNo.Text).Result;
                AdjustModel _adjust = apiResp.Get<AdjustModel>();
                if (_adjust == null)
                {
                    return;
                }

                List<AdjustModelDetails> details = JSON.Deserialize<List<AdjustModelDetails>>(e.ExtraParams["ParamStorePages"]);
                _adjust.AdjustModelDetails = details;
                _adjust.Remark = txtRemark.Text;
                _adjust.IsSentInterface = chkIsNotSendToInterface.Checked;
                _adjust.IsHideItem = chkIsHide.Checked;

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();

                datasave = InventoryToolsClient.Approve(_adjust).Result;

                if (datasave.ResponseCode != "0")
                {
                    isSuccess = false;
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }

                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (txtJobNo.Text == "new")
            {
                txtJobNo.Text = "new";
                txtRefered.Clear();
                txtRemark.Clear();
                txtProduct.Clear();
                txtPallet.Clear();
                txtLot.Clear();
                txtConfirmQty.Clear();
                StoreOfDataList.RemoveAll();
                dateCreateJob.Value = DateTime.Now;
                SetButton(0);
            }
            else
            {
                getDataAdjust(txtJobNo.Text);
            }
        }
        private void SetButton(int adjustState)
        {
            switch (adjustState)
            {
                case 0:
                    btnAddItem.Enable();
                    btnApprove.Disable();
                    btnSave.Enable();
                    break;
                case 10:
                    btnAddItem.Enable();
                    btnApprove.Enable();
                    btnSave.Enable();

                    MasterModel.Secure.UserAccounts user = ClientService.Common.User;
                    if (ConfigurationManager.AppSettings["powerUser"].ToString() == user.UserName)
                    {
                        chkIsNotSendToInterface.Hidden = false;
                        chkIsHide.Hidden = false;
                    }
                    break;
                case 100:
                    btnSave.Disable();
                    btnApprove.Disable();
                    btnAddItem.Disable();
                    break;
                case 102:
                    btnSave.Disable();
                    btnApprove.Disable();
                    btnAddItem.Disable();
                    break;
                default: break;
            }
        }
    }
}