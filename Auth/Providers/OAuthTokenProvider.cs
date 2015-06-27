using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;

namespace Demo.Auth.Providers
{
    public class OAuthTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {

        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            return Task.Run(() =>
            {
                Create(context);
            });
        }

        /// <summary>
        /// When the server receives the token, decrypt it and place it in the request context user object
        /// so that subsequent routines can consume it.
        /// </summary>
        /// <param name="context"></param>
        public void Receive(AuthenticationTokenReceiveContext context)
        {

        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            return Task.Run(() => Receive(context));
        }
    }
}
