using DITS.HILI.WMS.ClientService.Outbound;
using DITS.HILI.WMS.RegisterTruckModel;
//using DITS.WMS.Web.Common.Interfaces;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace DITS.HILI.WMS.Web.apps.outbound.registruck._usercontrol
{
    public partial class ucDispatchSelect : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(string PONo)
        {
            winDispatchSelect.Show();
            HeaderFilterDispatch.Text = PONo;
            HeaderFilterDispatch.Focus(false, 100);
            PagingToolbar1.MoveFirst();
        }

        public void Close()
        {
            winDispatchSelect.Close();
        }

        public object DispatchSelectBindData(string action, Dictionary<string, object> extraParams, string method)
        {

            RegisTruckModel searchModel = new RegisTruckModel();
            StoreRequestParameters prms = new StoreRequestParameters(extraParams);
            if (!string.IsNullOrWhiteSpace(HeaderFilterDispatch.Text))
            {
                searchModel.PoNo = HeaderFilterDispatch.Text;
            }

            if (method == "Get_DispatchForAssignJob")
            {
                //searchModel.PoNo = StatusCode.PENDING_A.GetStatus();
            }

            int total = 0;
            StoreDispatchSelect.PageSize = int.Parse(cmbPageList.SelectedItem.Value);
            Core.Domain.ApiResponseMessage apiResp = RegisterTruckClient.getdispatchForRegisTrucklistAll(null, searchModel.PoNo, prms.Page, int.Parse(cmbPageList.SelectedItem.Value)).Result;
            List<DispatchAllModel> data = new List<DispatchAllModel>();
            if (apiResp.IsSuccess)
            {
                total = apiResp.Totals;
                data = apiResp.Get<List<DispatchAllModel>>();
            }

            return new { data, total };

            //if (method == "Get_DispatchForAssignJob")
            //{
            //    searchModel.PoNo = StatusCode.PENDING_A.GetStatus();
            //}
        }

        protected void grdDataList_CellDblClick(object sender, DirectEventArgs e)
        {
            string _recordSelect = e.ExtraParams["DataKey"];
            X.AddScript("App.direct.ucDispatch_Select(" + _recordSelect + ");");

        }
    }
}