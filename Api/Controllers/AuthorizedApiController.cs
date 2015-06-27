using System.Web.Http;
using Demo.Auth.OAuth;
using Demo.Common;

namespace Demo.Api.Controllers
{
    [AuthorizeUser]
    public class AuthorizedApiController : ApiController
    {
        protected IDemoUser DemoUser
        {
            get
            {
                return new DemoPrincipal(ActionContext.RequestContext.Principal);
            }
        }
    }
}
