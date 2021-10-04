using DITS.HILI.WMS.ClientService.ProductionControl;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.ProductionControlModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.ProductionControl
{
    public partial class frmRe_PrintPalletList : BaseUIPage
    {
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

            LineTypeEnum lineType = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), hdLineType.Text);

            Core.Domain.ApiResponseMessage apiResp = ProductionControlClient.GetAllPacked(lineType
                                , dtPlanDate.SelectedDate
                                , lineID
                                , txtPallet.Text
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
    }
}