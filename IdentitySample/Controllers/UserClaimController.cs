using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Controllers
{
    public class UserClaimController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View(User.Claims);
        }
    }
}