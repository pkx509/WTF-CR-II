using DITS.HILI.WMS.ClientService;
using DITS.HILI.WMS.MasterModel.Core;
using Ext.Net.Utilities;
using System;
using System.Globalization;
using System.Threading;
using System.Web;

namespace DITS.HILI.WMS.Web
{
    public class BaseUIPage : System.Web.UI.Page
    {


        protected override void OnLoad(EventArgs e)
        {
            Header.Controls.Add(new System.Web.UI.LiteralControl("<link rel='stylesheet' href='" + ResolveUrl("~/Css/wms.page.min.css") + "'/>"));
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            base.OnLoad(e);
        }

        //[DirectMethod]
        public static string GetResource(string key)
        {
            Core.Domain.ApiResponseMessage resp = WMSProperty.GetResource(key).Result;
            if (resp.IsSuccess)
            {
                CustomResource c = resp.Get<CustomResource>();
                return c.ResourceValue;
            }
            return "";
        }
        //[DirectMethod]
        public static CustomMessage GetMessage(string key)
        {
            CustomMessage defaultMsg = new CustomMessage { MessageCode = "SYS99999", MessageValue = "System error. Please contact your system administrator." };
            Core.Domain.ApiResponseMessage resp = WMSProperty.GetMessage(key).Result;
            if (resp.IsSuccess)
            {
                CustomMessage c = resp.Get<CustomMessage>();
                if (c == null)
                {
                    return defaultMsg;
                }
                return c;
            }
            return defaultMsg;
        }

        protected string GetCurrentPageName()
        {
            string sPath = Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

        protected override void OnInit(EventArgs e)
        {
            if (ClientService.Common.User.IsNull() && (GetCurrentPageName().ToLower().Replace(".aspx", "") != "logon"))
            {
                Response.Redirect("~/apps/Logout.aspx");
            }
            // this needed to initialize its base page class 
            base.OnInit(e);
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

        protected override void InitializeCulture()
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

            base.InitializeCulture();
        }
    }
}