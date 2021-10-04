using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.ReceiveModel;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa
{
    public partial class frmInspectionGoodsReturn : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                bindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void bindData(string oDataKeyId)
        {

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetInspectionGoodsReturn(new Guid(oDataKeyId)).Result;



            List<ItemGoodsReturn> listData = new List<ItemGoodsReturn>();

            if (apiResp.IsSuccess)
            {

                GoodsReturn _data = apiResp.Get<GoodsReturn>();
                StoreOfDataList.DataSource = _data.ItemGoodsReturns;
                StoreOfDataList.DataBind();

                hddGoodsReturnID.Text = _data.GoodsReturnID.ToString();
                txtPono.Text = _data.PONumber;
                txtReceiveCode.Text = _data.ReceiveCode;
                txtGoodsReturnStatus.Text = _data.GoodsReturnStatus.Description();
                dtApproveDate.Text = _data.ApproveDate == null ? "" : _data.ApproveDate.Value.ToString("dd/MM/yyyy");
                txtDesc.Text = _data.Description;
                txtReceiveDate.Text = _data.ReceiveDate.ToString("dd/MM/yyyy");


                if (_data.GoodsReturnStatus == GoodsReturnStatusEnum.QA_Approve || _data.GoodsReturnStatus == GoodsReturnStatusEnum.SendtoReprocess)
                {
                    dtApproveDate.ReadOnly = true;
                    btnApprove.Visible = false;
                    btnSave.Visible = false;
                }
                else
                {
                    btnInspectionDamage.Disabled = true;
                    btnInspectionRepair.Disabled = true;
                }
            }
        }

        private void GetAddEditForm(string id)
        {
            string strTitle = GetResource("VIEW") + " " + GetResource("GOODSRETURNPALLET_VIEW");
            WindowShow.ShowNewPage(this, strTitle, "InspectionViewPalletNo", "frmInspectionViewPalletNo.aspx?oDataKeyId=" + id, Icon.ApplicationFormMagnify);
        }

        [DirectMethod(Timeout=180000)]
        public void GetProduct(string _product_code)
        {
            //if(string.IsNullOrEmpty(cmbProductStatusEdit.SelectedItem.Value))
            //{
            //    MessageBoxExt.ShowError(GetMessage("MSG00032"));
            //    return;
            //}

            //this.ucQAPalletMultiSelect.Show(_product_code, cmbProductStatusEdit.SelectedItem.Value);
        }

        [DirectMethod(Timeout=180000)]
        public void ucPalletTag_MultiSelect(string record)
        {
            List<PalletTagModel> pallets = JSON.Deserialize<List<PalletTagModel>>(record);

            List<ItemReclassified> listData = new List<ItemReclassified>();

            foreach (PalletTagModel pallet in pallets)
            {
                listData.Add(new ItemReclassified()
                {
                    Location = pallet.Location,
                    PalletCode = pallet.PalletCode,
                    ProductName = pallet.ProductName,
                    PalletQty = pallet.Qty.Value,
                    ReclassifiedQty = pallet.Qty.Value,
                    UnitName = pallet.UnitName,
                    ProductCode = pallet.ProductCode,
                    Lot = pallet.LotNo,
                    MFGDate = pallet.MFGDate,
                    LineCode = pallet.LineCode,
                    ProductStatusID = pallet.ProductStatusID,
                    ProductID = pallet.ProductID,
                    ReclassifiedDetailID = Guid.NewGuid()
                });
            }

            StoreOfDataList.Add(listData);
            StoreOfDataList.CommitChanges();

            ucQAPalletMultiSelect.Close();
            //cmbProductStatusEdit.ReadOnly = true;
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            if (command == "Delete")
            {
                X.Call("deleteProduct('" + oDataKeyId + "')");
            }
            else
            {
                GetAddEditForm(oDataKeyId);
            }
        }

        [DirectMethod(Timeout=180000)]
        public object ProductSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            return ucQAPalletMultiSelect.ProductSelectBindData(action, extraParams);
        }

        [DirectMethod(Timeout=180000)]
        public object ValidateProdcut(string product_code, string product_sys_code)
        {
            return new { valid = true, msg = "" };
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ItemGoodsReturn> gridDataList = JSON.Deserialize<List<ItemGoodsReturn>>(gridJson);

            gridDataList.ForEach(item =>
            {
                item.ApproveDate = dtApproveDate.SelectedDate;
                item.Description = txtDesc.Text;
            });
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.SaveInspectionGoodsReturn(gridDataList).Result;

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

        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {
            if (dtApproveDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("Please Select Approve Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtApproveDate').focus('', 10); ");
                return;
            }
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ItemGoodsReturn> gridDataList = JSON.Deserialize<List<ItemGoodsReturn>>(gridJson);

            gridDataList.ForEach(item =>
            {
                item.ApproveDate = dtApproveDate.SelectedDate;
                item.Description = txtDesc.Text;
            });
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.ApproveInspectionGoodsReturn(gridDataList).Result;

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


        protected void btnInspectionRepair_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ItemGoodsReturn> gridData = JSON.Deserialize<List<ItemGoodsReturn>>(gridJson).Where(x => x.IsReprocess == true).ToList();


            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage ok = InventoryToolsClient.SendGoodsReturntoReprocess(gridData).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                bindData(hddGoodsReturnID.Text);
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }

        protected void btnInspectionDamage_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ItemGoodsReturn> gridData = JSON.Deserialize<List<ItemGoodsReturn>>(gridJson).Where(x => x.IsReject == true).ToList();

            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage ok = InventoryToolsClient.SendGoodsReturntoDamage(gridData).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                bindData(hddGoodsReturnID.Text);
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }

    }
}