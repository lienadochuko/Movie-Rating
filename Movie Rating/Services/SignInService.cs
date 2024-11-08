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

		public SignInService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
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
				Name = user?.name,
				Gender = user?.gender,
				Id = user?.id,
				UserName = user?.userName,
				NormalizedUserName = user?.normalizedUserName,
				Email = user?.email,
				NormalizedEmail = user?.normalizedEmail,
				EmailConfirmed = user?.emailConfirmed,
				PhoneNumber = user?.phoneNumber,
				PhoneNumberConfirmed = user?.phoneNumberConfirmed,
				TwoFactorEnabled = user?.twoFactorEnabled,
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

				string token = jsonResponse?.token;
				var user = jsonResponse?.user;
				string userImage = jsonResponse?.userImage;

				if (string.IsNullOrEmpty(token)
					|| user == null)
				{
					return false;
				}

				// Save the token and user information (implementation of SaveToken method)
				SaveToken(token, user, userImage);

				return true;
			}
		}

		public void SaveToken(string token, object userResponse, string userImage)
		{
			var response = _httpContextAccessor.HttpContext.Response;
			var session = _httpContextAccessor.HttpContext.Session;

			// Create cookie options
			CookieOptions cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTime.Now.AddHours(1),
				Secure = true, // set to true if you're using HTTPS
				SameSite = SameSiteMode.Strict
			};
			var user = JsonConvert.SerializeObject(userResponse);

			// Add the cookies
			response.Cookies.Append("jwtToken", token, cookieOptions);
			response.Cookies.Append("userEmail", user, cookieOptions);
			//response.Cookies.Append("userImage", userImage, cookieOptions);

			byte[] userImageBytes = Convert.FromBase64String(userImage);
			session.Set("UserImage", userImageBytes);
		}
	}
}
