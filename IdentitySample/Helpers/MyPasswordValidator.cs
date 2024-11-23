using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentitySample.Helpers
{
    public class MyPasswordValidator : IPasswordValidator<User>
    {
        List<string> CommonPassword = new List<string>()
        {
            "123456","password", "123456789"
        };

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            if(CommonPassword.Contains(password))
            {
                var error = new IdentityError
                                    {
                                        Code = "CommonPassword",
                                        Description = "لطفا یک پسورد قوی انتخاب کنید"
                                    };
                var result = IdentityResult.Failed(error);
                return Task.FromResult(result);
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
