using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.outbound
{
    public partial class RePrintPalletOutboundList : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            List<PC_PackedModel> data = new List<PC_PackedModel>();

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            if (string.IsNullOrWhiteSpace(cmbPONo.SelectedItem.Text))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00051")); // PO No cannot be null
                return new { data, total };
            }

            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetRePrintOutboundList((DateTime?)dtMFGDate.SelectedValue
                                                                , txtProductName.Text
                                                                , cmbPONo.SelectedItem.Text
                                                                , prms.Page
                                                                , int.Parse(cmbPageList.SelectedItem.Value)).Result;

            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<PC_PackedModel>>();
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }

            return new { data, total };
        }

        protected void CommandRePrintRemainClick(object sender, DirectEventArgs e)
        {
            CallPrintForm(e);
        }

        protected void CommandRePrintDOClick(object sender, DirectEventArgs e)
        {
            CallPrintForm(e);
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            CallPrintForm(e);
        }

        private void CallPrintForm(DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string packingID = e.ExtraParams["packingID"];
            string pickingDetailID = e.ExtraParams["pickingDetailID"];
            string printType = e.ExtraParams["printType"];
            string printerName = cmbPrinterLocation.SelectedItem.Text;
            string width = "0";
            string height = "0";
            bool printSilent = !chkPDF.Checked;


            if (string.IsNullOrWhiteSpace(printerName) && printSilent)
            {
                MessageBoxExt.Warning(GetMessage("MSG00065").MessageValue);
                return;
            }

            if (Guid.TryParse(packingID, out Guid packingGUID))
            {
                X.Call("popitup", $"../Report/frmReportViewer.aspx?reportName=RPT_PalletTags&Silent={printSilent}&PackingID={packingID}&PickingDetailID={pickingDetailID}&PrintType={printType}&PrinterName={printerName}&width={width}&height={height}");
            }
        }
    }
}