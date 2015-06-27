using System;
using System.Threading.Tasks;

namespace Demo.Auth
{
    public interface IClientRepository : IDisposable
    {
        Task<AuthClient> GetClientAsync(string clientId);
        AuthClient GetClient(string clientId);
    }
}