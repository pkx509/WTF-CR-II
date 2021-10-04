using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.Web.Common.UI;
using DITS.HILI.WMS.Web.Common.Util;
using DITS.WMS.Common.Extensions;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.tools.cyclecount
{
    public partial class frmCycleCountList : BaseUIPage
    {
        private readonly string AppDataService = "../../../Common/DataClients/MsDataHandler.ashx";
        #region Initail
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cmbState();
                dtStartDate.SelectedDate = DateTime.Now;
                dtEndDate.SelectedDate = DateTime.Now;
            }

        }
        #endregion

        #region Event Handle
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new", "", "");
        }

        private void GetAddEditForm(string oDataKeyId, string oDataStatusId, string oDataStatusName)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("CYCLE_COUNT");

            WindowShow.showOperation(this, strTitle, "CreateCycleCount", "frmCycleCount.aspx?oDataKeyId=" + oDataKeyId, iconWindows);
        }

        protected async void CommandClick(object sender, DirectEventArgs e)
        {
            string status = e.ExtraParams["CycleCountStatus"];
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oCustomerCode = e.ExtraParams["oCustomerCode"];

            try
            {
                if (command == "Edit")
                {
                    GetAddEditForm(oDataKeyId, oCustomerCode, "");
                }
                else
                {
                    if ((status == CycleCountStatusEnum.Complete.ToString()) || (status == CycleCountStatusEnum.Approve.ToString()) || (status == CycleCountStatusEnum.Cancel.ToString()))
                    {
                        X.MessageBox.Show(new MessageBoxConfig
                        {
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Warning",
                            Buttons = MessageBox.Button.OK,
                            Message = "Job cycle count status cannot delete !"
                        });
                    }

                    if (command.ToLower() == "delete")
                    {
                        Core.Domain.ApiResponseMessage ok = InventoryToolsClient.Remove(oDataKeyId).Result;
                        if (ok.ResponseCode == "0")
                        {
                            NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                            PagingToolbar1.MoveFirst();
                        }
                        else
                        {
                            NotificationExt.Show(ok.ResponseCode, ok.ResponseMessage);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                X.MessageBox.Show(new MessageBoxConfig
                {
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Warning",
                    Buttons = MessageBox.Button.OK,
                    Message = ex.Message
                });
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            int total = 0;
            List<CycleCountModel> data = new List<CycleCountModel>();
            object output = new { data, total };
            if (!ValidateDateInput.Validate_Start_End_Date(dtStartDate.SelectedDate, dtEndDate.SelectedDate, out string errorMsg))
            {
                MessageBoxExt.ShowError(errorMsg);
                return output;
            }

            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetAll(dtStartDate.SelectedDate,
                                                      dtEndDate.SelectedDate,
                                                      int.Parse(cmbStatus.SelectedItem.Value),
                                                      txtSearch.Text,
                                                      prms.Page,
                                                      int.Parse(cmbPageList.SelectedItem.Value)).Result;
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<CycleCountModel>>();
                output = new { data, total };
            }

            return output;
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["CycleCountCode"];

            if (string.IsNullOrWhiteSpace(oDataKeyId))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }
            GetAddEditForm(oDataKeyId, "", "");
        }

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

        protected void cmbState_Change(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void cmbState()
        {
            cmbStatus.SetAutoCompleteValue(
            new List<CustomEnumerable>
                {
                    new CustomEnumerable
                    {
                       ID = 10,
                       Value = CycleCountStatusEnum.New.Description()
                    }
                });
        }

        #endregion
    }
}