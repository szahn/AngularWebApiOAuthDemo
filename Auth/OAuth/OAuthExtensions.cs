using System;
using Demo.Auth.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Demo.Auth.OAuth
{
    public static class OAuthExtensions
    {
        public static void UseOAuth(this IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(GetAuthorizationOptions());
            app.UseOAuthBearerAuthentication(GetBearerOptions());
        }

        private static OAuthBearerAuthenticationOptions GetBearerOptions()
        {
            return new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AccessTokenProvider = new OAuthTokenProvider(),
                AccessTokenFormat = new CustomTokenFormat()
            };
        }

        private static OAuthAuthorizationServerOptions GetAuthorizationOptions()
        {
            return new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                Provider = new AuthServerProvider(),
                RefreshTokenProvider = new OAuthRefreshTokenProvider(),
                ApplicationCanDisplayErrors = true,
                AccessTokenFormat = new CustomTokenFormat(),
                RefreshTokenFormat = new CustomTokenFormat(),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(30)  
            };
        }
    }
}
