using DITS.HILI.WMS.PutAwayModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools.qa
{
    public partial class frmQAList : BaseUIPage
    {
        #region Initail
        string AutoCompleteService = "~/Common/DataClients/OptDataHandler.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            getPutAwayStatus();
            AutoCompleteProxy();

        }

        private async void getPutAwayStatus()
        {
            try
            {
                cmbStatus.Items.Clear();
                cmbStatus.Items.Insert(0, new Ext.Net.ListItem
                {
                    Text = "All",
                    Value = ""
                });
                var status = await ClientService.Inbound.PutAwayClient.GetPutawayStatus();
                status.ForEach(Item =>
                {
                    cmbStatus.Items.Add(new Ext.Net.ListItem
                    {
                        Text = Item.Value,
                        Value = Item.ID.ToString()
                    });

                });

                this.cmbStatus.SelectedItem.Index = 1;
                this.cmbStatus.UpdateSelectedItems();

            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError("Load Putaway Status Fail : " + ex.Message.ToString());

            }

        }



        private void AutoCompleteProxy()
        {
            PutAwayStatusEnum? status = null;
            if (this.cmbStatus.SelectedItem.Value == null)
            {
                status = PutAwayStatusEnum.Draft;
            }
            else if (this.cmbStatus.SelectedItem.Text != "All")
            {
                status = (PutAwayStatusEnum?)Enum.Parse(typeof(PutAwayStatusEnum), this.cmbStatus.SelectedItem.Value.ToString());
            }

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Method", "putawaylist");
            param.Add("status", status);
            param.Add("query", this.txtSearch.Text.Trim());
            this.StoreOfDataList.AutoCompleteProxy(AutoCompleteService, param);
            this.StoreOfDataList.LoadProxy();
        }
        #endregion

        #region Event Handle
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.GetAddForm();
        }

        private void GetEditForm(string oDataPutAwayJobCode, PutAwayStatusEnum oDataPutawayStatus)
        {

            string strTitle = "Edit Putaway";


            WindowShow.ShowNewPage(this, strTitle, "PutawayOrderPage",
                                        "frmQA.aspx?oDataPutAwayJobCode=" + oDataPutAwayJobCode +
                                        "&oDataPutawayStatus=" + oDataPutawayStatus, Icon.Newspaper);
        }
        private void GetAddForm()
        {

            string strTitle = "Add Putaway";


            WindowShow.ShowNewPage(this, strTitle, "PutawayOrderPage",
                                        "frmQA.aspx", Icon.Newspaper);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AutoCompleteProxy();
        }

        protected async void CommandClick(object sender, DirectEventArgs e)
        {
            try
            {
                string command = e.ExtraParams["command"];
                string oDataPutAwayJobCode = e.ExtraParams["oDataPutAwayJobCode"].ToString();
                string oDataPutawayStatus = e.ExtraParams["oDataPutawayStatus"].ToString();

                if (command.ToLower() == "cancel")
                {
                    var delete = await ClientService.Inbound.PutAwayClient.Cancel(oDataPutAwayJobCode);
                    NotificationExt.Show("Cancel", "Cancel Complete");
                    AutoCompleteProxy();
                }
                else if (command.ToLower() == "edit")
                {
                    PutAwayStatusEnum rec = (PutAwayStatusEnum)Enum.Parse(typeof(PutAwayStatusEnum), oDataPutawayStatus);
                    GetEditForm(oDataPutAwayJobCode, rec);
                }

            }
            catch (Exception ex)
            {
                //  throw ex;
                MessageBoxExt.ShowError("Action Fail : " + ex.Message.ToString());
            }


        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string oDataPutawayID = e.ExtraParams["oDataPutawayID"].ToString();
            string oDataPutawayStatus = e.ExtraParams["oDataPutawayStatus"].ToString();
            PutAwayStatusEnum rec = (PutAwayStatusEnum)Enum.Parse(typeof(PutAwayStatusEnum), oDataPutawayStatus);
            GetEditForm(oDataPutawayID, rec);

        }
        #endregion

        #region "DirectMethod"
        [DirectMethod]
        public void Reload()
        {
            AutoCompleteProxy();
        }
        #endregion
    }
}