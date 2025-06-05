using Microsoft.AspNetCore.Mvc;
using EsunLibrarySystem.Models;
using EsunLibrarySystem.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EsunLibrarySystem.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Borrow()
        {
            var books = _bookService.GetAvailableBooks();
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Borrow(int inventoryId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                bool success = _bookService.BorrowBook(userId, inventoryId);
                if (success)
                    TempData["BorrowSuccess"] = "借書成功！";
                else
                    TempData["BorrowError"] = "借書失敗！";
            }
            catch (Exception ex)
            {
                TempData["BorrowError"] = "錯誤：" + ex.Message;
            }

            return RedirectToAction("Borrow");
        }

        [HttpGet]
        public IActionResult Return()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);
            var books = _bookService.GetBorrowedBooks(userId);
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Return(int inventoryId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                bool result = _bookService.ReturnBook(userId, inventoryId);
                if (result)
                    TempData["ReturnSuccess"] = "還書成功！";
                else
                    TempData["ReturnError"] = "還書失敗！";
            }
            catch (Exception ex)
            {
                TempData["ReturnError"] = $"錯誤：{ex.Message}";
            }

            return RedirectToAction("Return");
        }
    }
}
