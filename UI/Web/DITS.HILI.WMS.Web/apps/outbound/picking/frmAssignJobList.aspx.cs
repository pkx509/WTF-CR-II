using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.PickingModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.picking
{
    public partial class frmAssignJobList : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            LoadCombo();
            dtStartDate.SelectedDate = DateTime.Now;
            dtEndDate.SelectedDate = DateTime.Now;
        }

        private void LoadCombo()
        {
            Type type = typeof(PickingStatusEnum);

            List<PickingStatusEnum> listEnum = new List<PickingStatusEnum>
            {
                PickingStatusEnum.All,
                PickingStatusEnum.Pick,
                PickingStatusEnum.WaitingPick,
                PickingStatusEnum.LoadingOut,
                PickingStatusEnum.Complete,
                PickingStatusEnum.Cancel
            };

            IEnumerable<PickingStatusEnum> listStatus = Enum.GetValues(typeof(PickingStatusEnum)).Cast<PickingStatusEnum>();
            listStatus = listStatus.Where(x => listEnum.Contains(x));

            foreach (PickingStatusEnum status in listStatus)
            {
                System.Reflection.MemberInfo[] memInfo = type.GetMember(status.ToString());
                object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = ((DescriptionAttribute)attributes[0]).Description;

                Ext.Net.ListItem l = new Ext.Net.ListItem
                {
                    Text = description,
                    Value = status.ToString()
                };
                cmbPickingStatus.Items.Add(l);
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            PickingStatusEnum pickingStatus;

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            if (cmbPickingStatus.SelectedItem.Value == null)
            {
                cmbPickingStatus.SelectedItem.Value = PickingStatusEnum.All.ToString();
            }

            pickingStatus = (PickingStatusEnum)Enum.Parse(typeof(PickingStatusEnum), cmbPickingStatus.SelectedItem.Value);

            Core.Domain.ApiResponseMessage apiResp = PickingClient.GetAllAssignJob(pickingStatus
                                , dtStartDate.SelectedDate
                                , dtEndDate.SelectedDate
                                , txtDocNo.Text
                                , txtPONO.Text
                                , prms.Page
                                , int.Parse(cmbPageList.SelectedItem.Value)).Result;

            List<AssignJobModel> data = new List<AssignJobModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<AssignJobModel>>();
            }
            else
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }

            return new { data, total };
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("");
        }

        private void GetAddEditForm(string pickingID)
        {
            string strTitle = GetResource("ADDEDIT") + " " + GetResource("ASSIGNJOB");

            if (string.IsNullOrWhiteSpace(pickingID))
            {
                WindowShow.ShowNewPage(this, strTitle, "AddEditAssignJob", "frmAssignJob.aspx", Icon.Add);
            }
            else
            {
                WindowShow.ShowNewPage(this, strTitle, "AddEditAssignJob", "frmAssignJob.aspx?pickingID=" + pickingID, Icon.ApplicationEdit);
            }
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string pickingID = e.ExtraParams["pickingID"];

            if (!Guid.TryParse(pickingID, out Guid pickingGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }

            if (command == "Edit")
            {
                GetAddEditForm(pickingID);
            }
            else
            {
                Core.Domain.ApiResponseMessage apiResp = PickingClient.Remove(new Guid(pickingID)).Result;

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

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string pickingID = e.ExtraParams["pickingID"];

            if (!Guid.TryParse(pickingID, out Guid pickingGUID))
            {
                MessageBoxExt.ShowError(GetMessage("MSG00005"));
                return;
            }
            GetAddEditForm(pickingID);
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