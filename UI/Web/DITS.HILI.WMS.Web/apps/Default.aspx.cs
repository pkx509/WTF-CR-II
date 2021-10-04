using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.Web.Common.UI;
using Ext.Net;
using Ext.Net.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace DITS.HILI.WMS.Web.apps
{
    public partial class Default : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Redirect Page
            if (ClientService.Common.User.IsNull())
            {
                Response.Redirect("Logon.aspx?ReturnPage=" + HttpContext.Current.Request.Url.AbsoluteUri);
            }
            #endregion Redirect Page

            if (!X.IsAjaxRequest)
            {
                CreateMenu();
                AddItems();
                #region [ Program Version ]
                StatusBarMain.Text = ConfigurationManager.AppSettings["version"].ToString(); ;
                #endregion [ Program Version ]

                ResourceManager1.DirectEventUrl = Request.Url.AbsoluteUri;

                Theme theme = Ext.Net.Theme.Gray;

                if (Session["Ext.Net.Theme"] != null)
                {
                    theme = (Theme)Session["Ext.Net.Theme"];
                }

                ResourceManager.RegisterControlResources<TagLabel>();
                DynamicMenu.BulidingTree(panelMainMenu);
                MenuButton.Text = "<span style='font-weight:bold ; color:#fff;'>" + ClientService.Common.User.UserName + "</span>";

                //Change Password
                // this.txtUserName.Text = AppsInfo.UserLogin;
            }


        }

        private void CreateMenu()
        {
            Core.Domain.ApiResponseMessage apiResp = ClientService.WMSProperty.GetLanguages().Result;
            if (apiResp.IsSuccess)
            {
                List<Language> langs = apiResp.Get<List<Language>>();


                Language l = null;
                if (Request.QueryString["lang"] != null && !string.IsNullOrEmpty(Request.QueryString["lang"].ToString()))
                {
                    l = langs.Where(x => x.LanguageCode == Request.QueryString["lang"].ToString()).FirstOrDefault();
                }
                else if (string.IsNullOrEmpty(ClientService.Common.Language))
                {
                    l = langs.Where(x => x.IsDefault == true).FirstOrDefault();
                }
                else
                {
                    l = langs.Where(x => x.LanguageCode == ClientService.Common.Language).FirstOrDefault();
                }

                if (l != null)
                {
                    LangButton.Text = l.LanguageName;
                    LangButton.Icon = (Icon)Enum.Parse(typeof(Icon), l.Flag);
                    ClientService.Common.Language = l.LanguageCode;



                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(l.CultureCode);

                }
            }


        }


        protected void btnLogout_Click(object sender, DirectEventArgs e)
        {
            X.Msg.Confirm("Confirmation", "Please confirmation for exit program", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig { Handler = "CompanyX.DoYes();", Text = "Yes" },
                No = new MessageBoxButtonConfig { Handler = "CompanyX.DoNo();", Text = "No" }
            }).Show();

        }

        [DirectMethod(Timeout=180000)]
        public void DoYes()
        {
            Session.Abandon();
            Session.Clear();
            if (ClientService.Common.AccessToken == null)
            {
                Response.Redirect("~/");
            }
        }

        [DirectMethod(Timeout=180000)]
        public void DoNo()
        {
        }

        [DirectMethod(Timeout=180000)]
        public void DoConfirm()
        {
            X.Msg.Confirm("Message", "Change password complete, system logout.", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "CompanyX.DoLogoutYes()",
                    Text = "Yes"
                }
            }).Show();
        }

        [DirectMethod(Timeout=180000)]
        public void DoLogoutYes()
        {
            Response.Redirect("~/");
        }


        [DirectMethod(Timeout=180000)]
        public void ChangeLangauge(string lang)
        {

            ClientService.Common.Language = lang;
            X.Reload();

        }


        public void AddItems()
        {

            List<Ext.Net.MenuItem> items = new List<Ext.Net.MenuItem>();

            Core.Domain.ApiResponseMessage apiResp = ClientService.WMSProperty.GetLanguages().Result;
            if (apiResp.IsSuccess)
            {
                List<Language> langs = apiResp.Get<List<Language>>();

                langs.ForEach(x =>
                    {
                        Ext.Net.Parameter prmDesc = new Ext.Net.Parameter()
                        {
                            Name = "lang",
                            Value = x.LanguageCode,
                            Mode = ParameterMode.Value
                        };
                        MenuItem item = new Ext.Net.MenuItem(x.LanguageName)
                        {
                            Icon = (Icon)Enum.Parse(typeof(Icon), x.Flag)

                        };
                        item.OnClientClick = "window.location = 'Default.aspx?lang=" + x.LanguageCode + "';";
                        // item.DirectEvents.Click.ExtraParams.Add(prmDesc);

                        langMenu.Items.Add(item);
                        //items.Add(item);
                    });
            }


        }

    }
}