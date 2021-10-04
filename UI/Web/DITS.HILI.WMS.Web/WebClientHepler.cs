using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Web;

namespace DITS.HILI.WMS.Web
{
    public class WebClientHepler
    {

        public static Token AccessToken
        {
            get => (Token)HttpContext.Current.Session["Token"];
            set => HttpContext.Current.Session["Token"] = value;
        }

        public static UserAccounts User
        {
            set => HttpContext.Current.Session["User"] = value;
            get => new UserAccounts { UserID = new Guid("917ED702-7D31-4F97-8180-B88A29CAF3A1") };//return (UserAccounts)HttpContext.Current.Session["User"];
        }
        public static string Language
        {
            set => HttpContext.Current.Session["Language"] = value;
            get => Convert.ToString(HttpContext.Current.Session["Language"]);
        }
    }
}