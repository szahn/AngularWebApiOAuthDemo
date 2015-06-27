using System;
using System.Threading.Tasks;
using Demo.Common;

namespace Demo.Auth.User
{
    public interface IUserRepository : IDisposable
    {
        Task<IDemoUser> FindUserAsync(string userName, string password);
    }
}