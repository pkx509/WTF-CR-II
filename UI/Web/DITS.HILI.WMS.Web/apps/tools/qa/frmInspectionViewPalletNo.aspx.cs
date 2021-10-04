using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.ProductionControlModel;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.tools.qa
{
    public partial class frmInspectionViewPalletNo : BaseUIPage
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
            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetPalletList(new Guid(oDataKeyId)).Result;

            List<PC_PackedModel> listData = new List<PC_PackedModel>();

            if (apiResp.IsSuccess)
            {

                listData = apiResp.Get<List<PC_PackedModel>>();
            }
            StoreOfDataList.DataSource = listData;
            StoreOfDataList.DataBind();
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
        }

        [DirectMethod(Timeout=180000)]
        public object ValidateProdcut(string product_code, string product_sys_code)
        {
            return new { valid = true, msg = "" };
        }
    }
}