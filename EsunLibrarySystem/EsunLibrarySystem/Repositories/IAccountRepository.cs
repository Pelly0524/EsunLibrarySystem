namespace EsunLibrarySystem.Repositories
{
    public interface IAccountRepository
    {
        bool CreateUser(string phone, string hashedPassword, string salt, string userName);
        AuthInfo GetAuthInfo(string phone);
        bool UpdateLastLoginTime(int userId);
    }

    public class AuthInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
}
