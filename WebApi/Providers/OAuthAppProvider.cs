using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace WebApi.Providers
{
    public partial class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            context.TryGetFormCredentials(out clientId, out string clientSecret);
            if (!string.IsNullOrEmpty(clientId))
            {
                context.Validated(clientId);
            }
            else
            {
                context.Validated();
            }

            var task = base.ValidateClientAuthentication(context);
            return task;
        }

        public override Task
            GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    bool isValid = false;
                    isValid = true; //This should be the Service/DB call to validate the client id, client secret.
                    //ValidateApp(context.ClientId, clientSecret);

                    if (isValid)
                    {
                        var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                        oAuthIdentity.AddClaim(new Claim("ClientID", context.ClientId));
                        var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
                        context.Validated(ticket);
                    }
                    else
                    {
                        context.SetError("Error", "Invalid");
                        //logger.Error(string.Format("GrantResourceOwnerCredentials(){0}Credentials not valid for ClientID : {1}.", Environment.NewLine, context.ClientId));
                    }
                }
                catch (Exception)
                {
                    context.SetError("Error", "internal server error");
                    //logger.Error(string.Format("GrantResourceOwnerCredentials(){0}Returned tuple is null for ClientID : {1}.", Environment.NewLine, context.ClientId));
                }
            });
        }
    }
}