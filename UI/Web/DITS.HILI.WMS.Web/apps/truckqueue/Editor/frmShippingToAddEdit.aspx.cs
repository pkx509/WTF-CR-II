using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmShippingToAddEdit : BaseUIPage
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
                ShippingFrom queueDock = new ShippingFrom()
                {
                    Name = txtName.Text,
                    ShortName = txtShortName.Text,
                    Address = txtAddress.Text,
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
                    datasave = ClientService.Queue.QueueClient.AddShippingFrom(queueDock).Result;
                }
                else
                {
                    queueDock.ShipFromId = new Guid(id);
                    datasave = ClientService.Queue.QueueClient.ModifyShippingFrom(queueDock).Result;
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
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetShippingFromById(id).Result;
                ShippingFrom _data = data.Get<ShippingFrom>();
                if (_data != null)
                {
                    txtName.Text = _data.Name;
                    txtShortName.Text = _data.ShortName;
                    txtAddress.Text = _data.Address;
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