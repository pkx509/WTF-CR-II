using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using Ext.Net;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Web.apps.outbound.backorder
{
    public partial class frmBackOrderList : BaseUIPage
    {

        public static string ProgramCode = "P-0015";

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }

        [DirectMethod(Timeout=180000)]
        public object BindData(string action, Dictionary<string, object> extraParams)
        {
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);

            int total = 0;
            StoreOfDataList.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = DispatchClient.GetViewBackOrder(txtSearch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<BackOrderModel> data = new List<BackOrderModel>();
            if (apiResp.ResponseCode == "0")
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<BackOrderModel>>();
            }

            return new { data, total };
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void Store_Refresh(object sender, EventArgs e)
        {
            PagingToolbar1.MoveFirst();
        }

        protected void CommandClick(object sender, DirectEventArgs e)
        {
            string command = e.ExtraParams["command"];
            string oDataKeyId = e.ExtraParams["oDataKeyId"];
            string oDataIsConfirm = e.ExtraParams["oDataIsConfirm"];
            string oDataPono = e.ExtraParams["oDataPono"];
            string oDataKeyBookingId = e.ExtraParams["oDataKeyBookingId"];

            if (command.ToLower() == "confirm")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.Outbound.DispatchClient.OnBookingBackOrder(oDataKeyId, oDataPono, new Guid(oDataKeyBookingId)).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00034").MessageTitle, GetMessage("MSG00034").MessageValue);
                }
                else
                {
                    MessageBoxExt.ShowError(ok.ResponseMessage);
                }
            }
            else if (command.ToLower() == "revise")
            {
                Core.Domain.ApiResponseMessage ok = ClientService.Outbound.DispatchClient.RemoveBooking(new Guid(oDataKeyBookingId)).Result;
                if (ok.ResponseCode == "0")
                {
                    NotificationExt.Show(GetMessage("MSG00002").MessageTitle, GetMessage("MSG00002").MessageValue);
                }
                else
                {
                    MessageBoxExt.ShowError(ok.ResponseMessage);
                }
            }

            PagingToolbar1.MoveFirst();
        }
    }
}