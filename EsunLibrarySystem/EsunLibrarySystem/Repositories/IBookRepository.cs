using EsunLibrarySystem.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace EsunLibrarySystem.Repositories
{
    public interface IBookRepository
    {
        List<BookViewModel> GetAvailableBooks();
        bool BorrowBook(int userId, int inventoryId);
        List<BookViewModel> GetBorrowedBooks(int userId);
        bool ReturnBook(int userId, int inventoryId);

    }
}
