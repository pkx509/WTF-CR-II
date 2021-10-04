using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.ProductionControl
{
    public partial class frmPCList : BaseUIPage
    {
        private static LineTypeEnum _LineType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            string lineType = Request.QueryString["section"].ToString();

            if (lineType != "NP" && lineType != "SP")
            {
                MessageBoxExt.ShowError(GetMessage("MSG00030"));
                return;
            }

            hdLineType.SetValue(lineType);
            dtPlanDate.SelectedDate = DateTime.Now;
            cmbLine.FieldLabel = grdPacking.ColumnModel.Columns[4].Text = lineType == "NP" ? GetResource("LINE") : GetResource("MACHINE");
        }

        protected void CommandPackingClick(object sender, DirectEventArgs e)
        {
            string controlID = e.ExtraParams["controlID"];

            if (!Guid.TryParse(controlID, out Guid controlGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            GetAddEditForm(controlGUID);

        }

        protected void CommandPackedClick(object sender, DirectEventArgs e)
        {
            string controlID = e.ExtraParams["controlID"];

            if (!Guid.TryParse(controlID, out Guid controlGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            GetRePrintForm(controlGUID);
        }

        private void GetRePrintForm(Guid controlGUID)
        {
            string strTitle = GetResource("REPRINTPALLET");
            WindowShow.ShowNewPage(this, strTitle, "RePrintPalletTag", "frmRe-PrintPallet.aspx?controlID=" + controlGUID.ToString(), Icon.Printer);
        }

        private void GetAddEditForm(Guid controlGUID)
        {
            string strTitle = GetResource("EDIT") + " " + GetResource("PRINTPALLET");

            if (hdLineType.Text == "NP")
            {
                WindowShow.ShowNewPage(this, strTitle, "PrintPalletTag", "frmPrintPalletNP.aspx?controlID=" + controlGUID.ToString(), Icon.Add);
            }
            else
            {
                WindowShow.ShowNewPage(this, strTitle, "PrintPalletTag", "frmPrintPalletSP.aspx?controlID=" + controlGUID.ToString(), Icon.Add);
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindPackingData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            Guid? lineID = null;

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StorePacking.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            if (Guid.TryParse(cmbLine.SelectedItem.Value, out Guid tmpLineID))
            {
                lineID = tmpLineID;
            }

            _LineType = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), hdLineType.Text);

            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetAllPacking(_LineType
                                , dtPlanDate.SelectedDate
                                , lineID
                                , ""
                                , prms.Page
                                , int.Parse(cmbPageList.SelectedItem.Value)).Result;

            List<PC_PackingModel> data = new List<PC_PackingModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<PC_PackingModel>>();
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }

            return new { data, total };
        }

        [DirectMethod(Timeout=180000)]
        public object BindPackedData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            Guid? lineID = null;

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StorePacked.PageSize = int.Parse(cmbPageList2.SelectedItem.Value);

            if (Guid.TryParse(cmbLine.SelectedItem.Value, out Guid tmpLineID))
            {
                lineID = tmpLineID;
            }

            _LineType = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), hdLineType.Text);

            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetAllPacked(_LineType
                                , dtPlanDate.SelectedDate
                                , lineID
                                , ""
                                , prms.Page
                                , int.Parse(cmbPageList2.SelectedItem.Value)).Result;

            List<PC_PackedModel> data = new List<PC_PackedModel>();
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

        protected void grdPacking_CellDblClick(object sender, DirectEventArgs e)
        {
            string controlID = e.ExtraParams["controlID"];

            if (!Guid.TryParse(controlID, out Guid controlGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }
            GetAddEditForm(controlGUID);
        }

        protected void grdPacked_CellDblClick(object sender, DirectEventArgs e)
        {
            string controlID = e.ExtraParams["controlID"];

            if (!Guid.TryParse(controlID, out Guid controlGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }
            GetRePrintForm(controlGUID);
        }

        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, param);
            }
            else
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
        }

        [DirectMethod(Timeout=180000)]
        public void RefreshGrid()
        {
            PagingToolbar1.MoveFirst();
            PagingToolbar2.MoveFirst();
        }
    }
}