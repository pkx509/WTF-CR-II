using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa
{
    public partial class frmInspectionDispatch : BaseUIPage
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
            List<ItemReclassified> listData = new List<ItemReclassified>();
            if (oDataKeyId == "new")
            {
                Session.Add("ItemReclassified", listData);
                txtReclassCode.Text = oDataKeyId;
                txtReclassStatus.Text = ReclassifiedStatus.Reclassified.Description();
                btnApprove.Visible = false;
            }
            else
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetInspectionDispatch(new Guid(oDataKeyId)).Result;

                if (apiResp.IsSuccess)
                {

                    Reclassified _data = apiResp.Get<Reclassified>();
                    StoreOfDataList.DataSource = _data.ReclassifiedItem;
                    StoreOfDataList.DataBind();

                    Session.Add("ItemReclassified", _data.ReclassifiedItem);

                    hddReclassId.Text = _data.ReclassifiedID.ToString();
                    txtReclassCode.Text = _data.ReclassifiedCode;
                    txtReclassStatus.Text = _data.ReclassStatus.Description();
                    dtApproveDate.Text = _data.ApproveDispatchDate == null ? "" : _data.ApproveDate.Value.ToString("dd/MM/yyyy");
                    txtDesc.Text = _data.Description;


                    cmbProductStatusTo.InsertItem(0, _data.ProductStatus, _data.ProductStatusID);
                    cmbProductStatusTo.Select(_data.ProductStatusID);


                    cmbProductStatusEdit.InsertItem(0, _data.FromProductStatus, _data.FromProductStatusID);
                    cmbProductStatusEdit.Select(_data.FromProductStatusID);



                    cmbProductStatusEdit.ReadOnly = true;
                    cmbProductStatusTo.ReadOnly = true;

                    if (_data.ReclassStatus == ReclassifiedStatus.Approve || _data.ReclassStatus == ReclassifiedStatus.SendtoReprocess)
                    {
                        dtApproveDate.ReadOnly = false;
                        btnApprove.Enabled = true;
                        btnDispatchDamage.Enabled = true;
                        btnDispatch.Enabled = true;
                    }
                    else
                    {
                        dtApproveDate.ReadOnly = true;
                        btnApprove.Disabled = true;
                        btnDispatchDamage.Disabled = true;
                        btnDispatch.Disabled = true;
                    }
                }
                else
                {
                    Session.Add("ItemReclassified", listData);
                    StoreOfDataList.DataSource = listData;
                    StoreOfDataList.DataBind();
                }
            }
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            if (command == "Delete")
            {
                X.Call("deleteProduct('" + oDataKeyId + "')");
            }
        }



        [DirectMethod(Timeout=180000)]
        public object ValidateProdcut(string product_code, string product_sys_code)
        {



            return new { valid = true, msg = "" };
        }

        protected void btnDispatchRepair_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ItemReclassified> gridData = JSON.Deserialize<List<ItemReclassified>>(gridJson).Where(x => x.IsReprocess == true && x.ReprocessQty > 0).ToList();


            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage ok = InventoryToolsClient.SendDispatchtoReprocess(gridData).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                bindData(hddReclassId.Text);
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }

        protected void btnDispatchDamage_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ItemReclassified> gridData = JSON.Deserialize<List<ItemReclassified>>(gridJson).Where(x => x.IsReject == true && x.RejectQty > 0).ToList();

            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage ok = InventoryToolsClient.SendDispatchtoDamage(gridData).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                bindData(hddReclassId.Text);
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
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
            List<ItemReclassified> gridDataList = JSON.Deserialize<List<ItemReclassified>>(gridJson);

            Reclassified reclass = new Reclassified
            {
                ReclassifiedID = new Guid(hddReclassId.Text),
                ApproveDispatchDate = dtApproveDate.SelectedDate,
                Description = txtDesc.Text
            };

            reclass.ReclassifiedItem = gridDataList;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.ApproveInspectionDispatch(reclass).Result;

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
}