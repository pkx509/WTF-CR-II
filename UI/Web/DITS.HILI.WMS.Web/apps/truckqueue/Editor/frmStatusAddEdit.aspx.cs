using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmStatusAddEdit : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(Request.QueryString["oDataKeyId"]);
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string id = Request.QueryString["oDataKeyId"];
                QueueStatus queueDock = new QueueStatus()
                {
                    QueueStatusName = txtQueueStatusName.Text,
                    QueueStatusDesc = txtQueueStatusDesc.Text, 
                    IsActive = ckbIsActive.Checked,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Remark = "",
                    UserCreated = Guid.Empty,
                    UserModified = Guid.Empty
                };
                ApiResponseMessage datasave = new ApiResponseMessage();
                if (id == "new")
                {
                    datasave = ClientService.Queue.QueueClient.AddQueueStatus(queueDock).Result;
                }
                else
                {
                    int.TryParse(id, out int qid);
                    queueDock.QueueStatusID =qid;
                    datasave = ClientService.Queue.QueueClient.ModifyQueueStatus(queueDock).Result;
                }
                if (datasave.ResponseCode != "0")
                {
                    MessageBoxExt.ShowError(datasave.ResponseMessage);
                }
                else
                {
                    NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                    X.Call("parent.App.direct.Reload");
                    X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                }
            }
            catch (Exception)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
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
                int.TryParse(oDataKeyId, out int id);
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueueStatusById(id).Result;
                QueueStatus _data = data.Get<QueueStatus>();
                if (_data != null)
                {
                    txtQueueStatusDesc.Text = _data.QueueStatusDesc;
                    txtQueueStatusName.Text = _data.QueueStatusName;
                    //ckIsWaiting.Checked = _data.IsWaiting;
                    //ckIsInQueue.Checked = _data.IsInQueue;
                    //ckIsCompleted.Checked = _data.IsCompleted;
                    //ckIsCancel.Checked = _data.IsCancel;
                    ckbIsActive.Checked = _data.IsActive;
                }
                else
                {
                    txtQueueStatusDesc.Text = "";
                    txtQueueStatusName.Text = "";
                    ckIsWaiting.Checked = true;
                    ckIsInQueue.Checked = false;
                    ckIsCompleted.Checked = false;
                    ckIsCancel.Checked = false;
                    ckbIsActive.Checked = true;
                } 
            }
            catch
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999"));
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