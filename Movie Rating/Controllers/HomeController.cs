using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Models;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Models;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Printing;
using Movie_Rating.Models.DTO;

namespace Movie_Rating.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IMoviesGetterServices moviesGetterServices) : Controller
    {
        public async Task<IActionResult> Index(Object user, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            

            ViewBag.Action = "Home";
			ViewBag.User = user;
			IEnumerable<FilmDTO> film = await moviesGetterServices.GetAllFilms(cancellationToken);
            
            int totalItems = film.Count();

            // Calculate the films to display on the current page
            var paginatedFilms = film.OrderBy(f => f.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Prepare a ViewModel to pass pagination data
            var model = new PaginatedFilmViewModel
            {
                Films = paginatedFilms,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
