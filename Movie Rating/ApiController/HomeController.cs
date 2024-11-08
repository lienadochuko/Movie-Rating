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
using Microsoft.IdentityModel.Tokens;

namespace Movie_Rating.ApiController
{
	[Route("[controller]")]
	[ServiceFilter(typeof(SessionCheckFilter))]
	[Controller]
	public class HomeController(ILogger<HomeController> _logger,
	  IMoviesGetterServices moviesGetterServices,
	  ISignInService signInService,
	  ISessionChecker sessionChecker) : Controller
	{
		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> Index(CancellationToken cancellationToken, string? title, int page = 1, int pageSize = 20)
		{
			ViewBag.Action = "Home";
			ViewBag.Title = title;

			User();
			PaginatedFilmViewModel paginatedFilmViewModel = null;

			// Get all films with pagination
			if (title.IsNullOrEmpty())
            {
                ViewBag.Reset = "0";
                paginatedFilmViewModel = await moviesGetterServices.GetAllFilms(cancellationToken, page, pageSize);
			}
			else
            {
                ViewBag.Reset = "1";
                paginatedFilmViewModel = await moviesGetterServices.GetFilmsByTitle(cancellationToken, title, page, pageSize);
			}

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

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> FilmSearchResult(CancellationToken cancellationToken, string title, int page = 1, int pageSize = 20)
		{

			ViewBag.Action = "Home";

			User();

			// Get all films with pagination
			PaginatedFilmViewModel paginatedFilmViewModel = await moviesGetterServices.GetFilmsByTitle(cancellationToken, title, page, pageSize);

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

		[HttpGet]
		[Route("[action]")]
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

		[Route("[action]")]
		public async void User()
		{
			UserDTO user = await signInService.getUser();
			string userImg = await signInService.getuserImage();
			ViewBag.User = user;
			ViewBag.UserImage = userImg;
		}
	}
}
