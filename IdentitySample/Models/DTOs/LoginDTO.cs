using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool IsPersistent { get; set; } = false;

        public string ReturnUrl { get; set; }
    }
}
