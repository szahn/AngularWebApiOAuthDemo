using Demo.Common;
using Microsoft.AspNet.Identity;

namespace Demo.Auth.User
{
    public class DemoUser : IDemoUser, IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string Gender { get; set;  }
        public int Age { get; set; }
    }
}
