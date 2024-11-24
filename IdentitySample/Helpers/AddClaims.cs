using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentitySample.Helpers
{
    public class AddClaims : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null)
            {
                var identity = principal.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    /*
                     در اینجا به هر روشی مدنظرمان است مثلا از طریق فراخوانی یک ای پی آی لیست کلیم های موردنظ را به دست می آوریم
                    و در ادامه آن ها را به ادامه identity اضافه می کنیم.
                     */
                    Claim newCliam = new Claim("Test", "aaaa");
                    identity.AddClaim(newCliam);
                }
            }

            return Task.FromResult(principal);
        }
    }
}
