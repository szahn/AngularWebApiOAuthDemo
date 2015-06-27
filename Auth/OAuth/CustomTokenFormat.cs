using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;

namespace Demo.Auth.OAuth
{
    public class CustomTokenFormat : SecureDataFormat<AuthenticationTicket>
    {
        public CustomTokenFormat() : base(
            new TicketSerializer(), new CustomDataProtector(), new Base64TextEncoder())
        {
            
        }

    }

    //TODO: encrypt the data using your customized encryption routine
    public class CustomDataProtector : IDataProtector
    {
        public byte[] Protect(byte[] userData)
        {
            return userData;
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return protectedData;
        }
    }
}
