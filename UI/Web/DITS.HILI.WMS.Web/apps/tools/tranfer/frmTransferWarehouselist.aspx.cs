using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.tools
{
    public partial class frmTransferWarehouselist : BaseUIPage
    {

        // string AutoCompleteService = "../../Common/DataClients/DataOfMaster.ashx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtStartDate.SelectedDate = DateTime.Now;
                LoadCombo();
            }
        }

        private void LoadCombo()
        {
            Type type = typeof(TranferMargetingStatus);

            IEnumerable<TranferMargetingStatus> listStatus = Enum.GetValues(typeof(TranferMargetingStatus)).Cast<TranferMargetingStatus>();

            Ext.Net.ListItem ll = new Ext.Net.ListItem
            {
                Text = "All",
                Value = ""
            };
            cmbStatus.Items.Add(ll);

            foreach (TranferMargetingStatus status in listStatus)
            {
                System.Reflection.MemberInfo[] memInfo = type.GetMember(status.ToString());
                object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = ((DescriptionAttribute)attributes[0]).Description;

                Ext.Net.ListItem l = new Ext.Net.ListItem
                {
                    Text = description,
                    Value = status.ToString()
                };
                cmbStatus.Items.Add(l);
            }
            cmbStatus.SelectedItem.Index = 0;
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtStartDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("Please Select Start Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtStartDate').focus('', 10); ");
                return;
            }

            PagingToolbar1.MoveFirst();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }

        private void GetAddEditForm(string id)
        {
            string strTitle = GetResource("ADDEDIT") + " " + GetResource("TRANSFER");
            WindowShow.ShowNewPage(this, strTitle, "TransferMargetingWarehouse", "frmTransferWarehouse.aspx?oDataKeyId=" + id, Icon.Add);
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            try
            {
                Guid lineGUID = Guid.Empty;
                int total = 0;
                List<TRMTransferMarketing> datalist = new List<TRMTransferMarketing>();
                List<TRMTransferMarketing> data = new List<TRMTransferMarketing>();
                object output = new { data, total };


                if (dtStartDate.SelectedDate == DateTime.MinValue)
                {
                    return new { data, total };
                }

                StoreRequestParameters prms = new StoreRequestParameters(extraParams);

                StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetTransferMargetingAll(dtStartDate.SelectedDate, DateTime.Now, cmbStatus.SelectedItem.Value, txtTrmCode.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;

                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    datalist = apiResp.Get<List<TRMTransferMarketing>>();


                    datalist.ForEach(item =>
                    {
                        TRMTransferMarketing _data = new TRMTransferMarketing
                        {
                            TRM_CODE = item.TRM_CODE,
                            TRM_ID = item.TRM_ID,
                            TransferDate = item.TransferDate,
                            Description = item.Description,
                            ApproveDate = item.ApproveDate,
                            TransferStatusName = CheckStatus(item.TransferStatus.Value),
                            TransferStatus = item.TransferStatus,
                            IsApprove = item.TransferStatus.Value == (int)TranferMargetingStatus.Approve ? true : false,
                            Remark = item.Remark,
                            IsActive = item.IsActive,
                            UserCreated = item.UserCreated,
                            UserModified = item.UserModified,
                            DateCreated = item.DateCreated,
                            DateModified = item.DateModified
                        };
                        data.Add(_data);

                        //btnCheckStatus(item.TransferStatus.Value);
                    });
                }

                return new { data, total };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string CheckStatus(int status)
        {
            string statusName = string.Empty;
            switch (status)
            {
                case 10:
                    statusName = TranferMargetingStatus.New.ToString();
                    break;
                case 20:
                    statusName = TranferMargetingStatus.Assign.ToString();
                    break;
                case 30:
                    statusName = TranferMargetingStatus.Confirm.ToString();
                    break;
                case 100:
                    statusName = TranferMargetingStatus.Approve.ToString();
                    break;
                case 102:
                    statusName = TranferMargetingStatus.Cancel.ToString();
                    break;
                default:
                    break;
            }

            return statusName;
        }

        private void btnCheckStatus(int status)
        {
            switch (status)
            {
                case 10:
                    colDel.Enable();
                    colEdit.Enable();
                    break;
                case 20:
                    colDel.Disable();
                    colEdit.Disable();
                    break;
                case 30:
                    colDel.Disable();
                    colEdit.Enable();
                    break;
                case 100:
                    colDel.Disable();
                    colEdit.Enable();
                    break;
                default:
                    break;
            }
        }

        protected async void CommandClick(object sender, DirectEventArgs e)
        {
            string status = e.ExtraParams["CycleCountStatus"];
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            try
            {
                if (command == "Edit")
                {
                    GetAddEditForm(oDataKeyId);
                }
                else
                {
                    if ((status == TranferMargetingStatus.Approve.ToString()))
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
                        Core.Domain.ApiResponseMessage ok = InventoryToolsClient.RemoveTransfer(oDataKeyId).Result;
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

        protected void btnDispatch_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<Changestatus> gridDataList = JSON.Deserialize<List<Changestatus>>(gridJson);
        }

        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<Changestatus> gridDataList = JSON.Deserialize<List<Changestatus>>(gridJson);
        }

        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, param);
            }

            PagingToolbar1.MoveFirst();
        }
    }
}