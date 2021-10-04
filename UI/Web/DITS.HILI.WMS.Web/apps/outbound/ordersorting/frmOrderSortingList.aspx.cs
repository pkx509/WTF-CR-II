using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.outbound.ordersorting
{
    public partial class frmOrderSortingList : BaseUIPage
    {
        #region Initail
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Event Handle
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            GetAddEditForm("new", "", "");
        }

        private void GetAddEditForm(string oDataKeyId, string oDataStatusId, string oDataStatusName)
        {
            string strTitle = "Add/Edit Order Sorting";

            WindowShow.ShowNewPage(this, strTitle, "OrderSortingPage",
                                        "frmOrderSorting.aspx?oDataKeyId=" + oDataKeyId +
                                        "&oDataStatusID=" + oDataStatusId +
                                        "&oDataStatusName=" + oDataStatusName, Icon.Newspaper);
        }

        #endregion
    }
}