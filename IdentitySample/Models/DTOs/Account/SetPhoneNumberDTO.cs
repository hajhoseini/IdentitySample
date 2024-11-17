using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models.DTOs.Account
{
    public class SetPhoneNumberDTO
    {
        [Required]
        [RegularExpression(@"(\+98[0)?9\d{9}")]
        public string PhoneNumber { get; set; }
    }
}
