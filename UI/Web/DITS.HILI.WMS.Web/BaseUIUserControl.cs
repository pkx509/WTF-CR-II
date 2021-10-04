using System;
using System.Globalization;
using System.Threading;
using System.Web;

namespace DITS.HILI.WMS.Web
{
    public class BaseUIUserControl : System.Web.UI.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            //if (ClientService.Common.User == null)
            //{
            //    Response.Redirect("~/expire.html");
            //    return;
            //}

            //if (ClientService.Common.AccessToken == null)
            //{
            //    Response.Redirect("~/expire.html");
            //    return;
            //}

            base.OnLoad(e);
        }

        public void Access(Guid pageId)
        {
            //var ok = ClientService.Common.User.UserPermissionCollection.ToList().Exists(x => x.ProgramID == pageId);
            //if (!ok)
            //    Response.Redirect("~/accessdenie.html");
        }

        public string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }

        public string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            // *** Is it already an absolute Url?
            if (serverUrl.IndexOf("://") > -1)
            {
                return serverUrl;
            }

            // *** Start by fixing up the Url an Application relative Url
            string newUrl = ResolveUrl(serverUrl);

            Uri originalUri = HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                     "://" + originalUri.Authority + newUrl;

            return newUrl;
        }

        protected override void OnInit(EventArgs e)
        {

            string lang = Convert.ToString("en");
            string culture = string.Empty;

            if (lang.ToLower().CompareTo("en") == 0 || string.IsNullOrEmpty(culture))
            {
                culture = "en-US";
            }
            if (lang.ToLower().CompareTo("th") == 0)
            {
                culture = "th-TH";
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            base.OnInit(e);
        }
    }
}