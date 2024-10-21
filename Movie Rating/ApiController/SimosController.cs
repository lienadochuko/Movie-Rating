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
        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            bool user = await signInService.Login(loginDTO, cancellationToken);

            if (user == false)
            {
                return Unauthorized(new { message = "Invalid login credentials." });
            }

            // Return the user details or token
            return Ok();
        }
    }
}

