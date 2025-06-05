using System.ComponentModel.DataAnnotations;

namespace EsunLibrarySystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }
    }
}
