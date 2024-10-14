using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Models;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Models;
using System.Diagnostics;

namespace Movie_Rating.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IMoviesGetterServices moviesGetterServices) : Controller
    {       
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
			ViewBag.Action = "Home";
			IEnumerable<FilmDTO> film = await moviesGetterServices.GetAllFilms(cancellationToken);
			return View(film);
        }

        public IActionResult Privacy()
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
