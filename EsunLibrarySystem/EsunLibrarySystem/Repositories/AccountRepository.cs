using MySql.Data.MySqlClient;

namespace EsunLibrarySystem.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool CreateUser(string phone, string hashedPassword, string salt, string userName)
        {
            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("CALL sp_RegisterUser(@phone, @pass, @salt, @name);", conn);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@pass", hashedPassword);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@name", userName);

            conn.Open();
            var result = cmd.ExecuteNonQuery();
            return result > 0;
        }

        public AuthInfo GetAuthInfo(string phone)
        {
            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("CALL sp_GetUserAuthInfo(@phone);", conn);
            cmd.Parameters.AddWithValue("@phone", phone);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new AuthInfo
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    UserName = reader["UserName"].ToString()!,
                    PasswordHash = reader["PasswordHash"].ToString()!,
                    Salt = reader["Salt"].ToString()!
                };
            }
            return null;
        }

        public void UpdateLastLoginTime(int userId)
        {
            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("CALL sp_UpdateLastLoginTime(@userId);", conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
