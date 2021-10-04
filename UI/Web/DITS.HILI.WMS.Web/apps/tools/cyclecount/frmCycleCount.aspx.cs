using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.Web.Common.Util;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.tools.cyclecount
{
    public partial class frmCycleCount : BaseUIPage
    {
        private readonly string AppDataService = "../../../Common/DataClients/MsDataHandler.ashx";
        private readonly string AutoCompleteService = "../../../Common/DataClients/OptDataHandler.ashx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCustomerCode.Text = Request.QueryString["oCustomerCode"];
                bindData(Request.QueryString["oDataKeyId"]);
            }
        }
        protected void btnCycleCountlist_Click(object sender, DirectEventArgs e)
        {

            X.Call("popitup", $"../../Report/frmReportViewer.aspx?reportName=RPT_CycleCountForm&Silent={false}&CycleCountCode={txtJobNo.Text}");
        }
        protected void btnAddItem_Click(object sender, DirectEventArgs e)
        {
            WindowDataDetail.Show();
        }
        protected void btnConfirm_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStoreDetail"];
            List<CycleCountDetails> gridCurrentData = JSON.Deserialize<List<CycleCountDetails>>(e.ExtraParams["ParamStoreData"]);

            // Array of Dictionaries
            RowSelectionModel sm = GridGetWindows.GetSelectionModel() as RowSelectionModel;
            List<CycleCountModel> gridData = JSON.Deserialize<List<CycleCountModel>>(gridJson);

            List<CycleCountDetails> _details = new List<CycleCountDetails>();
            foreach (SelectedRow row in sm.SelectedRows)
            {
                CycleCountModel item = gridData[row.RowIndex];
                bool has = gridCurrentData.Any(x => x.ProductID == item.ProductID && x.ProductCode == item.ProductCode && x.LocationNo == item.LocationNo && x.PalletCode == item.PalletCode && x.ConversionQty == item.ConversionQty);
                if (has)
                {
                    X.MessageBox.Show(new MessageBoxConfig
                    {
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Warning",
                        Message = "An existing product [" + item.ProductName + " ]",
                        Buttons = MessageBox.Button.OK
                    });
                    return;
                }

                if (item != null)
                {
                    _details.Add(new CycleCountDetails
                    {
                        CycleCountCode = txtJobNo.Text == "new" ? string.Empty : txtJobNo.Text,
                        LocationNo = item.LocationNo,
                        ConversionQty = item.ConversionQty,
                        PalletCode = item.PalletCode,
                        ProductCode = item.ProductCode,
                        Lot = item.Lot,
                        ProductName = item.ProductName,
                        ProductID = item.ProductID,
                        ProductUnitID = item.ProductUnitID,
                        ProductUnitName = item.ProductUnitName,
                        RemainQTY = item.RemainQTY,
                        ZoneID = item.ZoneID,
                        LocationID = item.LocationID,
                        CountingQty = 0,
                        DiffQty = 0,
                        WarehouseName = item.WarehouseName
                    });
                }

            }
            X.AddScript("App.GridGetWindows.getSelectionModel().deselectAll();");
            IEnumerable<CycleCountDetails> result = gridCurrentData.Concat(_details);
            if (_details.Count() > 0)
            {
                btnSave.Show();
            }

            btnSave.Disabled = false;

            StoreOfDataList.DataSource = result;
            StoreOfDataList.DataBind();
            WindowDataDetail.Hide();
        }
        protected async void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string gridJson = e.ExtraParams["ParamStorePages"];
                List<CycleCountDetails> gridData = JSON.Deserialize<List<CycleCountDetails>>(gridJson);

                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (txtJobNo.Text == "new")
                {
                    CycleCountModel cy = new CycleCountModel
                    {
                        CycleCountCode = (txtJobNo.Text == "new" ? string.Empty : txtJobNo.Text),
                        WarehouseID = new Guid(cmbWarehouseName.SelectedItem.Value),
                        ZoneID = cmbZone.SelectedItem.Value == "0" ? (Guid?)null : new Guid(cmbZone.SelectedItem.Value),
                        CycleCountStartDate = Convert.ToDateTime(dateCreateJob.Value),
                        CycleCountAssignDate = Convert.ToDateTime(dateCreateJob.Value),
                        Remark = txtRemark.Text,
                        CycleCountDetails = gridData,
                    };

                    datasave = InventoryToolsClient.Add(cy).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }

                    txtJobNo.Text = "new";
                    dateCreateJob.Value = DateTime.Now;
                    txtRemark.Text = "";
                    btnSave.Hide();
                    StoreOfDataList.Reload();

                    if (isSuccess)
                    {
                        X.Call("parent.App.direct.Reload", datasave.ResponseMessage);
                    }
                }
                else
                {
                    ApiResponseMessage apiResp = InventoryToolsClient.GetCycleCountDetail(txtJobNo.Text).Result;
                    CycleCountModel result = apiResp.Get<CycleCountModel>();
                    if (result == null)
                    {
                        return;
                    }

                    result.CycleCountStartDate = Convert.ToDateTime(dateCreateJob.Value);
                    result.CycleCountAssignDate = Convert.ToDateTime(dateCreateJob.Value);
                    result.Remark = txtRemark.Text;
                    result.CycleCountDetails = gridData;

                    datasave = InventoryToolsClient.Modify(result).Result;

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
                string gridJson = e.ExtraParams["ParamStorePages"];
                List<CycleCountDetails> gridData = JSON.Deserialize<List<CycleCountDetails>>(gridJson);

                ApiResponseMessage apiResp = InventoryToolsClient.GetCycleCountDetail(txtJobNo.Text).Result;
                CycleCountModel result = apiResp.Get<CycleCountModel>();
                if (result == null)
                {
                    return;
                }

                result.Remark = txtRemark.Text;
                result.CycleCountDetails = gridData;
                bool isSuccess = true;
                ApiResponseMessage datasave = new ApiResponseMessage();

                datasave = InventoryToolsClient.Approve(result).Result;

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
        private async void bindData(string oDataKeyId)
        {
            //SetParameterProxy();
            txtJobNo.Text = oDataKeyId;
            if (oDataKeyId == "new")
            {
                WindowDataDetail.Hidden = true;
                dateCreateJob.Value = DateTime.Now;
                colCountingQty.Visible = false;
                colDiff.Visible = false;
                btnSave.Hide();
                CheckStatus(0);
                return;
            }

            WindowDataDetail.Hidden = true;
            ApiResponseMessage apiResp = InventoryToolsClient.GetCycleCountDetail(txtJobNo.Text).Result;
            CycleCountModel cyclecount = apiResp.Get<CycleCountModel>();
            if (cyclecount == null)
            {
                return;
            }

            if (apiResp.IsSuccess)
            {
                StoreWindowsShow.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                StoreWindowsShow.Data = cyclecount;
                StoreWindowsShow.DataBind();
            }
            if (cyclecount == null)
            {
                return;
            }

            txtRemark.Text = cyclecount.Remark;
            dateCreateJob.Value = cyclecount.CycleCountAssignDate.Value;
            StoreOfDataList.DataSource = cyclecount.CycleCountDetails;
            StoreOfDataList.DataBind();

            CheckStatus(cyclecount.CycleCountStatus.Value);
        }
        private void CheckStatus(int status)
        {
            switch (status)
            {
                case 0:
                    btnSave.Enable();
                    btnApprove.Disable();
                    break;
                case 10:
                    btnSave.Enable();
                    btnApprove.Disable();
                    break;
                case 20:
                    btnSave.Enable();
                    btnApprove.Enable();
                    break;
                case 30:
                    btnAddItem.Disable();
                    btnSave.Enable();
                    btnApprove.Enable();
                    break;
                case 100:
                    txtRemark.Disable();
                    dateCreateJob.Disable();
                    // this.txtConfirmQty.Editable = false;
                    btnSave.Disable();
                    btnApprove.Disable();
                    btnAddItem.Disable();
                    break;
                case 99:
                    txtRemark.Disable();
                    dateCreateJob.Disable();
                    // this.txtConfirmQty.Editable = false;
                    btnSave.Disable();
                    btnApprove.Disable();
                    btnAddItem.Disable();
                    break;
                default:
                    break;
            }
        }
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {

            if (!ValidationSearch(e))
            {
                return;
            }

            getProduct();
        }
        public void getProduct()
        {
            if (!string.IsNullOrWhiteSpace(cmbWarehouseName.GetValue()))
            {
                System.Guid? ZoneID = new Guid();
                System.Guid? WarehouseID = new Guid();
                if (!string.IsNullOrWhiteSpace(cmbZone.SelectedItem.Value) && cmbZone.SelectedItem.Value != "0")
                {
                    ZoneID = new Guid(cmbZone.SelectedItem.Value);
                }

                if (!string.IsNullOrWhiteSpace(cmbWarehouseName.SelectedItem.Value))
                {
                    WarehouseID = new Guid(cmbWarehouseName.SelectedItem.Value);
                }

                Dictionary<string, object> param = new Dictionary<string, object>
                {
                    { "Method", "ProductStock" },
                    { "WarehouseID", WarehouseID },
                    { "ZoneID", ZoneID },
                    { "query", txtSearch.Text }
                };

                StoreWindowsShow.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                StoreWindowsShow.AutoCompleteProxy(AutoCompleteService, param);
                StoreWindowsShow.LoadProxy();
            }
        }
        protected void btnClear_Click(object sender, DirectEventArgs e)
        {
            if (txtJobNo.Text == "new")
            {
                txtJobNo.Text = "new";
                dateCreateJob.Value = DateTime.Now;
                txtRemark.Text = "";
                btnSave.Hide();
                StoreOfDataList.RemoveAll();
                CheckStatus(0);
            }
            else
            {
                bindData(txtJobNo.Text);
            }
        }
        protected void cmbWarehouseName_Change(object sender, DirectEventArgs e)
        {
            if (Request.QueryString["oDataKeyId"] == "new")
            {
                ApiResponseMessage api = ClientService.Master.WarehouseClient.GetZone(new Guid(cmbWarehouseName.SelectedItem.Value), "", 0, int.Parse(cmbPageList.SelectedItem.Value)).Result;
                List<Zone> data = api.Get<List<Zone>>();
                if (api.IsSuccess)
                {
                    cmbZone.Items.Clear();
                    List<Ext.Net.ListItem> items = new List<Ext.Net.ListItem>();

                    foreach (Zone i in data)
                    {
                        cmbZone.InsertItem(0, i.Name.ToString(), i.ZoneID.ToString());
                    }

                    cmbZone.InsertItem(0, "- Select All -", 0);
                    cmbZone.SelectedItem.Index = 0;
                }
            }
        }
        private bool ValidationSearch(DirectEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cmbWarehouseName.GetValue()) && e.ExtraParams.Count() != 0)
            {
                MessageBoxExt.ShowError("Please Select warehouse ", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbWarehouseName').focus('', 10); ");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbZone.GetValue()) && e.ExtraParams.Count() != 0)
            {
                MessageBoxExt.ShowError("Please Select zone ", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('cmbZone').focus('', 10); ");
                return false;
            }

            return true;
        }

    }
}