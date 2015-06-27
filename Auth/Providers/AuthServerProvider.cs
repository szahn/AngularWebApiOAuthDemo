using System.Threading.Tasks;
using Demo.Auth.OAuth;
using Demo.Auth.User;
using Demo.Common;
using Microsoft.Owin.Security.OAuth;

namespace Demo.Auth.Providers
{
    public class AuthServerProvider : OAuthAuthorizationServerProvider
    {
        /*We may have additional clients we want to validate again, however, at the moment,
         we expect to serve only 1 client, otherwise we'll need to validate a client api key here.*/
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret; //The client secret is ignored as we can't share secrets on web clients
            if (!context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                context.Rejected();
                context.SetError("invalid_client", "The client is not available.");
                return;
            }

            var client = await GetClient(clientId);
            if (client == null || !client.IsActive)
            {
                context.Rejected();
                context.SetError("invalid_client", "The client is not available.");
                return;
            }

            context.Validated(client.ClientId);
        }

        /*Called when a form to the Token endpoint arrives with a "grant_type" of "password". 
         * This occurs when the user has provided name and password credentials directly into the
         * client application's user interface, and the client application is using those to acquire
         * an "access_token" and optional "refresh_token". If the web application supports the resource
         * owner credentials grant type it must validate the context.Username and context.Password as 
         * appropriate. To issue an access token the context.Validated must be called with a new ticket
         * containing the claims about the resource owner which should be associated with the access token. 
         * The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious 
         * callers. The default behavior is to reject this grant type. 
         * See also http://tools.ietf.org/html/rfc6749#section-4.3.2*/
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var client = await GetClient(context.ClientId);
            var user = await GetUser(context.UserName, context.Password);

            if (user == null 
                || string.IsNullOrEmpty(user.UserName))
            {
                context.Rejected();
                context.SetError("invalid_grant", "Invalid credentials.");
                return;
            }

            context.Validated(AuthTicketBuilder.BuildTicket(context.Options.AuthenticationType, user,
                client.AccessTokenLifetimeHours));
        }

        private static async Task<AuthClient> GetClient(string clientId)
        {
            using (var repository = new ClientRepository())
            {
                return await repository.GetClientAsync(clientId);
            };
        }

        /// <summary>
        /// Searches for the existence of the user based on company, username and password
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private static async Task<IDemoUser> GetUser(string userName, string password)
        {
            using (var repository = new UserRepository(new DemoUserDataSource()))
            {
                return await repository.FindUserAsync(userName, password);
            }
        }
    }
}
