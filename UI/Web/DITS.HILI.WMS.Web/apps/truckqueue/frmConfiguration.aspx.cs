using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmConfiguration : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                TimeSpan time = dtStartTime.SelectedTime;
                ApiResponseMessage datasave = new ApiResponseMessage();
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetConfigurationActive().Result;
                QueueConfiguration _data = data.Get<QueueConfiguration>();
                if (_data != null)
                {
                    _data.Message = txtAnnounce.Text;
                    _data.EnableMessage = ckbIsActive.Checked;
                    datasave = ClientService.Queue.QueueClient.ModifyConfiguration(_data).Result;
                }
                else
                {
                    _data = new QueueConfiguration()
                    {
                        ConfigurationID = Guid.NewGuid(),
                        Message = txtAnnounce.Text,
                        EnableMessage = ckbIsActive.Checked
                    };
                    datasave = ClientService.Queue.QueueClient.AddConfiguration(_data).Result;
                } 
                 ClientService.Queue.QueueClient.SentAnounceChange(_data);
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }
        protected void btnSaveQstart_Click(object sender, DirectEventArgs e)
        {
            try
            {
                TimeSpan time = dtStartTime.SelectedTime;
                ApiResponseMessage datasave = new ApiResponseMessage();
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetConfigurationActive().Result;
                QueueConfiguration _data = data.Get<QueueConfiguration>();
                if (_data != null)
                { 
                    _data.StartHour = time.Hours;
                    _data.StartMinute = time.Minutes;
                    datasave = ClientService.Queue.QueueClient.ModifyConfiguration(_data).Result;
                }
                else
                {
                    _data = new QueueConfiguration()
                    {
                        ConfigurationID = Guid.NewGuid(),
                        EnableMessage = false,
                        IsActive = true,
                        Message = string.Empty,
                        StartHour = time.Hours,
                        StartMinute = time.Minutes
                    };
                    datasave = ClientService.Queue.QueueClient.AddConfiguration(_data).Result;
                }
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }
        private void BindData()
        {
            try
            {
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetConfigurationActive().Result;
                QueueConfiguration _data = data.Get<QueueConfiguration>();
                if (_data != null)
                {
                    txtAnnounce.Text = _data.Message;
                    ckbIsActive.Checked = _data.EnableMessage;
                    dtStartTime.SelectedTime = new TimeSpan(_data.StartHour, _data.StartMinute, 0);
                }  
            }
            catch
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            FormPanelDetail.Reset();
            BindData();
        }
        protected void btnClearQstart_Click(object sender, EventArgs e)
        {
            FormPanelDetail2.Reset();
            BindData();
        }
    }
}