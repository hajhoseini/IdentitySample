using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Admin, Operator")]

    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Operator")]
    public class TestController : Controller
    {
        //[Authorize]        
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Index2()
        {
            return View();
        }

        [Authorize(Roles = "Operator")]
        public IActionResult Index3()
        {
            return View();
        }
    }
}
