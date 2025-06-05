using EsunLibrarySystem.Models;
using System.Collections.Generic;

namespace EsunLibrarySystem.Services
{
    public interface IBookService
    {
        List<BookViewModel> GetAvailableBooks();
        bool BorrowBook(int userId, int inventoryId);
        List<BookViewModel> GetBorrowedBooks(int userId);
        bool ReturnBook(int userId, int inventoryId);

    }
}
