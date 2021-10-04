using DITS.HILI.WMS.Core.CustomException;
using DITS.HILI.WMS.Core.Resource;
using DITS.HILI.WMS.MasterService.Secure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WebAPIs
{
    public class WMSAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        private readonly IUserAccountService service;
        private readonly IMessageService message;
        public WMSAuthorizationServerProvider(IUserAccountService u, IMessageService m)
        {
            service = u;
            message = m;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {


            //if (!context.TryGetBasicCredentials(out client_Id, out client_Secret))
            //{
            //    //Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Critical, MethodBase.GetCurrentMethod().Name,
            //    //    new Exception(MessageManger.GetMessage(Message.Core.SYS10003)));

            //    //context.SetError(MessageManger.GetMessage(Message.Core.SYS10003));
            //    context.SetError(message.GetMessage("SYS10003","en").MessageValue);
            //    return;
            //}

            //if ((!Configure.ClientId.Equals(client_Id)) && (!Configure.ClientSecret.Equals(client_Secret)))
            //{
            //    //Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Critical, MethodBase.GetCurrentMethod().Name,
            //    //    new Exception(MessageManger.GetMessage(Message.Core.SYS10004)));
            //    //context.SetError(MessageManger.GetMessage(Message.Core.SYS10004));
            //    context.SetError(message.GetMessage("SYS10004", "en").MessageValue);
            //    return;
            //}


            context.OwinContext.Set<string>("as:clientAllowedOrigin", Configure.AllowOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", Configure.RefreshTokenLifeTime.ToString());

            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            string allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null)
            {
                allowedOrigin = Configure.AllowOrigin;
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });


            try
            {

                bool isLogin = service.Login(context.UserName, context.Password);

            }
            catch (HILIException ex)
            {
                context.SetError(message.GetMessage(ex.ErrorCode, "en").MessageValue);

                //context.SetError(ex.Message);
                return;
            }

            //var role = user.UserInRoleCollection.FirstOrDefault().Role.Name;

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim("sub", context.UserName));

            AuthenticationProperties props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    }
                });

            AuthenticationTicket ticket = new AuthenticationTicket(identity, props);
            context.Validated(identity);
        }


        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            string originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            string currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                //Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Critical, MethodBase.GetCurrentMethod().Name,
                //    new Exception(MessageManger.GetMessage(Message.Core.SYS10005)));
                //context.SetError("invalid_clientId", Message.Core.SYS10005);
                context.SetError(message.GetMessage("SYS10005", "en").MessageValue);

                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            ClaimsIdentity newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            Claim newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            AuthenticationTicket newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}