using DITS.HILI.WMS.ClientService.Inbound;
using DITS.HILI.WMS.ReceiveModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.inbound.receive_WTF
{
    public partial class frmInternalRecList_WTF : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            dtReceiveDate.SelectedDate = DateTime.Now;
            LoadCombo();
        }

        private void LoadCombo()
        {
            Type type = typeof(ReceiveStatusEnum);

            List<ReceiveStatusEnum> listEnum = new List<ReceiveStatusEnum>
            {
                ReceiveStatusEnum.New,
                ReceiveStatusEnum.Check,
                ReceiveStatusEnum.GenDispatch,
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
                cmbReceiveStatus.Items.Add(l);
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            Guid receiveTypeId = Guid.Empty;
            ReceiveStatusEnum receiveStatusEnum = new ReceiveStatusEnum();

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            if (cmbReceiveStatus.SelectedItem != null && cmbReceiveStatus.SelectedItem.Value != null)
            {
                receiveStatusEnum = (ReceiveStatusEnum)Enum.Parse(typeof(ReceiveStatusEnum), cmbReceiveStatus.SelectedItem.Value);
            }

            Guid.TryParse(cmbReceiveType.SelectedItem.Value, out receiveTypeId);

            Core.Domain.ApiResponseMessage apiResp = ReceiveClient.GetAllInternalReceive(dtReceiveDate.SelectedDate
                                                                , receiveStatusEnum
                                                                , receiveTypeId
                                                                , txtReceiveNo.Text
                                                                , txtOrderNo.Text
                                                                , txtPONo.Text
                                                                , prms.Page
                                                                , int.Parse(cmbPageList.SelectedItem.Value)).Result;

            List<ReceiveHeaderModel> data = new List<ReceiveHeaderModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<ReceiveHeaderModel>>();
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

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("");
        }

        private void GetAddEditForm(string receiveID)
        {
            //Guid receiveGUID;
            string strTitle = GetResource("ADDEDIT") + " " + GetResource("INTERNALRECEIVE");
            WindowShow.ShowNewPage(this, strTitle, "InternalReceive", "frmInternalRec_WTF.aspx?receiveID=" + receiveID, Icon.Add);
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string receiveID = e.ExtraParams["receiveID"];

            GetAddEditForm(receiveID);
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
                GetAddEditForm(receiveID);
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
    }
}