using System.ComponentModel.DataAnnotations;

namespace EsunLibrarySystem.Models
{
    public class LoginViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = null!;
    }
}
