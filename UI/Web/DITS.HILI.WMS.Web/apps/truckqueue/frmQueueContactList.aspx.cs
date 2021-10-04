using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmQueueContactList : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombo();
            }
            if (!X.IsAjaxRequest && !IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }
        private void LoadCombo()
        {
            txtSearchDockStatus.Items.Add(new Ext.Net.ListItem
            {
                Text = "ใช้งาน",
                Value = "1"
            });
            txtSearchDockStatus.Items.Add(new Ext.Net.ListItem
            {
                Text = "ไม่ใช้งาน",
                Value = "0"
            });
            txtSearchDockStatus.InsertItem(0, "ทั้งหมด", -1);
            txtSearchDockStatus.Select(1);
        }
        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["DataKeyId"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }
            GetAddEditForm(oDataKeyId);
        }
        [Ext.Net.DirectMethod(Timeout = 900000)]
        public object BindData(string oDataKeyId)
        {
            //, Dictionary<string, object> extraParams
            // Ext.Net.StoreRequestParameters prms = new Ext.Net.StoreRequestParameters(extraParams);
            int total = 0;
            int status = -1;
            if (!int.TryParse(txtSearchDockStatus.SelectedItem.Value, out status))
            {
                status = -1;
            }
            Core.Domain.ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetQueueTypeAll(status, txtSearchDock.Text).Result;
            List<QueueRegisterType> data = new List<QueueRegisterType>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<QueueRegisterType>>();
            }
            return new { data, total };
        }
        protected void Store_Refresh(object sender, EventArgs e)
        {
            try
            {
                PagingToolbar1.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(ex.ToString());
            }
        }

        private void GetAddEditForm(string oDataKeyId)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") + " " + GetResource("QUEUECONTACT") : GetResource("EDIT") + " " + GetResource("QUEUECONTACT");
            WindowShow.Show(this, strTitle, "Create Contact Type", "Editor/frmContactAddEdit.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 450, 200);
        }
        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("new");
        }
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
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
            else if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueueTypeById(Id).Result;
                QueueRegisterType _data = data.Get<QueueRegisterType>();
                if (_data == null)
                {
                    return;
                }
                System.Threading.Tasks.Task<Core.Domain.ApiResponseMessage> apiResp = ClientService.Queue.QueueClient.RemoveQueueType(_data);
                if (apiResp.Result.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiResp.Result.ResponseMessage);
                }
                else
                {
                    NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
            }
        }
        [DirectMethod(Timeout = 180000)]
        public void Reload()
        {
            NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout = 180000)]
        public void Exit()
        {
            PagingToolbar1.MoveFirst();
        }
    }
}