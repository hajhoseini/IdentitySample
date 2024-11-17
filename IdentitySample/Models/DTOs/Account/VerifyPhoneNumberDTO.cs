using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models.DTOs.Account
{
    public class VerifyPhoneNumberDTO
    {
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(6)]
        public string Code { get; set; }
    }
}
