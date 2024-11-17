using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models.DTOs.Account
{
    public class TwoFactorLoginDTO
    {
        [Required]
        public string Code { get; set; }

        public bool IsPersistent { get; set; }

        public string Provider { get; set; }
    }
}
