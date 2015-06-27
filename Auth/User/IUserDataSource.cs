using System.Threading.Tasks;
using Demo.Common;

namespace Demo.Auth.User
{
    public interface IUserDataSource
    {
        Task<IDemoUser> GetUserAsync(string userName, string password);
        IDemoUser GetUser(string userName, string password);
    }
}
