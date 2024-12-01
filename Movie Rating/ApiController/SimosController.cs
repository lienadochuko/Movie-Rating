using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Models.DTO;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Services;

namespace Movie_Rating.ApiController
{
    [Route("[controller]")]
    [ApiController]
    public class SimosController(ISignInService signInService, 
        IMoviesUpdaterServices moviesUpdaterServices) : ControllerBase
    {
        [Route("[Action]")]
        [HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            bool user = await signInService.Login(loginDTO, cancellationToken);

            if (user == false)
            {
                return Unauthorized(new { message = "Invalid login credentials." });
            }

			// Save the login time in a cookie
			var loginTime = DateTime.UtcNow;
			Response.Cookies.Append("LoginTime", loginTime.ToString(), new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = loginTime.AddMinutes(5) // Set cookie to expire after 30 mins
			});

			return Ok("Logged in sucess");
         }
		

        [HttpPost]
        [Route("[Action]")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO, CancellationToken cancellationToken)
        {
            var image = await FetchRandomCartoonImageBase64Async();

            bool user = await signInService.Register(registerDTO, image, cancellationToken);

            if (user == false)
            {
                return Unauthorized(new { message = "Invalid credentials used." });
            }

			// Start the background session check service
			return Ok();
        }

		[HttpGet("[action]/{Id}")]
		public async Task<IActionResult> Like(int Id, CancellationToken cancellationToken = default)
		{
			if (Id <= 0)
			{
				return BadRequest("Invalid Film ID.");
			}

			UserDTO user = await signInService.getUser();
			if (user == null)
			{
				return Unauthorized("User is not authenticated.");
			}

			try
			{
				bool result = await moviesUpdaterServices.AddUserFilmLike(cancellationToken, Id, user.Id.ToString());

				if (!result)
				{
					return BadRequest("Failed to update like status.");
				}

				return Ok(true);
			}
			catch (Exception ex)
			{				
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}

		private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task<string> FetchRandomCartoonImageBase64Async()
        {
            string uniqueString = GenerateRandomString(8); // Generates an 8-character random string
            string apiUrl = $"https://robohash.org/{uniqueString}.png?set=set4";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                        return Convert.ToBase64String(imageBytes);
                    }
                    else
                    {
                        // Log the unsuccessful status code
                        Console.WriteLine($"API call unsuccessful: {response.StatusCode}. Using fallback image.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    Console.WriteLine($"Error fetching image from API: {ex.Message}. Using fallback image.");
                }

                // If API call fails or any exception occurs, return a generated Base64 image
                return null;
            }
        }
    }
}

