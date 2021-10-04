using DITS.HILI.WMS.ClientService.Tools;
using DITS.HILI.WMS.InventoryToolsModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web.apps.tools
{
    public partial class frmInspectionDamage : BaseUIPage
    {

        // string AutoCompleteService = "../../Common/DataClients/DataOfMaster.ashx";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;
            if (!IsPostBack)
            {

                LoadCombo();
                dtStartDate.SelectedDate = DateTime.Now;
                dtEndDate.SelectedDate = DateTime.Now;
            }
        }

        private void LoadCombo()
        {
            Type type = typeof(InspectionStatus);

            IEnumerable<InspectionStatus> listStatus = Enum.GetValues(typeof(InspectionStatus)).Cast<InspectionStatus>();

            Ext.Net.ListItem ll = new Ext.Net.ListItem
            {
                Text = "All",
                Value = ""
            };
            cmbStatus.Items.Add(ll);

            foreach (InspectionStatus status in listStatus)
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



            PagingToolbar1.MoveFirst();
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {

            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            Guid Id = new Guid(oDataKeyId);

            if (command.ToLower() == "approve")
            {
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.ApproveInspectionDamage(Id).Result;


                if (apiResp.IsSuccess)
                {
                    NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                    grdDataList.Dispose();
                    PagingToolbar1.MoveFirst();
                }
                else
                {
                    MessageBoxExt.ShowError(apiResp.ResponseMessage);
                }
            }

        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            try
            {
                StoreOfDataList.RemoveAll();
                Guid lineGUID = Guid.Empty;
                int total = 0;
                List<Changestatus> data = new List<Changestatus>();
                object output = new { data, total };
                if (cmbLine.SelectedItem.Value != null)
                {
                    if (!Guid.TryParse(cmbLine.SelectedItem.Value, out lineGUID))
                    {
                        MessageBoxExt.ShowError(GetMessage("MSG00032"));
                        return output;
                    }
                }

                if (dtStartDate.SelectedDate == DateTime.MinValue || dtEndDate.SelectedDate == DateTime.MinValue)
                {
                    return new { data, total };

                }

                StoreRequestParameters prms = new StoreRequestParameters(extraParams);

                StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
                Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.GetInspectionDamage(dtStartDate.SelectedDate, dtEndDate.SelectedDate, lineGUID, cmbStatus.SelectedItem.Value, txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;

                if (apiResp.ResponseCode == "0")
                {
                    total = apiResp.Totals;
                    data = apiResp.Get<List<Changestatus>>();
                }
                return new { data, total };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [DirectMethod(Timeout=180000)]
        public object SaveDamage(object gridJson, decimal ReprocessQty, decimal RejectQty)
        {

            Changestatus model = JSON.Deserialize<Changestatus>(gridJson.ToString());

            if ((ReprocessQty + RejectQty) > model.DamageQty)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00079"));
                return new { valid = false };
            }
            Core.Domain.ApiResponseMessage apiResp = InventoryToolsClient.SaveInspectionDamage(model.DamageID, RejectQty, ReprocessQty).Result;

            if (apiResp.ResponseCode != "0")
            {
                MessageBoxExt.ShowError(apiResp.ResponseMessage);
            }

            StoreOfDataList.Reload();
            return new { valid = true };
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

        protected void btnDispatchRepair_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<Changestatus> gridData = JSON.Deserialize<List<Changestatus>>(gridJson);


            Core.Domain.ApiResponseMessage ok = InventoryToolsClient.SendtoReprocess(gridData.Where(x => x.IsReprocess == true).ToList()).Result;

            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                PagingToolbar1.MoveFirst();
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }

        protected void btnDispatchDamage_Click(object sender, DirectEventArgs e)
        {
            string gridJson = e.ExtraParams["ParamStorePages"];
            List<Changestatus> gridData = JSON.Deserialize<List<Changestatus>>(gridJson).Where(x => x.IsReject == true).ToList();

            if (gridData == null || gridData.Count == 0)
            {
                MessageBoxExt.ShowError(GetMessage("MSG00006").MessageValue);
                return;
            }

            Core.Domain.ApiResponseMessage ok = InventoryToolsClient.SendtoDamage(gridData).Result;
            if (ok.ResponseCode == "0")
            {
                NotificationExt.Show(GetMessage("MSG00001").MessageTitle, GetMessage("MSG00001").MessageValue);
                PagingToolbar1.MoveFirst();
            }
            else
            {
                MessageBoxExt.ShowError(ok.ResponseMessage);
            }
        }




    }
}