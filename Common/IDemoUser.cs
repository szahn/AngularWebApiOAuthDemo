namespace Demo.Common
{
    public interface IDemoUser
    {
        int Id { get;}
        string UserName { get; }
        string ClientId { get; }
        string Gender { get; }
        int Age { get; }
    }
}
