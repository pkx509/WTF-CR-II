using DITS.HILI.WMS.ClientService.Inbound;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.inbound.receive_WTF
{
    public partial class frmReceiveList_WTF : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            dtReceiveDateEST.SelectedDate = DateTime.Now;
            LoadCombo();
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string receiveID = e.ExtraParams["receiveID"];

            if (!Guid.TryParse(receiveID, out Guid receiveGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }
            GetAddEditForm(receiveGUID);
        }

        private void GetAddEditForm(Guid receiveGUID)
        {
            string strTitle = GetResource("EDIT") + " " + GetResource("RECEIVE");
            WindowShow.ShowNewPage(this, strTitle, "AddEditReceive", "frmReceive_WTF.aspx?receiveID=" + receiveGUID.ToString(), Icon.Add);
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string receiveID = e.ExtraParams["receiveID"];

            if (!Guid.TryParse(receiveID, out Guid receiveGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            if (command == "Edit")
            {
                GetAddEditForm(receiveGUID);
            }
            else
            {
                Core.Domain.ApiResponseMessage apiResp = ReceiveClient.Cancel(receiveGUID).Result;

                if (apiResp.IsSuccess)
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }
        }

        private void LoadCombo()
        {
            Type type = typeof(ReceiveStatusEnum);

            List<ReceiveStatusEnum> listEnum = new List<ReceiveStatusEnum>
            {
                ReceiveStatusEnum.New,
                ReceiveStatusEnum.LoadIn,
                ReceiveStatusEnum.Partial,
                ReceiveStatusEnum.Complete,
                ReceiveStatusEnum.Cancel
            };

            IEnumerable<ReceiveStatusEnum> listStatus = Enum.GetValues(typeof(ReceiveStatusEnum)).Cast<ReceiveStatusEnum>();
            listStatus = listStatus.Where(x => listEnum.Contains(x));

            foreach (ReceiveStatusEnum status in listStatus)
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
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            Guid lineID = Guid.Empty;
            ReceiveStatusEnum receiveStatusEnum = new ReceiveStatusEnum();

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            if (cmbStatus.SelectedItem != null && cmbStatus.SelectedItem.Value != null)
            {
                receiveStatusEnum = (ReceiveStatusEnum)Enum.Parse(typeof(ReceiveStatusEnum), cmbStatus.SelectedItem.Value);
            }

            if (cmbLine.SelectedItem != null && cmbLine.SelectedItem.Value != null)
            {
                Guid.TryParse(cmbLine.SelectedItem.Value, out lineID);
            }

            Core.Domain.ApiResponseMessage apiResp = ReceiveClient.GetAll(dtReceiveDateEST.SelectedDate
                                            , receiveStatusEnum
                                            , lineID
                                            , txtSearch.Text, prms.Page
                                            , int.Parse(cmbPageList.SelectedItem.Value)).Result;

            List<ReceiveListModel> data = new List<ReceiveListModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ReceiveListModel>>();
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }

            return new { data, total };
        }

        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, param);
            }
            else
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
    }
}