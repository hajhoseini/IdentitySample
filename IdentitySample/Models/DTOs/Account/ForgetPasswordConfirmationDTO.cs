using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models.DTOs.Account
{
    public class ForgetPasswordConfirmationDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
