using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Models;
using System.Diagnostics;
using System.Threading;

namespace Movie_Rating.Controllers
{
    
    public class AuthController(ILogger<AuthController> _logger) : Controller
    {
		public async Task<IActionResult> Login()
        {
            return View();
        }

		public async Task<IActionResult> Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
