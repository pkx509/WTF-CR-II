using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.TruckQueueModel;
using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.truckqueue
{
    public partial class frmQueueAddEdit : BaseUIPage
    { 
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!X.IsAjaxRequest && !IsPostBack)
            { 
                BindData(Request.QueryString["oDataKeyId"]); 
            }
        } 
        protected void btnSave_Click(object sender, DirectEventArgs e)
        { 
            try
            {
                string id = Request.QueryString["oDataKeyId"];
                string command = Request.QueryString["command"];

                QueueReg queueRegisteration = new QueueReg()
                {
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsActive = true,
                    PONO = txtPoNo.Text,
                    QueueDockID = !string.IsNullOrEmpty(cmbSelectDock.Value.ToString()) ? Guid.Parse(cmbSelectDock.Value.ToString()) : Guid.Empty,
                    QueueRegisterTypeID = !string.IsNullOrEmpty(cmdRegisterType.Value.ToString()) ? Guid.Parse(cmdRegisterType.Value.ToString()) : Guid.Empty,
                    ShipFromId = !string.IsNullOrEmpty(cmdShipFrom.Value.ToString()) ? Guid.Parse(cmdShipFrom.Value.ToString()) : Guid.Empty,
                    ShipToId = !string.IsNullOrEmpty(cmbShipTo.Value.ToString()) ? Guid.Parse(cmbShipTo.Value.ToString()) : Guid.Empty,
                    TruckTypeID = !string.IsNullOrEmpty(cmbTruckType.Value.ToString()) ? Guid.Parse(cmbTruckType.Value.ToString()) : Guid.Empty,
                   // TruckRegProviceId = !string.IsNullOrEmpty(cmbProvince.Value.ToString()) ? Guid.Parse(cmbProvince.Value.ToString()) : Guid.Empty,
                    Remark = txtRemark.Text,
                    TruckRegNo = txtTruckRegno.Text,
                    TimeIn = DateTime.Now
                };
                ApiResponseMessage datasave = new ApiResponseMessage();
                QueueReg _data = null;
                QueueStatusEnum queueStatus = QueueStatusEnum.Register;
                if (command == "changestatus")
                {
                    var statusId = cmbQueueStatus.Value.ToString();
                    if (!Enum.TryParse(statusId, out queueStatus))
                    {
                        queueStatus = QueueStatusEnum.Register;
                    }
                    var queuId = Guid.Parse(id);
                    ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueue(queuId).Result;
                    _data = data.Get<QueueReg>();
                    _data.QueueStatusID = (int)queueStatus;
                    datasave = ClientService.Queue.QueueClient.ChangeStatus(_data).Result; 
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
                else
                {                
                    if (command == "new")
                    {
                        datasave = ClientService.Queue.QueueClient.AddQueue(queueRegisteration).Result;
                    }
                    else
                    {
                        var queuId = Guid.Parse(id);
                        ApiResponseMessage apiResp = ClientService.Queue.QueueClient.GetQueue(queuId).Result;
                        var data = apiResp.Get<QueueReg>();
                        data.PONO = txtPoNo.Text;
                        data.QueueDockID = !string.IsNullOrEmpty(cmbSelectDock.Value.ToString()) ? Guid.Parse(cmbSelectDock.Value.ToString()) : Guid.Empty;
                        data.QueueRegisterTypeID = !string.IsNullOrEmpty(cmdRegisterType.Value.ToString()) ? Guid.Parse(cmdRegisterType.Value.ToString()) : Guid.Empty;
                        data.ShipFromId = !string.IsNullOrEmpty(cmdShipFrom.Value.ToString()) ? Guid.Parse(cmdShipFrom.Value.ToString()) : Guid.Empty;
                        data.ShipToId = !string.IsNullOrEmpty(cmbShipTo.Value.ToString()) ? Guid.Parse(cmbShipTo.Value.ToString()) : Guid.Empty;
                        data.TruckTypeID = !string.IsNullOrEmpty(cmbTruckType.Value.ToString()) ? Guid.Parse(cmbTruckType.Value.ToString()) : Guid.Empty;
                       // data.TruckRegProviceId = !string.IsNullOrEmpty(cmbProvince.Value.ToString()) ? Guid.Parse(cmbProvince.Value.ToString()) : Guid.Empty;
                        data.Remark = txtRemark.Text;
                        data.TruckRegNo = txtTruckRegno.Text;
                        datasave = ClientService.Queue.QueueClient.ModifyQueue(data).Result;
                    }
                    if (datasave.ResponseCode != "0")
                    {
                        MessageBoxExt.ShowError(datasave.ResponseMessage);
                    }
                    else
                    {
                        _data = datasave.Get<QueueReg>();
                        NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                        X.Call("parent.App.direct.Reload");
                        X.AddScript("parent.Ext.WindowMgr.getActive().close();");
                    }
                }
                if (datasave.ResponseCode == "0")
                {
                    if (queueStatus == QueueStatusEnum.Completed)
                    {
                       // ClientService.Queue.QueueClient.SentCallCompletedQueue(_data).Wait();
                    }
                    else
                    {
                        ClientService.Queue.QueueClient.SentCallRefreshQueue();
                    }
                }
            }
            catch
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
                    dtRegisDate.Value = DateTime.Today;
                    flQueueStatus.Visible = false;
                    return;
                } 
                Guid id = new Guid(oDataKeyId);
                ApiResponseMessage data = ClientService.Queue.QueueClient.GetQueue(id).Result;
                QueueReg _data = data.Get<QueueReg>();
                if (_data != null)
                {
                    dtRegisDate.Value = _data.DateCreated;
                    txtPoNo.Text = _data.PONO;
                    txtQueuNo.Text = _data.QueueNo;
                    txtRemark.Text = _data.Remark;
                    txtTruckRegno.Text = _data.TruckRegNo;
                    if (_data.QueueDockID != Guid.Empty)
                    {
                        QueueDock customer_edit = new QueueDock
                        {
                            QueueDockID = _data.QueueDockID.Value,
                            QueueDockName = _data.QueueDock
                        };
                        StoreQueueDock.Add(customer_edit);
                        cmbSelectDock.SelectedItem.Value = _data.QueueDockID.ToString();
                        cmbSelectDock.UpdateSelectedItems();
                    }
                    if (_data.TruckTypeID != Guid.Empty)
                    {
                        TruckType customer_edit = new TruckType
                        {
                            TruckTypeID = _data.TruckTypeID.Value,
                            TypeName = _data.TruckType
                        };
                        StoreTruckType.Add(customer_edit);
                        cmbTruckType.SelectedItem.Value = _data.TruckTypeID.ToString();
                        cmbTruckType.UpdateSelectedItems();
                    }
                    if (_data.QueueRegisterTypeID != Guid.Empty)
                    {
                        QueueRegisterType customer_edit = new QueueRegisterType
                        {
                            QueueRegisterTypeID = _data.QueueRegisterTypeID.Value,
                            QueueRegisterTypeName = _data.QueueRegisterType
                        };
                        StoreRegisterType.Add(customer_edit);
                        cmdRegisterType.SelectedItem.Value = _data.QueueRegisterTypeID.ToString();
                        cmdRegisterType.UpdateSelectedItems();
                    }
                    //if (_data.TruckRegProviceId != Guid.Empty)
                    //{
                    //    Province customer_edit = new Province
                    //    {
                    //        Province_Id = _data.TruckRegProviceId.Value,
                    //        Name = _data.TruckRegProvice
                    //    };
                    //    StoreProvince.Add(customer_edit);
                    //    cmbProvince.SelectedItem.Value = _data.TruckRegProviceId.ToString();
                    //    cmbProvince.UpdateSelectedItems();
                    //}
                    if (_data.ShipFromId != Guid.Empty)
                    {
                        ShippingFrom customer_edit = new ShippingFrom
                        {
                            ShipFromId = _data.ShipFromId.Value,
                            Name = _data.ShipFrom
                        };
                        StoreShipFrom.Add(customer_edit);
                        cmdShipFrom.SelectedItem.Value = _data.ShipFromId.ToString();
                        cmdShipFrom.UpdateSelectedItems();
                    }
                    if (_data.ShipToId != Guid.Empty)
                    {
                        ShippingTo customer_edit = new ShippingTo
                        {
                            ShipToId = _data.ShipToId.Value,
                            Name = _data.ShippTo
                        };
                        StoreShipTo.Add(customer_edit);
                        cmbShipTo.SelectedItem.Value = _data.ShipToId.ToString();
                        cmbShipTo.UpdateSelectedItems();
                    }

                    QueueStatus qt = new QueueStatus
                    {
                        QueueStatusID = _data.QueueStatusID,
                        QueueStatusName = _data.QueueStatus
                    };
                    storeQueueStatus.Add(qt);
                    cmbQueueStatus.SelectedItem.Value = _data.QueueStatusID.ToString();
                    cmbQueueStatus.UpdateSelectedItems();
                }

                string command = Request.QueryString["command"];
                if (command == "changestatus")
                {
                    dtRegisDate.ReadOnly = true;
                    txtPoNo.ReadOnly = true;
                    txtRemark.ReadOnly = true;
                    txtTruckRegno.ReadOnly = true;
                    cmbSelectDock.ReadOnly = true;
                    cmbTruckType.ReadOnly = true;
                    cmdRegisterType.ReadOnly = true;
                   // cmbProvince.ReadOnly = true;
                    cmdShipFrom.ReadOnly = true;
                    cmbShipTo.ReadOnly = true;
                    btnClear.Visible = false;
                    cmbQueueStatus.AllowBlank = false;
                }
                else
                {
                    cmbQueueStatus.AllowBlank = true;
                    flQueueStatus.Visible = false;
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