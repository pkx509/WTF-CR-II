using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.ProductionControl
{
    public partial class frmRe_PrintPallet : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            //BindData(Request.QueryString["controlID"]);
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            string controlID = Request.QueryString["controlID"];
            List<PC_PackedModel> data = new List<PC_PackedModel>();

            if (!Guid.TryParse(controlID, out Guid controlGUID))
            {
                MessageBoxExt.Warning(GetMessage("MSG00029").MessageValue);
                return new { data, data.Count };
            }

            ApiResponseMessage apiResp = ProductionControlClient.GetRePrintList(controlGUID, chkIsProduction.Checked).Result;
            if (apiResp.ResponseCode == "0")
            {
                data = apiResp.Get<List<PC_PackedModel>>();

                Guid? lineID = data.Where(x => x.LineID != null).FirstOrDefault()?.LineID;
                if (lineID != null)
                {
                    hdLineID.SetValue(lineID.ToString());
                }

                cmbPrinterLocation.Show();
            }
            return new { data, data.Count };
        }
        private void BindDataXX(string controlID)
        {

            try
            {
                if (!Guid.TryParse(controlID, out Guid controlGUID))
                {
                    MessageBoxExt.Warning(GetMessage("MSG00029").MessageValue);
                    return;
                }

                ApiResponseMessage apiResp = ProductionControlClient.GetRePrintList(controlGUID, chkIsProduction.Checked).Result;

                if (apiResp.IsSuccess)
                {
                    List<PC_PackedModel> data = apiResp.Get<List<PC_PackedModel>>();

                    if (data == null)
                    {
                        MessageBoxExt.Warning(GetMessage("MSG00006").MessageValue);
                        return;
                    }

                    BindToGrid(data);
                }
                else
                {
                    MessageBoxExt.ShowError(string.Join(", ", apiResp.ResponseMessage));
                }
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
            }
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            //BindData(Request.QueryString["controlID"]);
        }

        private void BindToGrid(List<PC_PackedModel> data)
        {
            if (data != null)
            {
                StoreOfDataList.DataSource = data;
                StoreOfDataList.Data = data;

                Guid? lineID = data.Where(x => x.LineID != null).FirstOrDefault()?.LineID;
                if (lineID != null)
                {
                    hdLineID.SetValue(lineID.ToString());
                }

                StoreOfDataList.Reload();
            }
        }

        protected void CommandCancelPalletClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string packingID = e.ExtraParams["packingID"];
            string palletCode = e.ExtraParams["palletCode"];

            string strTitle = "Comfirm Cancel Pallet";
            WindowShow.Show(this, strTitle, "CancelPallet"
                , $"frmCancelPallet.aspx?packingID={ packingID }&palletCode={ palletCode }"
                , Icon.Newspaper, 400, 250);

        }

        protected void CommandRePrintRemainClick(object sender, DirectEventArgs e)
        {
            CallPrintForm(e);
        }

        protected void CommandRePrintClick(object sender, DirectEventArgs e)
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
            string printerName = cmbPrinterLocation.SelectedItem.Text;
            string width = txtWidth.Text;
            string height = txtHeight.Text;
            string printType = e.ExtraParams["printType"];
            Guid pkDetailID = Guid.Empty;
            bool printSilent = !chkPDF.Checked;


            if (string.IsNullOrWhiteSpace(printerName) && printSilent)
            {
                MessageBoxExt.Warning(GetMessage("MSG00065").MessageValue);
                return;
            }

            if (Guid.TryParse(packingID, out Guid packingGUID))
            {
                X.Call("popitup", $"../Report/frmReportViewer.aspx?reportName=RPT_PalletTags&Silent={printSilent}&PackingID={packingID}&PickingDetailID={pkDetailID}&PrintType={printType}&PrinterName={printerName}&width={width}&height={height}");
            }
        }

        [DirectMethod(Timeout=180000)]
        public void Reload(ApiResponseMessage apiResp)
        {
            if (!string.IsNullOrWhiteSpace(apiResp.ResponseMessage))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }

            CancelPalletModel data = apiResp.Get<CancelPalletModel>();
            if (data == null)
            {
                BindDataXX(data.ControlID.ToString());
            }
        }
    }
}