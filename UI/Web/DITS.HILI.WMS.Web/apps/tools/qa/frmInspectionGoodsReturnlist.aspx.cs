using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.tools
{
    public partial class frmInspectionGoodsReturnlist : BaseUIPage
    {

        // string AutoCompleteService = "../../Common/DataClients/DataOfMaster.ashx";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;
            if (!IsPostBack)
            {
                dtStartDate.SelectedDate = DateTime.Now;
                dtEndDate.SelectedDate = DateTime.Now;
                LoadCombo();
            }

        }

        private void LoadCombo()
        {
            Type type = typeof(GoodsReturnStatusEnum);

            IEnumerable<GoodsReturnStatusEnum> listStatus = Enum.GetValues(typeof(GoodsReturnStatusEnum)).Cast<GoodsReturnStatusEnum>();

            Ext.Net.ListItem ll = new Ext.Net.ListItem
            {
                Text = "All",
                Value = ""
            };
            cmbStatus.Items.Add(ll);

            foreach (GoodsReturnStatusEnum status in listStatus)
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
            if (dtEndDate.SelectedDate == DateTime.MinValue)
            {
                MessageBoxExt.ShowError("Please Select End Date", MessageBox.Button.OK,
                    Icon.Error, MessageBox.Icon.WARNING, "Ext.getCmp('dtEndDate').focus('', 10); ");
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
            string strTitle = GetResource("ADDEDIT") + " " + GetResource("GOODSRETURN");
            WindowShow.ShowNewPage(this, strTitle, "InspectionGoodsReturn", "frmInspectionGoodsReturn.aspx?oDataKeyId=" + id, Icon.Add);
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];

            if (command == "Edit")
            {
                GetAddEditForm(oDataKeyId);
            }
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            try
            {
                Guid lineGUID = Guid.Empty;
                int total = 0;
                List<GoodsReturn> data = new List<GoodsReturn>();
                object output = new { data, total };


                if (dtStartDate.SelectedDate == DateTime.MinValue || dtEndDate.SelectedDate == DateTime.MinValue)
                {
                    return new { data, total };

                }

                StoreRequestParameters prms = new StoreRequestParameters(extraParams);

                StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetInspectionGoodsReturn(dtStartDate.SelectedDate, dtEndDate.SelectedDate, cmbStatus.SelectedItem.Value, txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;

                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<GoodsReturn>>();
                }



                return new { data, total };

            }
            catch (Exception ex)
            {

                throw ex;
            }
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