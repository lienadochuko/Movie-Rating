using Azure;
using Movie_Rating.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace Movie_Rating.Services
{
    public interface ISignInService
    { 
      /// <summary>
      /// Login User
      /// </summary>
      /// <returns>Returns the token and user Details</returns>
        Task<bool> Login(LoginDTO loginDTO, CancellationToken cancellationToken);
    }

    public class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            string apiUrl = "http://localhost:5119/auth/Login";

            var content = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(apiUrl, content);

                // Throw an exception if the response status code doesn't indicate success
                response.EnsureSuccessStatusCode();

                // Read the response data
                string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

                // Deserialize the response JSON into a dynamic object
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseData);

                // Extract the token and user email from the response
                // Check if the token or user email is empty

                string token = jsonResponse?.token;
                string user = jsonResponse?.user?.email;

                if (string.IsNullOrEmpty(token) 
                    || string.IsNullOrEmpty(user))
                {
                    return false;
                }

                // Save the token and user information (implementation of SaveToken method)
                SaveToken(token, user);

                return true;
            }
        }

        public void SaveToken(string token, string user)
        {
            var response = _httpContextAccessor.HttpContext.Response;

            // Create cookie options
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
                Secure = true, // set to true if you're using HTTPS
                SameSite = SameSiteMode.Strict
            };

            // Add the cookies
            response.Cookies.Append("jwtToken", token, cookieOptions);
            response.Cookies.Append("userEmail", user, cookieOptions);
        }

        //public string GetTokenFromCookies()
        //{
        //    var Request = _httpContextAccessor.HttpContext.Request;
        //    return Request.Cookies["jwtToken"];
        //}
    }
}
