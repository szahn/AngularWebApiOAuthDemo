using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Auth
{
    public class ClientRepository : IClientRepository
    {
        private readonly List<AuthClient> clients;

        public ClientRepository()
        {
            clients = new List<AuthClient>
            {
                {
                    new AuthClient {
                        AccessTokenLifetimeHours = 1,
                        CompanyName = "Demo",
                        ClientName = "Demo",
                        ClientDescription = "Demo",
                        ClientId = "123",
                        IsActive = true
                }}
            };
        }

        public async Task<AuthClient> GetClientAsync(string clientId)
        {
            return await Task.FromResult(GetClient(clientId));
        }

        public AuthClient GetClient(string clientId)
        {
            return clients.FirstOrDefault(c => c.ClientId == clientId);
        }

        public void Dispose()
        {
        }
    }
}
