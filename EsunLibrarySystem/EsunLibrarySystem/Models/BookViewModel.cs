namespace EsunLibrarySystem.Models
{
    public class BookViewModel
    {
        public int InventoryId { get; set; }
        public string ISBN { get; set; } = null!;
        public string BookName { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
