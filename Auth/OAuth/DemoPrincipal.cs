using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Demo.Common;

namespace Demo.Auth.OAuth
{

    public class DemoPrincipal : IDemoUser
    {
        private ClaimsPrincipal principal;

        /// <summary>
        /// Uses the adapter pattern to expose user properties from the ClaimsPrincipal
        /// </summary>
        /// <param name="principal"></param>
        public DemoPrincipal(IPrincipal principal)
        {
            this.principal = principal as ClaimsPrincipal;
            if (principal == null)
            {
                throw new Exception("Object is not a ClaimsPrincipal");
            }
        }

        private Claim GetClaim(string typeId)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type.Equals(typeId));
            if (claim == null)
            {
                throw new Exception("User claim not found in principal");
            }
            return claim;
        }

        private string GetClaimValue(string typeId)
        {
            var claim = GetClaim(typeId);
            return claim.Value;
        }

        private int GetClaimIntValue(string typeId)
        {
            var claim = GetClaim(typeId);
            return int.Parse(claim.Value);
        }

        public const string CLAIM_USER_NAME = "sub";

        public string UserName
        {
            get { return GetClaimValue(CLAIM_USER_NAME); }
        }

        public const string CLAIM_USER_ID = "id";

        public int Id
        {
            get { return GetClaimIntValue(CLAIM_USER_ID); }
        }

        public const string CLAIM_CLIENT_ID = "clientid";

        public string ClientId
        {
            get { return GetClaimValue(CLAIM_CLIENT_ID); }
        }

        public const string CLAIM_GENDER = "gender";

        public string Gender
        {
            get { return GetClaimValue(CLAIM_GENDER); }
        }

        public const string CLAIM_AGE = "age";

        public int Age
        {
            get { return GetClaimIntValue(CLAIM_AGE); }
        }

    }
}
