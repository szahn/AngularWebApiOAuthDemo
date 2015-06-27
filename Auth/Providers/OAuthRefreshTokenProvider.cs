using System.Threading.Tasks;
using Demo.Auth.OAuth;
using Microsoft.Owin.Security.Infrastructure;

namespace Demo.Auth.Providers
{
    internal class OAuthRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            context.SetToken(new CustomTokenFormat().Protect(context.Ticket));
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            return Task.Run(() =>
            {
                Create(context);
            });
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            //TODO: before generating a new ticket, validate the ticket has not expired, and has never
            //been used to generate a new ticket
            var token = new CustomTokenFormat().Unprotect(context.Token.Replace(" ", "+"));
            var ticket = AuthTicketBuilder.BuildTicket(token);
            context.SetTicket(ticket);
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            return Task.Run(() =>
            {
                Receive(context);
            });
        }
    }
}