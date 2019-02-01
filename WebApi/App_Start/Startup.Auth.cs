using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
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
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        
        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new OAuthAppProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(General.AccessTokenExpiryDays),
                AllowInsecureHttp = General.UseHttp
            };
            app.UseOAuthBearerTokens(OAuthOptions);
            LogHelper.CreateLog("ConfigureAuth method");
        }
    }
}