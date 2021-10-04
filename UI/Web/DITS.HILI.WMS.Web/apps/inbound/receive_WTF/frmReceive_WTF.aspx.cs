using DITS.HILI.WMS.ClientService.Inbound;
using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.inbound.receive_WTF
{
    public partial class frmReceive_WTF : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData(Request.QueryString["receiveID"]);
            X.Js.Call("sumTotal");
        }

        private void BindData(string receiveID)
        {
            Guid receiveGUID = Guid.Empty;

            if (!Guid.TryParse(receiveID, out receiveGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            Core.Domain.ApiResponseMessage apiResp = ReceiveClient.GetReceiveByID(receiveGUID).Result;

            if (apiResp.IsSuccess)
            {
                ReceiveHeaderModel data = apiResp.Get<ReceiveHeaderModel>();

                if (data == null)
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00006"));
                    return;
                }

                BindtoControl(data);
                BindtoGrid(data.ReceiveDetails);
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }
        }

        private void BindtoControl(ReceiveHeaderModel data)
        {
            if (data.ReceiveStatus != ReceiveStatusEnum.New)
            {
                txtInvoiceNo.ReadOnly = txtContainerNo.ReadOnly = txtPONo.ReadOnly = true;
                txtOrderNo.ReadOnly = txtRemark.ReadOnly = chkUrgent.ReadOnly = true;
                dtEstReceiveDate.ReadOnly = true;
                cmbReceiveType.ReadOnly = cmbLocation.ReadOnly = cmbProductOwner.ReadOnly = true;

                txtAreaRemark.ReadOnly = true;
                cmbProductStatusEdit.ReadOnly = true;
                cmbProductUnitEdit.ReadOnly = true;
                nbMFGDateEdit.ReadOnly = true;
                nbEXPDateEdit.ReadOnly = true;

                btnProduce.Hidden = true;
            }

            hdReceiveID.SetValue(data.ReceiveID);
            hdLineID.Text = data.LineID.ToString();
            txtReceiveCode.Text = data.ReceiveCode;
            txtSupplier.Text = data.SupplierName;
            txtInvoiceNo.Text = data.InvoiceNo;
            txtContainerNo.Text = data.ContainerNo;
            txtPONo.Text = data.PONo;
            txtOrderNo.Text = data.OrderNo;
            txtRemark.Text = data.Remark;

            chkUrgent.Checked = data.IsUrgent ?? false;

            dtEstReceiveDate.SelectedDate = data.ESTReceiveDate.Value;

            #region bindCombo

            if (data.ReceiveTypeID != null)
            {
                cmbReceiveType.InsertItem(0, data.ReceiveType, data.ReceiveTypeID);
                cmbReceiveType.Select(data.ReceiveTypeID);
            }

            if (data.LocationID != null)
            {
                cmbLocation.InsertItem(0, data.Location, data.LocationID);
                cmbLocation.Select(data.LocationID);
            }

            if (data.ProductOwnerID != null)
            {
                cmbProductOwner.InsertItem(0, data.ProductOwner, data.ProductOwnerID);
                cmbProductOwner.Select(data.ProductOwnerID);
            }

            #endregion
        }

        private void BindtoGrid(IEnumerable<ReceiveDetailModel> data)
        {
            if (data != null)
            {
                StoreOfDataList.Data = data.ToList();
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);
            ReceiveHeaderModel receieve = GetToModel(gridData);
            if (receieve == null)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005").MessageValue);
                return;
            }

            if (!receieve.ESTReceiveDate.HasValue || receieve.ESTReceiveDate.Value == DateTime.MinValue)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00004").MessageValue, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }
            Core.Domain.ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(receieve.ESTReceiveDate.Value).Result;
            if (apiresponse.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }

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

        private bool Save(DirectEventArgs e, ref string resMsg)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);
            ReceiveHeaderModel receieve = GetToModel(gridData); 
            if (receieve == null)
            {
                return false;
            }
           
            Core.Domain.ApiResponseMessage apiResult = ReceiveClient.Save(receieve).Result;
            resMsg = apiResult.ResponseMessage;
            return apiResult.IsSuccess;
        }

        protected void btnSendToProductionControl_Click(object sender, DirectEventArgs e)
        {
            string resMsg = string.Empty;
            Guid receiveGUID = Guid.Empty;

            if (!Guid.TryParse(hdReceiveID.Text, out receiveGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005").MessageValue);
                return;
            }

            if (receiveGUID == Guid.Empty)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005").MessageValue);
                return;
            }
            if (!Save(e, ref resMsg))
            {
                MessageBoxExt.ShowError(resMsg == string.Empty ? GetMessage("MSG00004").MessageValue : resMsg);
                return;
            }
            string gridJson = e.ExtraParams["ParamStorePages"];
            IEnumerable<ReceiveDetailModel> gridData = JSON.Deserialize<IEnumerable<ReceiveDetailModel>>(gridJson);
            ReceiveHeaderModel receieve = GetToModel(gridData);
            if (receieve == null)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005").MessageValue);
                return;
            }

            if (!receieve.ESTReceiveDate.HasValue || receieve.ESTReceiveDate.Value == DateTime.MinValue)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00004").MessageValue, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }
            Core.Domain.ApiResponseMessage apiresponse = MonthEndClient.CheckCutoffDate(receieve.ESTReceiveDate.Value).Result;
            if (apiresponse.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(apiresponse.ResponseMessage, MessageBox.Button.OK, Icon.Error, MessageBox.Icon.WARNING);
                return;
            }
            List<Guid> receiveIDs = new List<Guid>
            {
                receiveGUID
            };
            Core.Domain.ApiResponseMessage apiResult = ReceiveClient.SendtoProductionControl(receiveIDs).Result;

            if (apiResult.IsSuccess)
            {
                X.Call("parent.App.direct.Reload", apiResult.ResponseMessage);
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }
            else
            {
                MessageBoxExt.ShowError(GetMessage("MSG00027").MessageValue);
            }
        }

        private ReceiveHeaderModel GetToModel(IEnumerable<ReceiveDetailModel> detail)
        {
            Guid receiveTypeID, productOwnerID, locationID, receiveGUID;
            receiveGUID = receiveTypeID = productOwnerID = locationID = Guid.Empty;

            Guid.TryParse(hdReceiveID.Text, out receiveGUID);
            Guid.TryParse(cmbReceiveType.SelectedItem.Value, out receiveTypeID);
            Guid.TryParse(cmbProductOwner.SelectedItem.Value, out productOwnerID);
            Guid.TryParse(cmbLocation.SelectedItem.Value, out locationID);

            if (receiveTypeID == Guid.Empty || productOwnerID == Guid.Empty || locationID == Guid.Empty || receiveGUID == Guid.Empty)
            {
                return null;
            }

            ReceiveHeaderModel receive = new ReceiveHeaderModel()
            {
                ReceiveID = receiveGUID,
                ReceiveTypeID = receiveTypeID,
                ProductOwnerID = productOwnerID,
                LocationID = locationID,
                ESTReceiveDate = dtEstReceiveDate.SelectedDate,
                InvoiceNo = txtInvoiceNo.Text,
                ContainerNo = txtContainerNo.Text,
                PONo = txtPONo.Text,
                OrderNo = txtOrderNo.Text,
                Remark = txtRemark.Text,
                IsUrgent = chkUrgent.Checked
            };

            receive.ReceiveDetails = detail;

            return receive;
        }

        [DirectMethod(Timeout=180000)]
        public void LoadEditCombo(string unitID, string unit, string statusID, string status)
        {
            cmbProductUnitEdit.InsertItem(0, unit, unitID);
            cmbProductUnitEdit.Select(unitID);

            cmbProductStatusEdit.InsertItem(0, status, statusID);
            cmbProductStatusEdit.Select(statusID);
        }
        //[DirectMethod(Timeout=180000)]
        //public bool ValidateEstReceiveDate(DateTime EstReceiveDate)
        //{
        //    try
        //    {
        //        var apiresponse = ClientService.Master.MonthEndClient.CheckCutoffDate(EstReceiveDate).Result;
        //        if (apiresponse.ResponseCode != "0")
        //        {
        //            MessageBoxExt.ShowError(apiresponse.ResponseMessage);
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxExt.ShowError(ex);
        //        return false;
        //    }
        //}
    }
}