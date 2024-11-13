using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Rating.Helpers.Enums;
using Movie_Rating.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Movie_Rating.Services
{
	public interface ISignInService
	{
		/// <summary>
		/// Get Logged in user details
		/// </summary>
		/// <returns>object of user details</returns>
		Task<UserDTO> getUser();

		/// <summary>
		/// Get Logged in user image
		/// </summary>
		/// <returns>user image</returns>
		Task<string> getuserImage();

        /// <summary>
        /// Login User
        /// </summary>
        /// <returns>Returns the token and user Details</returns>
        Task<bool> Login(LoginDTO loginDTO, CancellationToken cancellationToken);

		/// <summary>
		/// Register User
		/// </summary>
		/// <returns>Returns the Registered user Details</returns>
		Task<bool> Register(RegisterDTO registerDTO, string image, CancellationToken cancellationToken);
	}

	public class SignInService : ISignInService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _configuration;

		public SignInService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}

		[ServiceFilter(typeof(SessionCheckFilter))]
		public async Task<UserDTO> getUser()
		{
			
			var userResponse = _httpContextAccessor.HttpContext.Request.Cookies["userEmail"];
			var user = JsonConvert.DeserializeObject<dynamic>(userResponse);

			if (user == null)
			{
				return null;
			}

			UserDTO userDTO = new()
			{
				Name = user?.Name,
				Gender = user?.Gender,
				Id = user?.Id,
				UserName = user?.UserName,
				NormalizedUserName = user?.NormalizedUserName,
				Email = user?.Email,
				NormalizedEmail = user?.NormalizedEmail,
				EmailConfirmed = user?.EmailConfirmed,
				PhoneNumber = user?.PhoneNumber,
				PhoneNumberConfirmed = user?.PhoneNumberConfirmed,
				TwoFactorEnabled = user?.TwoFactorEnabled,
			};

			return userDTO;
		}
		
		[ServiceFilter(typeof(SessionCheckFilter))]
		public async Task<string> getuserImage()
		{
			var session = _httpContextAccessor.HttpContext.Session;
			if (session.TryGetValue("UserImage", out var userImageBytes))
			{
				// Convert byte array back to Base64 string if needed
				string base64Image = Convert.ToBase64String(userImageBytes);
				return base64Image;
			}

			// Return null if the image is not found in session
			return null;
		}

		public async Task<bool> Register(RegisterDTO registerDTO, string image, CancellationToken cancellationToken)
		{
			string apiUrl = "https://localhost:7291/Auth/Register";
			Register register = new Register()
			{
				FirstName = registerDTO.FirstName,
				LastName = registerDTO.LastName,
				Email = registerDTO.Email,
				Password = registerDTO.Password,
				ConfirmPassword = registerDTO.ConfirmPassword,
				PhoneNumber = registerDTO.PhoneNumber,
				Gender = Enum.TryParse<GenderOptions>(registerDTO.Gender, out var gender) ? gender : default,
				UserType = Enum.TryParse<UserTypeOptions>(registerDTO.UserType, out var userType) ? userType : default,
                Image = image
            };


			var content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient())
			{
				var response = await client.PostAsync(apiUrl, content);

				// Throw an exception if the response status code doesn't indicate success
				response.EnsureSuccessStatusCode();

				// Read the response data
				string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

				// Deserialize the response JSON into a dynamic object
				//var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseData);

				// Extract the token and user email from the response
				// Check if the token or user email is empty

				//string Successresponse = responseData;

				if (string.IsNullOrEmpty(responseData)
					|| responseData == null)
				{
					return false;
				}

				return true;
			}
		}

		public async Task<bool> Login(LoginDTO loginDTO, CancellationToken cancellationToken)
		{
			string apiUrl = "https://localhost:7291/Auth/Login";

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

				var token = jsonResponse?.token;
				var user = jsonResponse?.userJson;
				var userImage = jsonResponse?.userImageJson;

				if (token == null || user == null || userImage == null)
				{
					return false;
				}

				// Save the token and user information (implementation of SaveToken method)
				SaveToken(token, user, userImage);

				return true;
			}
		}

        public void SaveToken(dynamic token, dynamic userResponse, dynamic userImage)
        {
            // Initialize the AES encryption service
            var aes = new AES(_configuration["AesGcm:Key"]);

            // Deserialize and decrypt the token
            var tokenData = JsonConvert.DeserializeObject<encrypt>(Convert.ToString(token));
            var decryptedToken = aes.Decrypt(
                Convert.FromBase64String(tokenData.EncryptedToken),
                Convert.FromBase64String(tokenData.TagBase64),
                Convert.FromBase64String(tokenData.NonceBase64)
            );

            // Deserialize and decrypt the user information
            var userResponseData = JsonConvert.DeserializeObject<encrypt>(Convert.ToString(userResponse));
            var decryptedUserResponse = aes.Decrypt(
                Convert.FromBase64String(userResponseData.EncryptedToken),
                Convert.FromBase64String(userResponseData.TagBase64),
                Convert.FromBase64String(userResponseData.NonceBase64)
            );

            // Deserialize and decrypt the user image
            var userImageData = JsonConvert.DeserializeObject<encrypt>(Convert.ToString(userImage));
            var decryptedUserImage = aes.Decrypt(
                Convert.FromBase64String(userImageData.EncryptedToken),
                Convert.FromBase64String(userImageData.TagBase64),
                Convert.FromBase64String(userImageData.NonceBase64)
            );

            // Access the HTTP response and session
            var response = _httpContextAccessor.HttpContext.Response;
            var session = _httpContextAccessor.HttpContext.Session;

            // Set up cookie options for security
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
                Secure = true, // Ensure this is true in production (HTTPS)
                SameSite = SameSiteMode.Strict
            };

            // Store decrypted data in cookies
            response.Cookies.Append("jwtToken", decryptedToken, cookieOptions);
            response.Cookies.Append("userEmail", decryptedUserResponse, cookieOptions);

            // Convert decrypted user image back to bytes and save in session
            byte[] userImageBytes = Convert.FromBase64String(decryptedUserImage);
            session.Set("UserImage", userImageBytes);
        }

    }
}
