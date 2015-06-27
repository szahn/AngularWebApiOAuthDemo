using System;

namespace Demo.Auth.User
{
    public class UserOperationNotSupportedExecption : Exception
    {
        public UserOperationNotSupportedExecption(string message) : base(message)
        {
            
        }
    }
}
