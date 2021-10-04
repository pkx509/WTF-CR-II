using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.PickingModel;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace DITS.HILI.WMS.Web.apps.outbound.picking._usercontrol
{
    public partial class ucDispatchforAssignJob : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(List<DispatchforAssignJobModel> listData)
        {
            winDispatchSelect.Show();
            HeaderFilterDispatch.Text = listData.FirstOrDefault()?.SearchPO ?? " ";
            HeaderFilterDispatch.Focus(false, 100);
            PagingToolbar1.MoveFirst();

            //if (listData != null && listData.Count() > 0)
            //{
            //    StoreDispatchSelect.DataSource = listData;
            //}

        }

        public object DispatchSelectBindData(string action, Dictionary<string, object> extraParams)
        {
            int total = 0;
            List<DispatchforAssignJobModel> data = new List<DispatchforAssignJobModel>();

            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            StoreDispatchSelect.PageSize = int.Parse(cmbPageList.SelectedItem.Value);

            Core.Domain.ApiResponseMessage apiResp = PickingClient.GetDispatchforAssignJob(HeaderFilterDispatch.Text, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;

            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<DispatchforAssignJobModel>>();
            }

            return new { data, total };
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["DataKey"];
            X.AddScript("App.direct.ucDispatch_Select(" + _recordSelect + ");");
        }

        public void Close()
        {
            winDispatchSelect.Close();
        }

    }
}