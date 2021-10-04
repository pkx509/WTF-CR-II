using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Web;

namespace DITS.HILI.WMS.ClientService
{

    public class Common
    {
        public static Token AccessToken
        {
            get => (Token)HttpContext.Current.Session["Token"];
            set => HttpContext.Current.Session.Add("Token", value);
        }

        public static UserAccounts User
        {
            set => HttpContext.Current.Session.Add("User", value);
            get => (UserAccounts)HttpContext.Current.Session["User"];
        }

        public static string Language
        {
            set => HttpContext.Current.Session.Add("Language", value);
            get => HttpContext.Current.Session["Language"] == null ? "en" : Convert.ToString(HttpContext.Current.Session["Language"]);
        }

    }
}
