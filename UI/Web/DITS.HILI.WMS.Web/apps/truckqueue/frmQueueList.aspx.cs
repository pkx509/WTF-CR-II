using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmQueueList : BaseUIPage
    {
        private string url = ConfigurationManager.AppSettings["QueueService"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest && !IsPostBack)
            {
                LoadCombo();
                dtStartDate.SelectedDate = DateTime.Today.AddDays(-1);
                dtEndDate.SelectedDate = DateTime.Today;

                dtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
                dtEndDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                grdDataList.Margin = 0;
            } 
        }
        private void LoadCombo()
        {
            Core.Domain.ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetQueueStatusAll(null, string.Empty).Result;
            var data = apiResp.Get<List<QueueStatus>>();
            foreach (var item in data)
            {
                txtSearchDockStatus.Items.Add(new Ext.Net.ListItem
                {
                    Text = item.QueueStatusName,
                    Value = item.QueueStatusID.ToString()
                });
            }
            txtSearchDockStatus.InsertItem(0, "ทั้งหมด", Guid.Empty);
            txtSearchDockStatus.Select(0);
        }
        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
           // string oDataKeyId = e.ExtraParams["DataKeyId"];
          //  if (string.IsNullOrEmpty(oDataKeyId))
           // {
                return;
            //}
            //GetAddEditForm(oDataKeyId);
        }
        [Ext.Net.DirectMethod(Timeout = 900000)]
        public object BindData(string oDataKeyId, Dictionary<string, object> extraParams)
        {
            int total = 0;
            QueueStatusEnum status =  QueueStatusEnum.All;
            if (txtSearchDockStatus.SelectedItem.Value != null)
            {
                if (!Enum.TryParse(txtSearchDockStatus.Value.ToString(), out QueueStatusEnum queueStatus))
                {
                    status = QueueStatusEnum.All;
                } 
            }
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);  
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetSearchQueue(dtStartDate.SelectedDate, dtEndDate.SelectedDate, status, txtSearchKeyword.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            //  Core.Domain.ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetQueueAll(status, txtSearchKeyword.Text).Result;
            List<QueueReg> data = new List<QueueReg>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<QueueReg>>();
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
        private void GetAddEditForm(string oDataKeyId, string command)
        {
            Ext.Net.Icon iconWindows = Ext.Net.Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Ext.Net.Icon.ApplicationFormAdd;
            }
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") + " " + GetResource("QUEUEREGISTER") : GetResource("EDIT") + " " + GetResource("QUEUEREGISTER");
            if (command.ToLower() == "changestatus")
            {
                strTitle = GetResource("QUEUECHANGESTS");
            }
            WindowShow.Show(this, strTitle, "Create Queue", "Editor/frmQueueAddEdit.aspx?oDataKeyId=" + oDataKeyId + "&command=" + command, iconWindows, 480, 400);
        }
        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("new", "new");
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
                GetAddEditForm(oDataKeyId, command.ToLower());
            }
            else if (command.ToLower() == "call")
            {
                Core.Domain.ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueue(Id).Result;
                QueueReg _data = data.Get<QueueReg>();
                if (_data == null)
                {
                    return;
                }
                if (_data.QueueStatusID == (int)QueueStatusEnum.Completed)
                {
                    //ClientService.Queue.QueueClient.SentCallCompletedQueue(_data).Wait();
                }
                else
                {
                    data = ClientService.Queue.QueueClient.CallQueue(_data).Result;
                    if (data.ResponseCode != "0")
                    {
                        MessageBoxExt.ShowError(data.ResponseMessage);
                    }
                    else
                    {
                        ClientService.Queue.QueueClient.SentCallQueue(_data);
                        NotificationExt.Show(GetMessage("QUEUE00002").MessageTitle, GetMessage("QUEUE00002").MessageValue);
                        PagingToolbar1.MoveFirst();
                    }
                }
            }
            else if (command.ToLower() == "changestatus")
            {
                GetAddEditForm(oDataKeyId, command.ToLower());
            }
            else if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueue(Id).Result;
                QueueReg _data = data.Get<QueueReg>();
                if (_data == null)
                {
                    return;
                }
                System.Threading.Tasks.Task<Core.Domain.ApiResponseMessage> apiResp = ClientService.Queue.QueueClient.RemoveQueue(_data);
                if (apiResp.Result.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(apiResp.Result.ResponseMessage);
                }
                else
                {
                    ClientService.Queue.QueueClient.SentCallRefreshQueue();
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