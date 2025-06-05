using EsunLibrarySystem.Models;

namespace EsunLibrarySystem.Services
{
    public interface IAccountService
    {
        bool Register(RegisterViewModel model);
        LoginResult ValidateLogin(string phone, string password);
        bool UpdateLastLogin(int userId);
    }

    public class LoginResult
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
    }
}
