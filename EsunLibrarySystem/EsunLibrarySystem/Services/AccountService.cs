using System.Security.Cryptography;
using System.Text;
using EsunLibrarySystem.Models;
using EsunLibrarySystem.Repositories;

namespace EsunLibrarySystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public bool Register(RegisterViewModel model)
        {
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(model.Password, salt);
            return _repository.CreateUser(model.PhoneNumber, hashedPassword, salt, model.UserName);
        }

        public LoginResult ValidateLogin(string phone, string password)
        {
            var authInfo = _repository.GetAuthInfo(phone);
            if (authInfo == null) return null;

            var inputHash = HashPassword(password, authInfo.Salt);
            if (inputHash != authInfo.PasswordHash) return null;

            return new LoginResult { UserId = authInfo.UserId, UserName = authInfo.UserName };
        }

        public void UpdateLastLogin(int userId)
        {
            _repository.UpdateLastLoginTime(userId);
        }

        private string GenerateSalt(int length = 16)
        {
            var buffer = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        private string HashPassword(string password, string salt)
        {
            var combined = password + salt;
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(bytes);
        }
    }
}
