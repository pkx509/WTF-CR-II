using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.outbound.dispatch
{
    public partial class frmDispatchList : BaseUIPage
    {

        #region Initail
        public static string ProgramCode = "P-0013";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDeliveryDate.Text = DateTime.Now.ToString("dd/MM/yyyy");


                if (Request.QueryString["IsPopup"] != null)
                {
                    grdDataList.Margin = 0;
                    hidIsPopup.Text = "true";
                    colDel.Visible = false;
                    colEdit.Visible = false;
                    colConfirm.Visible = false;
                    btnAdd.Visible = false;
                }
                LoadCombo();

            }
        }

        private void LoadCombo()
        {
            Type type = typeof(DispatchStatusEnum);

            List<DispatchStatusEnum> listEnum = new List<DispatchStatusEnum>
            {
                DispatchStatusEnum.New,
                DispatchStatusEnum.Inprogress,
                DispatchStatusEnum.InprogressConfirm,
                DispatchStatusEnum.InprogressConfirmQA,
                DispatchStatusEnum.InBackOrder,
                DispatchStatusEnum.Register,
                DispatchStatusEnum.WaitingConfirmDispatch,
                DispatchStatusEnum.InternalReceive,
                DispatchStatusEnum.WaitingConfirmDispatchNoneRegister,
                DispatchStatusEnum.WaitingConfirmDispatchQA,
                DispatchStatusEnum.Complete,
                DispatchStatusEnum.Close
            };

            IEnumerable<DispatchStatusEnum> listStatus = Enum.GetValues(typeof(DispatchStatusEnum)).Cast<DispatchStatusEnum>();
            listStatus = listStatus.Where(x => listEnum.Contains(x));

            foreach (DispatchStatusEnum status in listStatus)
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

            cmbStatus.InsertItem(0, "ทั้งหมด", 0);
        }

        private void GetAddEditForm(string oDataKeyId, string oDataStatus)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("DISPATCH");
            WindowShow.ShowNewPage(this, strTitle, "CreateDispatach", "frmDispatch.aspx?oDataKeyId=" + oDataKeyId + "&oDataStatus=" + oDataStatus, iconWindows);
        }

        private void GetConfirmForm(string oDataKeyId)
        {
            WindowsPopup.showOperation(this, "ยินยันการจ่ายสินค้า", ProgramCode + "-FORM",
                                "frmDispatchConfirm.aspx?oDataKeyId=" + oDataKeyId, Icon.CheckError);
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int? _status = 0;
            if (cmbStatus.SelectedItem.Value != null)
            {
                try
                {
                    _status = (int)(DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), cmbStatus.SelectedItem.Value.ToString());//int.Parse(this.cmbStatus.SelectedItem.Value);
                }
                catch
                {
                    _status = (int)(DispatchStatusEnum.New);
                }
            }

            DateTime? _datedelivery;
            if (dtDeliveryDate.Text == DateTime.MinValue.ToString())
            {
                _datedelivery = null;
            }
            else if (dtDeliveryDate.SelectedDate == null)
            {
                _datedelivery = DateTime.Now.Date;
            }
            else
            {
                _datedelivery = dtDeliveryDate.SelectedDate;
            }
            Guid? _documenttypeid = null;
            if (cmbDispatchType.SelectedItem.Value != null && cmbDispatchType.SelectedItem.Value != Guid.Empty.ToString())
            {
                _documenttypeid = new Guid(cmbDispatchType.SelectedItem.Value);
            }
            //var _status = (DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), this.cmbStatus.SelectedItem.Value.ToString());
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = DispatchClient.Get(_documenttypeid, _datedelivery, _status, txtPono.Text, txtOrderNo.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<DispatchModels> data = new List<DispatchModels>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<DispatchModels>>();
            }
            return new { data, total };
        }

        #endregion

        #region Event Handle
        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oDataStatus = e.ExtraParams["oDataStatus"];
            if (string.IsNullOrEmpty(oDataKeyId))
            {
                return;
            }

            if (hidIsPopup.Text != "")
            {
                X.Call("parent.App.direct.SomeDirectMethod", oDataKeyId);
                X.Call("parentAutoLoadControl.close()");
            }
            else
            {
                GetAddEditForm(oDataKeyId, oDataStatus);
            }
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

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            GetAddEditForm("new", "");
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oDataStatus = e.ExtraParams["oDataStatus"];

            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId, oDataStatus);
            }
            if (command.ToLower() == "confirm")
            {

            }
            if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.Outbound.DispatchClient.Remove(Id).Result;
                if (ok.ResponseCode == "0")
                {
                    //NotificationExt.Show("Delete", "Delete complete");
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                    PagingToolbar1.MoveFirst();
                }
            }
        }
        #endregion

        #region Method
        [DirectMethod(Timeout=180000)]
        public void SomeDirectMethod(string param)
        {
            NotificationExt.Show("Save", param);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void SaveReload()
        {
            NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void BookingReload()
        {
            NotificationExt.Show(GetMessage("MSG00034").MessageTitle, GetMessage("MSG00034").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void ApproveReload()
        {
            NotificationExt.Show(GetMessage("MSG00034").MessageTitle, GetMessage("MSG00034").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void ApproveDispatchReload()
        {
            NotificationExt.Show(GetMessage("MSG00092").MessageTitle, GetMessage("MSG00092").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void CancelDispatchReload()
        {
            NotificationExt.Show(GetMessage("MSG00035").MessageTitle, GetMessage("MSG00035").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void CancelReload()
        {
            NotificationExt.Show(GetMessage("MSG00035").MessageTitle, GetMessage("MSG00035").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod(Timeout=180000)]
        public void Close()
        {
            PagingToolbar1.MoveFirst();
        }

        #endregion

    }

}