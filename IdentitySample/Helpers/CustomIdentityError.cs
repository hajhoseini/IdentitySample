using Microsoft.AspNetCore.Identity;

namespace IdentitySample.Helpers
{
    public class CustomIdentityError : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = $"طول پسورد باید حداقل {length} کاراکتر باشد"
            };
        }
    }
}
