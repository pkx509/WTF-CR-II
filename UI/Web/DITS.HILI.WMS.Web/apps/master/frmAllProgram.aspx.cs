using DITS.HILI.WMS.ClientService.Master;
using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.master
{
    public partial class frmAllProgram : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            if (!IsPostBack)
            {
                LoadCombo();
            }
        }

        private void LoadCombo()
        {
            foreach (string type in Enum.GetNames(typeof(ProgramType)))
            {

                Ext.Net.ListItem l = new Ext.Net.ListItem
                {
                    Text = type,
                    Value = ((int)Enum.Parse(typeof(ProgramType), type)).ToString()
                };
                cmbPromgramType.Items.Add(l);
            }
            cmbPromgramType.SelectedItem.Value = ((int)ProgramType.Program).ToString();

        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = ProgramClient.GetAll((ProgramType)(Convert.ToInt32(cmbPromgramType.SelectedItem.Value)), prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<Program> data = new List<Program>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<Program>>();
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

        private void GetAddEditForm(string oDataKeyId)
        {
            string strTitle = (oDataKeyId == "new") ? GetResource("ADD_NEW") : GetResource("EDIT") + " " + GetResource("PROGRAM_INFO");
            WindowShow.Show(this, strTitle, "CreateProgram", "AddEdit/frmCreateProgram.aspx?oDataKeyId=" + oDataKeyId, Icon.Add, 650, 400);
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
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        #region "DirectMethod"
        [DirectMethod(Timeout=180000)]
        public void Reload()
        {
            MasterModel.Core.CustomMessage msg = GetMessage("MSG00001");
            NotificationExt.Show(msg.MessageTitle, msg.MessageValue);
            PagingToolbar1.MoveFirst();
        }
        #endregion
    }
}