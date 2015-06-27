using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Demo.Common;

namespace Demo.Auth.User
{
    public sealed class DemoUserDataSource : IUserDataSource
    {
        private static List<IDemoUser> users = new List<IDemoUser>
        {
            new DemoUser
            {
                Age = 32,
                ClientId = "123",
                Gender = "Male",
                Id = 1,
                UserName = "demo",                
            }
        };

        public async Task<IDemoUser> GetUserAsync(string userName, string password)
        {
            return await Task.FromResult(GetUser(userName, password));
        }

        //TODO: implement custom user/password validation here
        public IDemoUser GetUser(string userName, string password)
        {
            if (password != "demo")
            {
                return null;
            }

            return  users.FirstOrDefault(u => u.UserName.Equals(userName));
        }

    }
}
