using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Demo.Auth.OAuth
{
    /// <summary>
    /// Authorize a controller/action by validating the existence of a GUID in the user
    /// </summary>
    public class AuthorizeUser : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                var principal = GetPrincipal(actionContext);
                Validate(principal);
                return base.IsAuthorized(actionContext);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return false;
        }

        private void Validate(DemoPrincipal principal)
        {
            if (string.IsNullOrWhiteSpace(principal.UserName))
            {
                throw new MalformedTicketException();
            }

        }

        private static DemoPrincipal GetPrincipal(HttpActionContext actionContext)
        {
            return new DemoPrincipal(actionContext.RequestContext.Principal);
        }

    }
}
