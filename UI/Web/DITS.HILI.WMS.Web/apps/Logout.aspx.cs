using System;

namespace DITS.HILI.WMS.Web.apps
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            ClientScript.RegisterStartupScript(GetType(), "redirect", "if(top!=self) {top.location.href = 'logon.aspx';} else {self.location.href = 'logon.aspx';}", true);
        }
    }
}