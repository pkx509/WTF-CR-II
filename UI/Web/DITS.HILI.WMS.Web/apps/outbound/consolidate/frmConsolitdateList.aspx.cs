using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.RegisterTruckModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DITS.HILI.WMS.Web.apps.outbound.consolidate
{
    public partial class frmConsolitdateList : BaseUIPage
    {

        #region Initail
        public static string ProgramCode = "P-0016";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["IsPopup"] != null)
                {
                    grdDataList.Margin = 0;
                    hidIsPopup.Text = "true";
                    colDel.Visible = false;
                    colEdit.Visible = false;
                }
                LoadCombo();

            }
        }

        private void LoadCombo()
        {
            Type type = typeof(ConsolidateStatusEnum);

            List<ConsolidateStatusEnum> listEnum = new List<ConsolidateStatusEnum>
            {
                ConsolidateStatusEnum.LoadingOut,
                ConsolidateStatusEnum.WaitingConfirm,
                ConsolidateStatusEnum.Complete,
                ConsolidateStatusEnum.Cancel
            };

            IEnumerable<ConsolidateStatusEnum> listStatus = Enum.GetValues(typeof(ConsolidateStatusEnum)).Cast<ConsolidateStatusEnum>();
            listStatus = listStatus.Where(x => listEnum.Contains(x));

            foreach (ConsolidateStatusEnum status in listStatus)
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


            cmbStatus.InsertItem(0, "ทั้งหมด", null);

            cmbStatus.SelectedItem.Index = 2;
            cmbStatus.UpdateSelectedItems();
        }

        private void GetAddEditForm(string oDataKeyId, string oDataStatus, string oDataDocumentNo)
        {
            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(oDataKeyId);
            string base64 = Convert.ToBase64String(bytes);

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("CONSOLIDATE");
            WindowShow.ShowNewPage(this, strTitle, "ViewConsolidate", "frmConsolitdate.aspx?oDataKeyId=" + base64 + "&oDataStatus=" + oDataStatus + "&oDataDocumentNo=" + oDataDocumentNo, iconWindows);
        }


        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            int? _status = null;
            if (cmbStatus.SelectedItem.Value != null)
            {
                _status = (int)(ConsolidateStatusEnum)Enum.Parse(typeof(ConsolidateStatusEnum), cmbStatus.SelectedItem.Value.ToString());//int.Parse(this.cmbStatus.SelectedItem.Value);
            }

            DateTime? _datefrom;
            if (dtStartDate.Text == DateTime.MinValue.ToString())
            {
                _datefrom = null;
            }
            else
            {
                _datefrom = dtStartDate.SelectedDate;
            }

            DateTime? _dateto;
            if (dtEndDate.Text == DateTime.MinValue.ToString())
            {
                _dateto = null;
            }
            else
            {
                _dateto = dtEndDate.SelectedDate;
            }

            //var _status = (DispatchStatusEnum)Enum.Parse(typeof(DispatchStatusEnum), this.cmbStatus.SelectedItem.Value.ToString());
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            ApiResponseMessage apiResp = ConsolidateClient.GetConsolidateAll(txtPono.Text, txtDocNo.Text, _status, _datefrom, _dateto, txtLicensePlate.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<RegisterTruckConsolidateListModel> data = new List<RegisterTruckConsolidateListModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<RegisterTruckConsolidateListModel>>();
            }
            return new { data, total };
        }

        #endregion

        #region Event Handle
        protected void gvdDataListCenter_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oDataStatus = e.ExtraParams["oDataStatus"];
            string oDataDocumentNo = e.ExtraParams["oDataDocumentNo"];
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
                GetAddEditForm(oDataKeyId, oDataStatus, oDataDocumentNo);
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



        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {

            // GetAddEditForm("new", "");
            PagingToolbar1.MoveFirst();
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oDataStatus = e.ExtraParams["oDataStatus"];
            string oDataDocumentNo = e.ExtraParams["oDataDocumentNo"];
            //  Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId, oDataStatus, oDataDocumentNo);
            }
            if (command.ToLower() == "confirm")
            {

            }
            //if (command.ToLower() == "delete")
            //{
            //    var ok = ClientService.Outbound.DispatchClient.Remove(Id).Result;
            //    if (ok.ResponseCode == "0")
            //    {
            //        NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
            //        this.PagingToolbar1.MoveFirst();
            //    }
            //}
        }
        #endregion

        #region Method
        [DirectMethod]
        public void SomeDirectMethod(string param)
        {
            NotificationExt.Show("Save", param);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod]
        public void SaveReload()
        {
            NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            PagingToolbar1.MoveFirst();
        }

        [DirectMethod]
        public void ApproveReload(string pono)
        {
            ApiResponseMessage datacheck = new ApiResponseMessage();

            datacheck = ClientService.Outbound.DispatchClient.CheckBackOrder(pono).Result;
            if (datacheck.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(datacheck.ResponseMessage);
            }
            else
            {

                NotificationExt.Show(GetMessage("MSG00056").MessageTitle, GetMessage("MSG00056").MessageValue);

                string _msg = "**หมายเหตุ PO นี้ ยังมีสินค้าที่ยังติด Back Order**";
                string _title = "เตือน";
                int _timemm = 5000;
                if ((bool)datacheck.Data)
                {
                    NotificationExt.Show(_title, _msg, _timemm);
                }
            }

            PagingToolbar1.MoveFirst();
        }

        [DirectMethod]
        public void Close()
        {
            PagingToolbar1.MoveFirst();
        }
        #endregion

    }

}