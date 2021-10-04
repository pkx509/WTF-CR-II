using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.tools.adjust
{
    public partial class frmAdjustList : BaseUIPage
    {
        #region Initail
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getState();
            }
        }
        #endregion

        #region Event Handle
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }

        private void GetAddEditForm(string oDataKeyId)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("ADJUSTT");

            WindowShow.showOperation(this, strTitle, "CreateAdjust", "frmAdjust.aspx?oDataKeyId=" + oDataKeyId, iconWindows);
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
                    GetAddEditForm(oDataKeyId);
                }
                else
                {
                    if ((status == "77") || (status == "99"))
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
                        Core.Domain.ApiResponseMessage ok = InventoryToolsClient.RemoveAdjust(oDataKeyId).Result;
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

                    Ext.Net.Notification.Show(new NotificationConfig
                    {
                        Icon = Icon.Information,
                        Title = "Information",
                        Html = "Delete complete"
                    });

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

        [DirectMethod(Timeout = 180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetAdjustAll(cmbStatus.SelectedItem.Value, txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<AdjustModel> data = new List<AdjustModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<AdjustModel>>();
            }

            return new { data, total };
        }

        protected async void getState()
        {
            try
            {
                cmbStatus.Items.Clear();
                Array values = Enum.GetValues(typeof(AdjustStatusEnum));

                List<Ext.Net.ListItem> items = new List<Ext.Net.ListItem>(values.Length);

                foreach (object i in values)
                {
                    Ext.Net.ListItem l = new Ext.Net.ListItem
                    {
                        Text = Enum.GetName(typeof(AdjustStatusEnum), i),
                        Value = i.ToString()
                    };
                    cmbStatus.Items.Add(l);
                }

                cmbStatus.Items.Add(new Ext.Net.ListItem { Value = "0", Text = "- Select All -" });
                cmbStatus.SelectedItem.Index = 0;
                PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex);
            }
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            GetAddEditForm(oDataKeyId);
        }

        protected void cmbState_Change(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }
        #region "DirectMethod"
        [DirectMethod(Timeout = 180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }

        #endregion
        #endregion
    }
}