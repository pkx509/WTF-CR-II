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
    public partial class frmInspectionReClassified : BaseUIPage
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
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetInspectionReclassified(new Guid(oDataKeyId)).Result;

                if (apiResp.IsSuccess)
                {

                    Reclassified _data = apiResp.Get<Reclassified>();
                    StoreOfDataList.DataSource = _data.ReclassifiedItem;
                    StoreOfDataList.DataBind();

                    Session.Add("ItemReclassified", _data.ReclassifiedItem);

                    hddReclassId.Text = _data.ReclassifiedID.ToString();
                    txtReclassCode.Text = _data.ReclassifiedCode;
                    txtReclassStatus.Text = _data.ReclassStatus.Description();
                    dtApproveDate.Text = _data.ApproveDate == null ? "" : _data.ApproveDate.Value.ToString("dd/MM/yyyy");
                    txtDesc.Text = _data.Description;


                    cmbProductStatusTo.InsertItem(0, _data.ProductStatus, _data.ProductStatusID);
                    cmbProductStatusTo.Select(_data.ProductStatusID);


                    cmbProductStatusEdit.InsertItem(0, _data.FromProductStatus, _data.FromProductStatusID);
                    cmbProductStatusEdit.Select(_data.FromProductStatusID);

                    cmbProductStatusEdit.ReadOnly = true;


                    if (_data.ReclassStatus == ReclassifiedStatus.Approve || _data.ReclassStatus == ReclassifiedStatus.SendtoReprocess || _data.ReclassStatus == ReclassifiedStatus.ApproveDispatch)
                    {
                        dtApproveDate.ReadOnly = true;
                        btnApprove.Visible = false;
                        btnSave.Visible = false;
                        cmbProductStatusTo.ReadOnly = true;
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

        [DirectMethod(Timeout=180000)]
        public void GetProduct(string _product_code)
        {
            if (string.IsNullOrEmpty(cmbProductStatusEdit.SelectedItem.Value))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00032"));
                return;
            }

            ucQAPalletMultiSelect.Show(_product_code, cmbProductStatusEdit.SelectedItem.Value);
        }

        [DirectMethod(Timeout=180000)]

        public void ucPalletTag_MultiSelect(string record)
        {
            List<PalletTagModel> pallets = JSON.Deserialize<List<PalletTagModel>>(record);

            List<ItemReclassified> listData = (List<ItemReclassified>)Session["ItemReclassified"];
            List<ItemReclassified> tmp = new List<ItemReclassified>();

            foreach (PalletTagModel pallet in pallets)
            {
                tmp.Add(new ItemReclassified()
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

            tmp.AddRange(listData);

            var groups = tmp.GroupBy(g => new
            {
                g.MFGDate,
                g.LineCode,
                g.Lot,
                g.ProductID
            }).ToList();


            if (groups.Count > 1)
            {
                MessageBoxExt.Warning(GetMessage("MSG00080").MessageValue);
                return;
            }

            List<ItemReclassified> tmpData = new List<ItemReclassified>();

            foreach (PalletTagModel pallet in pallets)
            {
                tmpData.Add(new ItemReclassified()
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
            StoreOfDataList.Add(tmpData);
            StoreOfDataList.CommitChanges();

            //
            ucQAPalletMultiSelect.Close();
            cmbProductStatusEdit.ReadOnly = true;
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
            List<ItemReclassified> gridDataList = JSON.Deserialize<List<ItemReclassified>>(gridJson);

            if (cmbProductStatusTo.SelectedItem.Value == cmbProductStatusEdit.SelectedItem.Value)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00091"));

                return;
            }
            gridDataList.ForEach(item =>
            {
                item.ProductStatusID = new Guid(cmbProductStatusTo.SelectedItem.Value);
                item.FromProductStatusID = new Guid(cmbProductStatusEdit.SelectedItem.Value);
                item.ApproveDate = dtApproveDate.SelectedDate;
                item.Description = txtDesc.Text;
            });


            if (txtReclassCode.Text == "new")
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.AddInspectionReclassified(gridDataList).Result;

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
            else
            {
                Reclassified reclass = new Reclassified
                {
                    ReclassifiedID = new Guid(hddReclassId.Text),
                    ApproveDate = dtApproveDate.SelectedDate,
                    ProductStatusID = new Guid(cmbProductStatusTo.SelectedItem.Value),
                    Description = txtDesc.Text
                };
                reclass.ReclassifiedItem = gridDataList;

                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.SaveInspectionReclassified(reclass).Result;

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

            gridDataList.ForEach(item =>
            {
                item.ProductStatusID = new Guid(cmbProductStatusTo.SelectedItem.Value);
                item.FromProductStatusID = new Guid(cmbProductStatusEdit.SelectedItem.Value);
                item.ApproveDate = dtApproveDate.SelectedDate;
                item.Description = txtDesc.Text;
            });

            Reclassified reclass = new Reclassified
            {
                ReclassifiedID = new Guid(hddReclassId.Text),
                ApproveDate = dtApproveDate.SelectedDate,
                ProductStatusID = new Guid(cmbProductStatusTo.SelectedItem.Value),
                Description = txtDesc.Text
            };
            reclass.ReclassifiedItem = gridDataList;

            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.ApproveInspectionReclassified(reclass).Result;

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