namespace Demo.Auth
{
    public class AuthClient
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientDescription { get; set; }
        public string CompanyName { get; set; }
        public int AccessTokenLifetimeHours { get; set; }
        public bool IsActive { get; set; }
    }
}
