using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Models;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Models;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Printing;
using Movie_Rating.Models.DTO;
using Movie_Rating.Services;
using Microsoft.AspNetCore.Authorization;

namespace Movie_Rating.Controllers
{
	[ServiceFilter(typeof(SessionCheckFilter))]
	public class HomeController(ILogger<HomeController> _logger, 
		IMoviesGetterServices moviesGetterServices,
		ISignInService signInService,
		ISessionChecker sessionChecker) : Controller
	{
		public async Task<IActionResult> Index( CancellationToken cancellationToken, int page = 1, int pageSize = 50)
		{

			//sessionCheckService.StartAsync(cancellationToken);
			//session session = await sessionChecker.CheckSession();

			ViewBag.Action = "Home";

			User();

			// Get all films with pagination
			PaginatedFilmViewModel paginatedFilmViewModel = await moviesGetterServices.GetAllFilms(cancellationToken, page, pageSize);

			//// Assuming GetAllFilms returns the right number of items per page
			//int totalItems = films.Count();

			//// Calculate the total number of pages
			//int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			//// Prepare ViewModel with pagination data
			//var model = new PaginatedFilmViewModel
			//{
			//	Films = films,
			//	CurrentPage = page,
			//	PageSize = pageSize,
			//	TotalItems = totalItems,
			//	TotalPages = totalPages // This is necessary for the pagination links
			//};

			return View(paginatedFilmViewModel);
		}


		public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
		{
			ViewBag.Action = "Home";

			User();

			UserDTO user = await signInService.getUser();

			return View(user);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		
		public async void User()
		{
			UserDTO user = await signInService.getUser();
			ViewBag.User = user;
		}
	}
}
