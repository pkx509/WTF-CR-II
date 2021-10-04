using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.MasterModel.Core;
using DITS.HILI.WMS.MasterModel.Secure;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace DITS.HILI.WMS.Web.apps
{
    public partial class Logon : BaseUIPage
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                InitLang();
            }

        }

        protected void btnLogin_Click(object sender, DirectEventArgs e)
        {
            try
            {
                Core.Domain.ApiResponseMessage api = ClientService.WMSProperty.GetToken(txtUsername.Text, txtPassword.Text).Result;

                if (api.IsSuccess)
                {
                    ClientService.Common.AccessToken = (Token)api.Data;
                    Core.Domain.ApiResponseMessage apiResp = ClientService.WMSProperty.GetUser(txtUsername.Text).Result;

                    if (apiResp.IsSuccess)
                    {
                        UserAccounts user = apiResp.Get<UserAccounts>();
                        if (user != null)
                        {
                            ClientService.Common.User = user;
                            Response.Redirect("Default.aspx", true);
                        }
                    }
                    else
                    {
                        MessageBoxExt.ShowError(api.ResponseMessage);
                    }
                }
                else
                {
                    MessageBoxExt.ShowError(api.ResponseMessage);
                }
            }
            catch (HILIException ex)
            {
                MessageBoxExt.ShowError(GetMessage(ex.ErrorCode).MessageValue);
            }
            catch (Exception ex)
            {
                MessageBoxExt.ShowError(GetMessage("SYS99999").MessageValue);

                throw ex;
            }
        }


        private void InitLang()
        {
            Core.Domain.ApiResponseMessage apiResp = ClientService.WMSProperty.GetLanguages().Result;
            if (apiResp.IsSuccess)
            {
                List<Language> langs = apiResp.Get<List<Language>>();


                Language l = null;
                if (string.IsNullOrEmpty(ClientService.Common.Language))
                {
                    l = langs.Where(x => x.IsDefault == true).FirstOrDefault();
                }
                else
                {
                    l = langs.Where(x => x.LanguageCode == ClientService.Common.Language).FirstOrDefault();
                }

                if (l != null)
                {
                    ClientService.Common.Language = l.LanguageCode;

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(l.CultureCode);

                }
            }


        }
    }
}