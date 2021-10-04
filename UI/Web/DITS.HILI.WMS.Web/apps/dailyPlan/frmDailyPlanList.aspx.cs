using DITS.HILI.WMS.ClientService.DailyPlan;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.Web.Common.UI;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.dailyPlan
{
    public partial class frmDailyPlanList : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            string section;

            hdSection.Text = section = Request.QueryString["section"].ToString();

            // only first time yeahhhhh!!
            if (IsPostBack)
            {
                return;
            }

            if (Request.QueryString.Count == 0)
            {
                return;
            }

            if (section != "NP" && section != "SP")
            {
                MessageBoxExt.ShowError(GetMessage("MSG00030"));
                return;
            }

            dtStartDate.SelectedDate = DateTime.Now;
            dtEndDate.SelectedDate = DateTime.Now;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string productionDetailID = e.ExtraParams["productionDetailID"];

            if (command == "Edit")
            {
                GetAddEditForm(productionDetailID);
            }
            else
            {
                if (!Guid.TryParse(productionDetailID, out Guid productionDetailGUID))
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00005"));
                    return;
                }

                List<Guid> pDetailGUIDs = new List<Guid>
                {
                    productionDetailGUID
                };

                Core.Domain.ApiResponseMessage ok = ImportProductionClient.DeletePlan(pDetailGUIDs).Result;
                if (ok.IsSuccess)
                {
                    NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    MessageBoxExt.ShowError(ok.ResponseMessage);
                }
            }
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string productionDetailID = e.ExtraParams["productionDetailID"];
            if (string.IsNullOrEmpty(productionDetailID))
            {
                return;
            }

            GetAddEditForm(productionDetailID);

        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ProductionPlanCustomModel> gridData = JSON.Deserialize<List<ProductionPlanCustomModel>>(gridJson);

            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            List<Guid> pDetailGUIDs = new List<Guid>();
            pDetailGUIDs.AddRange(gridData.Where(x => x.ProductionDetailID != null).Select(x => x.ProductionDetailID.Value).ToList());

            Core.Domain.ApiResponseMessage ok = ImportProductionClient.DeletePlan(pDetailGUIDs).Result;
            if (ok.IsSuccess)
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                PagingToolbar1.MoveFirst();
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }

        private void GetAddEditForm(string productionDetailID)
        {
            string strTitle = (productionDetailID == "new") ? "Add" : "Edit" + " Production Plan";
            WindowShow.ShowNewPage(this, strTitle, "DailyPlan"
                , "frmDailyPlan.aspx?productionDetailID=" + productionDetailID + "&section=" + hdSection.Text
                , Icon.Newspaper);
        }

        protected void btnToReceive_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<ProductionPlanCustomModel> gridData = JSON.Deserialize<List<ProductionPlanCustomModel>>(gridJson);

            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage ok = ImportProductionClient.SendtoReceive(gridData).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                PagingToolbar1.MoveFirst();
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindLineData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            List<Line> data = new List<Line>();
            LineTypeEnum sectionEnum = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), hdSection.Text);
            Core.Domain.ApiResponseMessage Line = ProductionLineClient.GetLine(prms.Query, false, sectionEnum, prms.Page, prms.Limit).Result;

            if (Line.IsSuccess)
            {
                total = Line.Totals;
                data = Line.Get<List<Line>>();
            }
            return new { data, total };
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            Guid lineGUID = Guid.Empty;
            int total = 0;
            List<ProductionPlanCustomModel> data = new List<ProductionPlanCustomModel>();
            object output = new { data, total };

            btnSend.Disabled = btnDelete.Disabled = chkIsReceive.Checked;

            if (cmbLine.SelectedItem.Value != null)
            {
                if (!Guid.TryParse(cmbLine.SelectedItem.Value, out lineGUID))
                {
                    MessageBoxExt.ShowError(GetMessage("MSG00032"));
                    return output;
                }
            }

            try
            {
                if (!ValidateDateInput.Validate_Start_End_Date(dtStartDate.SelectedDate, dtEndDate.SelectedDate, out string errorMsg))
                {
                    MessageBoxExt.ShowError(errorMsg);
                    return output;
                }

                StoreRequestParameters prms = new StoreRequestParameters(extraParams);
                StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                LineTypeEnum sectionEnum = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), hdSection.Text);

                Core.Domain.ApiResponseMessage apiResp = ImportProductionClient.SearchPlan(dtStartDate.SelectedDate
                                                                , dtEndDate.SelectedDate
                                                                , lineGUID, sectionEnum
                                                                , chkIsReceive.Checked
                                                                , txtProduct.Text
                                                                , prms.Page
                                                                , int.Parse(cmbPageList.SelectedItem.Value)).Result;

                if (apiResp.IsSuccess)
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<ProductionPlanCustomModel>>();
                    if (data.Count() == 0)
                    {
                        StoreOfDataList.RemoveAll();
                        StoreOfDataList.DataBind();
                        return output;
                    }

                    output = new { data, total };
                }
                else
                {
                    MessageBoxExt.ShowError(string.Join(", ", apiResp.ResponseMessage));
                }

                return output;

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.Message);
                return output;
            }
        }

        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
    }
}