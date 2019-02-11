using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApi.Infrastructure;
using WebApi.Providers;

namespace WebApi
{
    public class General
    {
        public static double AccessTokenExpiryDays { get; set; } = 1;

        public static bool UseHttp { get; set; } = true;
    }

	public partial class Startup
	{
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            app.CreatePerOwinContext<RepositoryUserManager>(RepositoryUserManager.Create);
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = General.UseHttp,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(General.AccessTokenExpiryDays),
                Provider = new OAuthAppProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost:59822")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }
    }
}