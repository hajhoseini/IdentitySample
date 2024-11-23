using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IdentitySample.Helpers
{
    public class AddMyClaims : UserClaimsPrincipalFactory<User>
    {
        public AddMyClaims(UserManager<User> userManager, IOptions<IdentityOptions> options) : base(userManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            /*
             در اینجا به هر روشی مدنظرمان است مثلا از طریق فراخوانی یک ای پی آی لیست کلیم های موردنظ را به دست می آوریم
            و در ادامه آن ها را به ادامه identity اضافه می کنیم.
             */
            Claim newCliam = new Claim("FullName", $"{user.FirstName} {user.LastName}");

            var identity = await base.GenerateClaimsAsync(user);            
            identity.AddClaim(newCliam);

            return identity;
        }
    }
}
