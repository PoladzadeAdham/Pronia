using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.UserViewModel
{
    public class LoginVm
    {
        [Required, MaxLength(256), EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(6), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool IsRemember { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
