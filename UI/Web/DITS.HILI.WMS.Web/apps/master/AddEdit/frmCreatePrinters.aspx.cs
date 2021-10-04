using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Data.CustomModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master.AddEdit
{
    public partial class frmCreatePrinters : BaseUIPage
    {

        public static string ProgramCode = frmPrinters.ProgramCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        private void BindData(string oDataKeyId)
        {
            try
            {
                if (oDataKeyId == "new")
                {
                    return;
                }

                Guid id = new Guid(oDataKeyId);
                Core.Domain.ApiResponseMessage data = ClientService.Master.PrinterClient.GetByID(id).Result;
                Printer _data = data.Get<Printer>();
                if (_data == null)
                {
                    return;
                }

                txtPrinterID.Text = oDataKeyId;
                txtPrintersLocation.Text = _data.PrinterLocation;
                txtDescription.Text = _data.Description;
                cmbPrintersLocation.SetAutoCompleteValue(
                    new List<Line>
                    {
                            new Line
                            {
                                LineID = _data.PrinterLocationId.Value,
                                LineCode = _data.PrinterLocation

                            }
                    }, _data.PrinterLocationId.Value.ToString()
                );

                cmbPrinterName.SetAutoCompleteValue(
                 new List<BasePrinter>
                    {
                            new BasePrinter
                            {
                                PrinterName =_data.PrinterName
                            }
                    }, _data.PrinterName
               );

            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Guid lineGUID = Guid.Empty;
                Printer _printer = new Printer();

                if (!string.IsNullOrWhiteSpace(cmbPrintersLocation.SelectedItem.Value))
                {
                    if (!Guid.TryParse(cmbPrintersLocation.SelectedItem.Value, out lineGUID))
                    {
                        MessageBoxExt.ShowError(GetMessage("MSG00005").MessageValue);
                        return;
                    }
                }

                string id = Request.QueryString["oDataKeyId"];

                if (id != "new")
                {
                    _printer.PrinterId = new Guid(id);
                }

                _printer.PrinterName = cmbPrinterName.SelectedItem.Value;
                _printer.PrinterLocationId = lineGUID;
                _printer.PrinterLocation = cmbPrintersLocation.SelectedItem.Text;
                _printer.Description = txtDescription.Text;
                _printer.IsDefault = false;
                _printer.IsActive = true;

                bool isSuccess = true;
                if (id == "new")
                {

                    Core.Domain.ApiResponseMessage datasave = ClientService.Master.PrinterClient.Add(_printer).Result;

                    if (datasave.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }

                }

                else
                {
                    Core.Domain.ApiResponseMessage datamodify = ClientService.Master.PrinterClient.Modify(_printer).Result;
                    if (datamodify.ResponseCode != "0")
                    {
                        isSuccess = false;
                        MessageBoxExt.ShowError(datamodify.ResponseMessage);
                    }

                }


                if (isSuccess)
                {
                    X.Call("parent.App.direct.Reload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }

        }


        protected void btnExit_Click(object sender, EventArgs e)
        {
            X.Call("parent.App.direct.Exit");
            X.AddScript("parent.Ext.WindowMgr.getActive().close();");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            FormPanelDetail.Reset();
            BindData(Request.QueryString["oDataKeyId"]);
        }

    }
}