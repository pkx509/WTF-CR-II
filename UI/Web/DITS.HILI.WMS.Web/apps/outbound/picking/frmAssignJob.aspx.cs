using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.PickingModel;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.picking
{
    public partial class frmAssignJob : BaseUIPage
    {
        private readonly PalletInfo palletInfo = new PalletInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData(Request.QueryString["pickingID"]);
        }

        protected void btnPicking_Click(object sender, DirectEventArgs e)
        {
            Guid pickingGUID = Guid.Empty;
            Guid.TryParse(hdPickingID.Text, out pickingGUID);

            X.Call("popitup", $"../../Report/frmReportViewer.aspx?reportName=RPT_PickingList&Silent={false}&PickingID={pickingGUID}");
        }

        protected void btnAssign_Click(object sender, DirectEventArgs e)
        {
            Guid pickingGUID = Guid.Empty;
            Guid.TryParse(hdPickingID.Text, out pickingGUID);

            X.Call("popitup", $"../../Report/frmReportViewer.aspx?reportName=RPT_AssingForm&Silent={false}&PickingID={pickingGUID}");
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string resMsg = string.Empty;
            if (Save(e, ref resMsg))
            {
                X.Call("parent.App.direct.Reload", resMsg);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(resMsg == string.Empty ? GetMessage("MSG00004").MessageValue : resMsg);
            }
        }

        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {
            string resMsg = string.Empty;
            if (Save(e, ref resMsg))
            {
                Guid.TryParse(hdPickingID.Text, out Guid pickingID);

                AssignJobModel assignJobModel = new AssignJobModel()
                {
                    PickingID = pickingID
                };

                if (assignJobModel == null)
                {
                    return;
                }

                Core.Domain.ApiResponseMessage apiResult = PickingClient.Approve(assignJobModel).Result;
                resMsg = apiResult.ResponseMessage;

                if (apiResult != null && apiResult.IsSuccess)
                {
                    X.Call("parent.App.direct.Reload", resMsg);
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
                else
                {
                    MessageBoxExt.ShowError(resMsg == string.Empty ? GetMessage("MSG00004").MessageValue : resMsg);
                }

            }
            else
            {
                MessageBoxExt.ShowError(resMsg == string.Empty ? GetMessage("MSG00004").MessageValue : resMsg);
            }
        }

        private bool Save(DirectEventArgs e, ref string resMsg)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<AssignJobDetailModel> gridData = JSON.Deserialize<IEnumerable<AssignJobDetailModel>>(gridJson);

            if (!ValidateSave(gridData, ref resMsg))
            {
                return false;
            }

            AssignJobModel assignJobModel = GetToModel(gridData);

            if (assignJobModel == null)
            {
                return false;
            }

            Core.Domain.ApiResponseMessage apiResult = PickingClient.Save(assignJobModel).Result;
            resMsg = apiResult.ResponseMessage;
            return apiResult != null ? apiResult.IsSuccess : false;
        }

        private bool ValidateSave(IEnumerable<AssignJobDetailModel> gridData, ref string resMsg)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(txtPONo.Text))
            {
                result = false;
                resMsg = GetMessage("MSG00051").MessageValue; // PO No cannot be null
            }

            if (gridData.Any(x => x.OrderPick == null))
            {
                result = false;
                resMsg = GetMessage("MSG00049").MessageValue; // Order Pick Must not be null
            }

            if (gridData.GroupBy(x => x.OrderPick).Any(g => g.Count() > 1))
            {
                result = false;
                resMsg = GetMessage("MSG00050").MessageValue; // Order Pick can't duplicate
            }

            if (gridData.Any(x => x.PalletQTY == null))
            {
                result = false;
                resMsg = GetMessage("MSG00024").MessageValue; // Some Data Missing
            }

            return result;
        }

        private AssignJobModel GetToModel(IEnumerable<AssignJobDetailModel> gridData)
        {
            Guid.TryParse(hdPickingID.Text, out Guid pickingID);
            Guid.TryParse(cmbEmployee.SelectedItem.Value, out Guid empAssignID);

            AssignJobModel picking = new AssignJobModel()
            {
                PickingID = pickingID,
                PickingCode = txtPickingCode.Text,
                DispatchCode = hdDispatchCode.Text,
                PickingDate = dtEntryDate.SelectedDate,
                OrderNo = txtOrderNo.Text,
                DocNo = txtDocNo.Text,
                PONo = txtPONo.Text,
                EmployeeAssignID = empAssignID,
                EmployeeAssign = cmbEmployee.SelectedItem.Text,
                PickingStatus = txtPickingStatus.Text,
                //PickingStatusEnums = (PickingStatusEnum)Enum.Parse(typeof(PickingStatusEnum), txtPickingStatus.Text),
                PickingStatusEnums = PickingStatusEnum.WaitingPick,
                ShippingCode = txtShippingCode.Text,
                Remark = txtRemark.Text,
            };

            picking.AssignJobDetailCollection = gridData;

            return picking;
        }

        private void BindData(string pickingID)
        {
            Guid pickingGUID = Guid.Empty;

            if (pickingID != null)
            {
                // in case Regis-Truck user cannot select PO
                txtSearchPO.ReadOnly = btnBrowsePO.Disabled = true;

                if (!Guid.TryParse(pickingID, out pickingGUID))
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00005"));
                    return;
                }

                Core.Domain.ApiResponseMessage apiResp = PickingClient.GetAssignJob(pickingGUID).Result;

                if (apiResp.IsSuccess)
                {
                    AssignJobModel data = apiResp.Get<AssignJobModel>();

                    if (data == null)
                    {
                        MessageBoxExt.ShowError(GetMessage("MSG00006"));
                        return;
                    }
                    BindDatatoControl(data);
                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }
            else
            {
                btnSave.Disabled = btnApprove.Disabled = true;
            }

        }

        private void BindDatatoControl(AssignJobModel data)
        {
            btnSave.Disabled = (data.PickingStatusEnums != PickingStatusEnum.WaitingPick);
            btnApprove.Disabled = (data.PickingStatusEnums != PickingStatusEnum.WaitingPick || string.IsNullOrWhiteSpace(data.PickingID.ToString()));

            hdPickingID.Text = data.PickingID.ToString();
            hdDispatchCode.Text = data.DispatchCode;

            txtShippingCode.Text = data.ShippingCode;
            txtPickingCode.Text = data.PickingCode;
            txtPONo.Text = data.PONo;
            dtEntryDate.SelectedDate = data.PickingDate.Value;
            txtDocNo.Text = data.DocNo;
            txtShippingTo.Text = data.ShipTo;
            txtShippingCode.Text = data.ShippingCode;
            txtOrderNo.Text = data.OrderNo;
            txtPickingStatus.Text = data.PickingStatus;
            txtRemark.Text = data.Remark;

            if (data.EmployeeAssignID != null)
            {
                cmbEmployee.InsertItem(0, data.EmployeeAssign, data.EmployeeAssignID);
                cmbEmployee.Select(data.EmployeeAssignID);
            }

            if (data.AssignJobDetailCollection != null && data.AssignJobDetailCollection.Count() > 0)
            {
                //if (string.IsNullOrWhiteSpace(data.ShippingCode))
                //{
                //    // From PO
                //    var suggestList = SuggestPallet(data);
                //    StoreOfDataList.Data = suggestList;
                //    StoreOfDataList.DataBind();
                //}
                //else
                //{
                //    // From Register Truck
                //    StoreOfDataList.Data = data.AssignJobDetailCollection.ToList();
                //    StoreOfDataList.DataBind();
                //}

                StoreOfDataList.Data = data.AssignJobDetailCollection.OrderBy(e=>e.ProductCode).ToList();
                StoreOfDataList.DataBind();

            }

            #region Handle Case

            if (data.PickingStatusEnums == PickingStatusEnum.Pick || data.PickingStatusEnums == PickingStatusEnum.Complete)
            {
                btnPicking.Enable();
                btnAssign.Enable();
            }
            else
            {
                btnPicking.Disable();
                btnAssign.Disable();
            }

            #endregion

        }

        private AssignJobModel GetAssignJobByPO(string PONo)
        {
            int total = 0;
            AssignJobModel data = null;

            Core.Domain.ApiResponseMessage apiResp = PickingClient.GetAssignJobByPO(PONo).Result;
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<AssignJobModel>();
            }

            return data;
        }

        private List<DispatchforAssignJobModel> GetDispatchList()
        {
            int total = 0;
            List<DispatchforAssignJobModel> data = new List<DispatchforAssignJobModel>();

            Core.Domain.ApiResponseMessage apiResp = PickingClient.GetDispatchforAssignJob(txtSearchPO.Text).Result;
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<DispatchforAssignJobModel>>();
            }

            return data;
        }

        private static List<AssignJobDetailModel> SuggestPallet(AssignJobModel data)
        {
            List<AssignJobDetailModel> suggestList = new List<AssignJobDetailModel>();

            var groupProducts = data.AssignJobDetailCollection
                                        .GroupBy(x => new { _ID = x.ProductID })
                                        .Select(x => new { ID = x.Key._ID });

            foreach (var product in groupProducts)
            {
                // Assign Job group by productID
                IOrderedEnumerable<AssignJobDetailModel> AJGroupbyProduct = data.AssignJobDetailCollection.Where(x => x.ProductID == product.ID).OrderBy(x => x.PalletQTY);

                for (int i = 1; i <= AJGroupbyProduct.Count(); i++)
                {
                    IEnumerable<AssignJobDetailModel> suggestPallet = AJGroupbyProduct.Take(i);
                    decimal? sumPalletQTY = suggestPallet.Sum(x => x.PalletQTY);
                    decimal? orderQTY = AJGroupbyProduct.FirstOrDefault().OrderQTY;

                    if (sumPalletQTY >= orderQTY)
                    {
                        suggestList.AddRange(suggestPallet);
                        break;
                    }
                }
            }

            return suggestList;
        }

        [DirectMethod(Timeout=180000)]
        public void btnBrowsePO_Click()
        {
            List<DispatchforAssignJobModel> listData = GetDispatchList();

            if (listData.Count == 1)
            {
                DispatchforAssignJobModel dispatch = listData.FirstOrDefault();

                AssignJobModel assignJob = GetAssignJobByPO(dispatch.PONo);

                if (assignJob != null)
                {
                    BindDatatoControl(assignJob);
                }
                else
                {
                    // PO not found
                    MessageBoxExt.Warning(GetMessage("MSG00046").MessageValue);
                }
            }
            else
            {
                ucDispatchforAssignJob.Show(listData);
            }

        }

        [DirectMethod(Timeout=180000)]
        public void ucDispatch_Select(string record)
        {
            DispatchforAssignJobModel data = JSON.Deserialize<DispatchforAssignJobModel>(record);

            txtSearchPO.Text = data.PONo;
            txtSearchShipto.Text = data.ShiptoName;

            AssignJobModel assignJob = GetAssignJobByPO(data.PONo);

            if (assignJob != null)
            {
                BindDatatoControl(assignJob);
            }
            else
            {
                // PO not found
                MessageBoxExt.Warning(GetMessage("MSG00046").MessageValue);
            }

            ucDispatchforAssignJob.Close();
        }

        [DirectMethod(Timeout=180000)]
        public object DispatchSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucDispatchforAssignJob.DispatchSelectBindData(action, extraParams);
        }

        [DirectMethod(Timeout=180000)]
        public void ShowErrorX(string msg_code)
        {
            MessageBoxExt.ShowError(GetMessage(msg_code));
        }
        [DirectMethod(Timeout=180000)]
        public PalletInfoModel UpdateSuggestLocation(PalletInfo palletInfo)
        {
            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetPalletInfo(palletInfo.PalletCode, palletInfo.OldPalletCode, palletInfo.OldProductID, palletInfo.OrderQTY).Result;

            if (apiResp.IsSuccess)
            {
                PalletInfoModel data = apiResp.Get<PalletInfoModel>();

                if (data == null)
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00006"));
                    return new PalletInfoModel();
                }

                return data;
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
                return null;
            }
        }
    }
}