using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Contacts;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllContacts : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            //if (IsPostBack)
            //    GetData();
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            ContactType _contactType = new ContactType();

            Core.Domain.ApiResponseMessage apiResp = ContactClient.Get(_contactType, txtSearch.Text, ckbIsActive.Checked, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<Contact> data = new List<Contact>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<Contact>>();
            }

            return new { data, total };
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new");
        }
        protected void btnSearch_Event(object sender, DirectEventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        private void GetAddEditForm(string oDataKeyId)
        {

            Icon iconWindows = Icon.ApplicationFormEdit;
            if (oDataKeyId == "new")
            {
                iconWindows = Icon.ApplicationFormAdd;
            }

            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("CONTACTS");
            WindowShow.Show(this, strTitle, "CreateContacts", "AddEdit/frmCreateContacts.aspx?oDataKeyId=" + oDataKeyId, iconWindows, 650, 520);
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "edit")
            {
                GetAddEditForm(oDataKeyId);
            }
            if (command.ToLower() == "delete")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.Master.ContactClient.Remove(Id).Result;

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

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        #region "DirectMethod"
        [DirectMethod(Timeout=180000)]
        public void Reload(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
            }
            PagingToolbar1.MoveFirst();
        }
        #endregion
    }
}