using System;
using System.Security.Claims;
using Demo.Common;
using Microsoft.Owin.Security;

namespace Demo.Auth.OAuth
{
    public class AuthTicketBuilder
    {
        public static AuthenticationTicket BuildTicket(string authType, IDemoUser user,
            int accessTokenExpirationHours)
        {
            return new AuthenticationTicket(BuildIdentity(authType, user), 
                BuildAuthProps());
        }

        public static AuthenticationTicket BuildTicket(AuthenticationTicket ticket)
        {
            var claims = new ClaimsIdentity(ticket.Identity);
            var props = BuildAuthProps();
            return new AuthenticationTicket(claims, props);
        }

        /// <summary>
        /// Builds the user claims identity model by hydrating the claims from the user model,
        /// These claims will be serialzied into the bearer token.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private static ClaimsIdentity BuildIdentity(string authType, IDemoUser user)
        {
            var identity = new ClaimsIdentity(authType);
            identity.AddClaim(new Claim(DemoPrincipal.CLAIM_AGE, user.Age.ToString()));
            identity.AddClaim(new Claim(DemoPrincipal.CLAIM_GENDER, user.Gender));
            identity.AddClaim(new Claim(DemoPrincipal.CLAIM_USER_NAME, user.UserName));
            identity.AddClaim(new Claim(DemoPrincipal.CLAIM_USER_ID, user.Id.ToString()));
            return identity;
        }

        private static AuthenticationProperties BuildAuthProps()
        {
            return new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            };
        }
    }
}
