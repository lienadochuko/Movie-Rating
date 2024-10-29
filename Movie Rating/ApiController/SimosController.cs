using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Models.DTO;
using Movie_Rating.Services;

namespace Movie_Rating.ApiController
{
    [Route("[controller]")]
    [ApiController]
    public class SimosController(ISignInService signInService) : ControllerBase
    {
		//[ValidateAntiForgeryToken]
        [HttpPost]
        [Route("[Action]")]
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

			// Start the background session check service
			return Ok();
        }
    }
}

