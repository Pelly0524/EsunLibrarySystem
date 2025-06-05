using EsunLibrarySystem.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace EsunLibrarySystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<BookViewModel> GetAvailableBooks()
        {
            var books = new List<BookViewModel>();

            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("CALL sp_GetAvailableBooks();", conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                books.Add(new BookViewModel
                {
                    InventoryId = Convert.ToInt32(reader["InventoryId"]),
                    ISBN = reader["ISBN"].ToString()!,
                    BookName = reader["BookName"].ToString()!,
                    Author = reader["Author"].ToString()!,
                    Status = reader["Status"].ToString()!
                });
            }

            return books;
        }

        public bool BorrowBook(int userId, int inventoryId)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                using var cmd = new MySqlCommand("CALL sp_BorrowBook(@userId, @inventoryId);", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@inventoryId", inventoryId);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }


        public List<BookViewModel> GetBorrowedBooks(int userId)
        {
            var books = new List<BookViewModel>();
            using var conn = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("CALL sp_GetBorrowedBooksByUser(@userId);", conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                books.Add(new BookViewModel
                {
                    InventoryId = Convert.ToInt32(reader["InventoryId"]),
                    ISBN = reader["ISBN"].ToString()!,
                    BookName = reader["BookName"].ToString()!,
                    Author = reader["Author"].ToString()!,
                    Status = reader["Status"].ToString()!
                });
            }

            return books;
        }

        public bool ReturnBook(int userId, int inventoryId)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                using var cmd = new MySqlCommand("CALL sp_ReturnBook(@userId, @inventoryId);", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@inventoryId", inventoryId);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }


    }
}
