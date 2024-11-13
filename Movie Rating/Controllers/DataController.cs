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

namespace Movie_Rating.Controllers
{

    [Route("[controller]")]
    [ServiceFilter(typeof(SessionCheckFilter))]
	[ApiController]
	public class DataController(ILogger<DataController> _logger,
		IMoviesUpdaterServices moviesUpdaterServices,
		IMoviesGetterServices moviesGetterServices,
		ISignInService signInService,
		ISessionChecker sessionChecker) : ControllerBase
	{
		[Route("[action]")]
		[HttpPost]
		public async Task<IActionResult> UpdateFilmPosters(CancellationToken cancellationToken)
		{			
			bool response = await moviesUpdaterServices.UpdateFilmPosters(cancellationToken);
			
			if(response)
			{
			return Ok();
			}
			else
			{
				return BadRequest();
			}
		}		

	}
}
