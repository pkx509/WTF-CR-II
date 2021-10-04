using DITS.HILI.WMS.ClientService.DailyPlan;
using DITS.HILI.WMS.DailyPlanModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllProductionLine : BaseUIPage
    {
        private static LineTypeEnum _SectionEnum;
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            if (IsPostBack)
            {
                return;
            }

            cmbLineType.SelectedItem.Value = "NP";
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            if (cmbLineType.SelectedItem.Value != null)
            {
                _SectionEnum = (LineTypeEnum)Enum.Parse(typeof(LineTypeEnum), cmbLineType.SelectedItem.Value);
            }

            Core.Domain.ApiResponseMessage apiResp = ProductionLineClient.GetLine(txtSearch.Text, ckbIsActive.Checked, _SectionEnum, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<Line> data = new List<Line>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<Line>>();
            }

            return new { data, total };
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }

        private void GetAddEditForm(string oDataKeyId)
        {
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("PRODUCTION_LINE");
            WindowShow.Show(this, strTitle, "CreateProductionLine", "AddEdit/frmCreateProductionLine.aspx?oDataKeyId=" + oDataKeyId, Icon.Add, 450, 270);
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId);
            }
            if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.DailyPlan.ProductionLineClient.Remove(Id).Result;
                if (ok.IsSuccess)
                {
                    //NotificationExt.Show("Delete", "Delete complete");
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                }
            }
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        //FormMasterList _FormMasterList = new FormMasterList();

        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        //protected void btnAdd_Click(object sender, DirectEventArgs e)
        //{
        //    this.GetAddEditForm("new");
        //}

        //private void GetAddEditForm(string oDataKeyId)
        //{
        //    WindowsPopup.show(this, "Add/Edit Production Line", ProgramCode + "-FORM",
        //                        "AddEdit/frmCreateProductionLine.aspx?oDataKeyId=" + oDataKeyId,
        //                        Icon.Add, 550, 270);
        //}

        //protected void CommandClick(object sender, DirectEventArgs e)
        //{
        //    string command = e.ExtraParams["command"];
        //    string oDataKeyId = e.ExtraParams["oDataKeyId"];

        //    if (command == "Edit")
        //    {
        //        GetAddEditForm(oDataKeyId);
        //    }
        //    else
        //    {
        //        //Delete
        //        DataServiceModel dataService = new DataServiceModel();
        //        dataService.Add<string>("Line_code", oDataKeyId);

        //        Results data = WebServiceHelper.Post<Results>("DeleteProductionLine", dataService.GetObject());

        //        if (data.result == true)
        //        {
        //            NotificationExt.Show(Resources.Langauge.Delete, MyToolkit.CustomMessage(data.message));
        //            this.PagingToolbar1.MoveFirst();
        //        }
        //        else
        //        {
        //            MessageBoxExt.ShowError(MyToolkit.CustomMessage(data.message));
        //        }
        //    }
        //}

        //        [DirectMethod(Timeout=180000)]
        //public object BindData(string action, Dictionary<string, object> extraParams)
        //{
        //    List<SearchModel> _searchModel = new List<SearchModel>();

        //    //_searchModel.Add(new SearchModel()
        //    //{
        //    //    Group = 1,
        //    //    Key = "Line_code",
        //    //    Operation = "LIKE",
        //    //    Value = this.txtSearch.Text,
        //    //    IsCondition = ConditionWhere.OR
        //    //});

        //    //_searchModel.Add(new SearchModel()
        //    //{
        //    //    Group = 1,
        //    //    Key = "Production_Type",
        //    //    Operation = "LIKE",
        //    //    Value = this.txtSearch.Text
        //    //});

        //    return _FormMasterList.BindData<ProductionLineModel>(action, extraParams, this.txtSearch.Text, this.grdDataList, "SYS_Get_Production_Line");
        //}

        //protected void Store_Refresh(object sender, EventArgs e)
        //{
        //    this.PagingToolbar1.MoveFirst();
        //}


        #region "DirectMethod"
        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
        #endregion

    }
}