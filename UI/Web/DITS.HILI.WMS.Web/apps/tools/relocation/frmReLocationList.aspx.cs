using Ext.Net;
using System;

namespace DITS.HILI.WMS.Web.apps.tools.relocation
{
    public partial class frmReLocationList : BaseUIPage
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

            string strTitle = "Add/Edit Re-Location";

            WindowShow.ShowNewPage(this, strTitle, "ReLocationPage",
                                        "frmReLocation.aspx?oDataKeyId=" + oDataKeyId +
                                        "&oDataStatusID=" + oDataStatusId +
                                        "&oDataStatusName=" + oDataStatusName, Icon.Newspaper);
        }


        #endregion
    }
}