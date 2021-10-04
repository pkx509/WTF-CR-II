using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DITS.HILI.WMS.Web.apps.tools
{
    public partial class frmInspectionReClassifiedList : BaseUIPage
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
            Type type = typeof(ReclassifiedStatus);

            IEnumerable<ReclassifiedStatus> listStatus = Enum.GetValues(typeof(ReclassifiedStatus)).Cast<ReclassifiedStatus>();

            Ext.Net.ListItem ll = new Ext.Net.ListItem
            {
                Text = "All",
                Value = ""
            };
            cmbStatus.Items.Add(ll);

            foreach (ReclassifiedStatus status in listStatus)
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
            string strTitle = GetResource("ADDEDIT") + " " + GetResource("RECLASSIFIED");
            WindowShow.ShowNewPage(this, strTitle, "InspectionReClassified", "frmInspectionReClassified.aspx?oDataKeyId=" + id, Icon.Add);
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
                List<Reclassified> data = new List<Reclassified>();
                object output = new { data, total };


                if (dtStartDate.SelectedDate == DateTime.MinValue || dtEndDate.SelectedDate == DateTime.MinValue)
                {
                    return new { data, total };

                }

                StoreRequestParameters prms = new StoreRequestParameters(extraParams);

                StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetInspectionReclassified(dtStartDate.SelectedDate, dtEndDate.SelectedDate, cmbStatus.SelectedItem.Value, txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;

                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<Reclassified>>();
                }

                return new { data, total };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [DirectMethod(Timeout=180000)]
        public Changestatus UpdateSuggestLocation(Guid damageID, decimal changeStatusQty, decimal inspecQty)
        {

            if (inspecQty > changeStatusQty)
            {
                grdDataList.GetStore().GetById(damageID).Reject();
                MessageBoxExt.ShowError(GetMessage("MSG00006"));
                return null;
            }

            //var apiResp = InventoryToolsClient.SaveInspectionDamage(damageID, inspecQty).Result;


            //if (apiResp.IsSuccess)
            //{
            //    var data = apiResp.Get<Changestatus>();

            //    if (data == null)
            //    {
            //        MessageBoxExt.ShowError(GetMessage("MSG00006"));
            //        return new Changestatus();
            //    }

            //    this.grdDataList.GetStore().GetById(damageID).Commit();
            //    return data;
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(apiResp.ResponseMessage);
            return new Changestatus();
            //}
        }

        [DirectMethod(Timeout=180000)]
        public void Edit(string id, string field, string oldValue, string newValue, object gridJson)
        {

            //DataServiceModel dataService = new DataServiceModel();
            //PutAwayRuleModel _search = JSON.Deserialize<PutAwayRuleModel>(gridJson.ToString());

            //_search.search = field;

            //dataService.Add<PutAwayRuleModel>("PutAwayRuleModel", _search);

            //Results res = WebServiceHelper.Post<Results>("UpdatePutAwayRule", dataService.GetObject());

            //if (res.result)
            //{
            //    NotificationExt.Show("Update", "Update Complete");
            //    this.grdDataList.GetStore().GetById(id).Commit();
            //    BindData();
            //}
            //else
            //{
            //    MessageBoxExt.ShowError(res.message);
            //}

        }

        #region [ Add Data ]






        private void Clear_Data()
        {



        }

        private bool Validate_Save()
        {


            return true;
        }

        #endregion [ Add Data ]


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
            else
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
    }
}