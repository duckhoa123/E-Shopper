using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
