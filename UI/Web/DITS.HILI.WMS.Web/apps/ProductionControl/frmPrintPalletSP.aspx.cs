using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.ProductionControl
{
    public partial class frmPrintPalletSP : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            ShowPopupShifts();
            BindData(Request.QueryString["controlID"]);
        }

        [DirectMethod(Timeout=180000)]
        public void SelectShifts(string shiftsNo)
        {
            hdShiftsNo.Text = shiftsNo;
        }

        private void ShowPopupShifts()
        {
            //กรุณาเลือกกะการทำงาน
            X.Msg.Confirm(GetMessage("MSG00031").MessageTitle, GetMessage("MSG00031").MessageValue, new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {

                    Handler = "App.direct.SelectShifts('S01')",
                    Text = "01"

                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "App.direct.SelectShifts('S02')",
                    Text = "02"
                }
            }).Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string printerName = cmbPrinterLocation.SelectedItem.Text;
            bool printSilent = !chkPDF.Checked;

            if (string.IsNullOrWhiteSpace(printerName) && printSilent)
            {
                MessageBoxExt.Warning(GetMessage("MSG00065").MessageValue);
                return;
            }

            if (nbQTYperPallet.Number != double.Parse(txtQTYperPallet.Text))
            {
                MessageBoxExt.Warning(GetMessage("MSG00032").MessageValue);
                return;
            }


            if (!Guid.TryParse(hdControlID.Text, out Guid controlGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            PrintPalletModel model = new PrintPalletModel()
            {
                ControlID = controlGUID,
                OptionalSuffix = !string.IsNullOrWhiteSpace(hdShiftsNo.Text) ? hdShiftsNo.Text : "S01",
                LineCode = txtLine.Text,
                QTYPerPallet = decimal.Parse(txtQTYperPallet.Text)
            };

            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.PrintPalletTag(model).Result;

            if (apiResp.IsSuccess)
            {
                PC_PackingModel data = apiResp.Get<PC_PackingModel>();

                if (data == null)
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00006"));
                    return;
                }

                BindtoControl(data);

                // print pallet tags
                if (data.PackingID != null)
                {
                    CallPrintForm(data.PackingID.Value);
                    MessageBoxExt.Warning("พิมพ์พาเลทสำเร็จ");
                }
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }


        }

        private void BindData(string controlID)
        {

            try
            {
                if (!Guid.TryParse(controlID, out Guid controlGUID))
                {
                    MessageBoxExt.Warning(GetMessage("MSG00029").MessageValue);
                    return;
                }

                hdControlID.SetValue(controlGUID);

                Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetByID(controlGUID).Result;

                if (apiResp.IsSuccess)
                {
                    PC_PackingModel data = apiResp.Get<PC_PackingModel>();

                    if (data == null)
                    {
                        MessageBoxExt.Warning(GetMessage("MSG00006").MessageValue);
                        return;
                    }

                    BindtoControl(data);
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

        private void BindtoControl(PC_PackingModel data)
        {
            decimal tempTotalQTY, tempQTYPerPallet, tempCurrentQTY;
            tempTotalQTY = tempQTYPerPallet = tempCurrentQTY = 0.0M;

            if (data.StdPalletQTY == 0)
            {
                X.Call("parent.App.direct.Reload", "Standard Pallety QTY " + GetMessage("MSG00058"));
                X.AddScript("parent.Ext.WindowMgr.getActive().close();");
            }

            decimal palletQTY = (data.QTY ?? 1) / (data.StdPalletQTY ?? 1);
            palletQTY = Math.Ceiling(palletQTY); // always rounding up

            #region Condition

            tempTotalQTY = data.QTY ?? 0;
            tempQTYPerPallet = data.StdPalletQTY ?? 0;
            tempCurrentQTY = (data.PalletCount ?? 0) + 1;

            if (data.IsLastestPallet ?? false)
            {
                //cmbUnit.ReadOnly = false;
                tempQTYPerPallet = (tempTotalQTY - (data.StdPalletQTY * (palletQTY - 1))) ?? 0;

                if (palletQTY < tempCurrentQTY)
                {
                    tempCurrentQTY = (data.PalletCount ?? 0);

                    cmbUnit.ReadOnly = true;
                    btnSave.Hide();
                    btnSave.Disable();
                }

            }

            #endregion

            hdProductID.Text = data.ProductID.ToString();
            txtProductCode.Text = data.ProductCode;
            txtProductName.Text = data.ProductName;
            txtPalletQTY.Text = palletQTY.ToString();
            txtTotalQTY.Text = tempTotalQTY.ToString();
            txtCurrentPallet.Text = tempCurrentQTY.ToString();
            txtQTYperPallet.Text = tempQTYPerPallet.ToString();
            nbQTYperPallet.Number = (double)tempQTYPerPallet;
            txtMFGDate.Text = data.StartDate.Value.ToString("dd-MM-yyyy");
            tmMFGTime.SelectedTime = data.StartTime ?? DateTime.Now.TimeOfDay;
            txtOrderNo.Text = data.OrderNo;
            txtOrderType.Text = data.OrderType;
            hdLineID.Text = data.LineID.ToString();
            txtLine.Text = data.LineCode;

            // combobox
            if (data.UnitID != null)
            {
                cmbUnit.InsertItem(0, data.Unit, data.UnitID);
                cmbUnit.Select(data.UnitID);
            }
        }

        private void CallPrintForm(Guid packingID)
        {
            string printerName = cmbPrinterLocation.SelectedItem.Text;
            string width = "0";
            string height = "0";
            string printType = "1";
            Guid pkDetailID = Guid.Empty;
            bool printSilent = !chkPDF.Checked;

            if (string.IsNullOrWhiteSpace(printerName) && printSilent)
            {
                MessageBoxExt.Warning(GetMessage("MSG00065").MessageValue);
                return;
            }

            //X.Call("popitup", $"../Report/frmReportViewer.aspx?reportName=RPT_PalletTags&Silent&PackingID&PickingDetailID={pkDetailID}&PrintType={printType}&PrinterName={printerName}&width={width}&height={height}");
            X.Call("popitup", $"../Report/frmReportViewer.aspx?reportName=RPT_PalletTags&Silent={printSilent}&PackingID={packingID}&PickingDetailID={pkDetailID}&PrintType={printType}&PrinterName={printerName}&width={width}&height={height}");
        }
    }
}