using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmContactAddEdit : BaseUIPage
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
                QueueRegisterType queueDock = new QueueRegisterType()
                {
                    QueueRegisterTypeName = txtDockName.Text,
                    QueueRegisterTypeDesc = txtDockDesc.Text,
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
                    datasave = ClientService.Queue.QueueClient.AddQueueType(queueDock).Result;
                }
                else
                {
                    queueDock.QueueRegisterTypeID = new Guid(id);
                    datasave = ClientService.Queue.QueueClient.ModifyQueueType(queueDock).Result;
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

                Guid id = new Guid(oDataKeyId);
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueueTypeById(id).Result;
                QueueRegisterType _data = data.Get<QueueRegisterType>();
                if (_data != null)
                {
                    txtDockDesc.Text = _data.QueueRegisterTypeDesc;
                    txtDockName.Text = _data.QueueRegisterTypeName;
                    ckbIsActive.Checked = _data.IsActive;
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