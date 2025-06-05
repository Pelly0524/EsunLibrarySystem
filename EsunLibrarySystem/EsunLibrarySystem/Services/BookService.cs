using EsunLibrarySystem.Models;
using EsunLibrarySystem.Repositories;

namespace EsunLibrarySystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public List<BookViewModel> GetAvailableBooks()
        {
            return _repository.GetAvailableBooks();
        }

        public bool BorrowBook(int userId, int inventoryId)
        {
            return _repository.BorrowBook(userId, inventoryId);
        }

        public List<BookViewModel> GetBorrowedBooks(int userId)
        {
            return _repository.GetBorrowedBooks(userId);
        }

        public bool ReturnBook(int userId, int inventoryId)
        {
            return _repository.ReturnBook(userId, inventoryId);
        }

    }
}
