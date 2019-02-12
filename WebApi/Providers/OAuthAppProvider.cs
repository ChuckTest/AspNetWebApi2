using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using WebApi.Infrastructure;

namespace WebApi.Providers
{
    public partial class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        //https://docs.microsoft.com/en-us/previous-versions/aspnet/mt180817(v%3Dvs.113)
        /*Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are present on the request.
         If the web application accepts Basic authentication credentials, context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. 
         If the web application accepts "client_id" and "client_secret" as form encoded POST parameters, context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body. 
         If context.Validated is not called the request will not proceed further.*/
        /// <summary>
        /// validate that the origin of the request is a registered client_id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.TryGetFormCredentials(out var clientId, out string clientSecret))
            {
                try
                {
                    Client client = ClientManager.FindClient(clientId);

                    if (client != null) //need check the client secret here
                    {
                        context.Validated(clientId);
                    }
                    else
                    {
                        // Client could not be validated.
                        context.SetError("invalid_client", "Client credentials are invalid.");
                        context.Rejected();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.CreateLog(ex);
                    // Could not get the client through the IClientManager implementation.
                    context.SetError("server_error");
                    context.Rejected();
                }
            }
            else
            {
                // The client credentials could not be retrieved.
                context.SetError(
                    "invalid_client",
                    "Client credentials could not be retrieved through the request body.");

                context.Rejected();
            }
            var task = base.ValidateClientAuthentication(context);
            return task;
        }

        /// <summary>
        /// validate provided username and password when the grant_type is set to password
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<RepositoryUserManager>();

            RepositoryUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = userManager.GenerateUserIdentityAsync("JWT");

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);

        }

    }
}