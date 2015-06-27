using System.Threading.Tasks;
using Demo.Common;

namespace Demo.Auth.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDataSource _userDataSource;
        public UserRepository(IUserDataSource _userDataSource)
        {
            this._userDataSource = _userDataSource;
        }

        public async Task<IDemoUser> FindUserAsync(string userName, string password)
        {
            var user = await _userDataSource.GetUserAsync(userName, password);
            return user;
        }

        public void Dispose()
        {

        }
    }
}
