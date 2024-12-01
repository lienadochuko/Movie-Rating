using System;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Drawing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;
using Movie_Rating.ServiceContracts;
using Movie_Rating.RepositoryContracts;
using Movie_Rating.Models.DTO;
using Movie_Rating.Helpers;
using Movie_Rating.Models;
using Newtonsoft.Json;
using System.Text;

namespace Movie_Rating.Services
{
    public class MoviesUpdaterService(IMoviesRepository moviesRepository,
		ISignInService signInService, IConfiguration configuration,
		IHttpContextAccessor _httpContextAccessor) : IMoviesUpdaterServices
	{
        public async Task<bool> UpdateFilmPosters(CancellationToken cancellationToken)
        {
            string apiUrl = "https://localhost:7291/Movies/UpdateFilmPosters";
            string bearerToken = GetTokenFromCookies();

            using (HttpClient client = new HttpClient())
            {
                // Add the Authorization header with the bearer token
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                try
                {
                    // Make the PUT request
                    HttpResponseMessage response = await client.PutAsync(apiUrl, null, cancellationToken);

                    // Check for successful response
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        // Optionally log the response content if it fails
                        string errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Network error: {ex.Message}");
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Request canceled: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }

                return false;
            }
        }
				
		public async Task<bool> AddUserFilmLike(CancellationToken cancellationToken, int FilmID, string UserID)
		{
			string apiUrl = "https://localhost:7291/Movies/AddUserFilmLike";
			string bearerToken = GetTokenFromCookies();
			var aes = new AES(configuration["AesGcm:Key"]);

			Like like = new Like() 
			{
				FilmID = FilmID,
				UserID = UserID,
			};
			var likeString = JsonConvert.SerializeObject(like);

			var ressult = aes.Encrypt(likeString);

            encrypt encrypted = new encrypt()
            {
                EncryptedToken = Convert.ToBase64String(ressult.CipherText),
                TagBase64 = Convert.ToBase64String(ressult.Tag),
                NonceBase64 = Convert.ToBase64String(ressult.Nonce)
            };

			var content = new StringContent(JsonConvert.SerializeObject(encrypted), Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
				var response = await client.PostAsync(apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					return true;
				}

				return false;
			}
		}


		/// <summary>
		/// Updates the actor's details
		/// </summary>
		/// <param name="actorsUpdateRequest"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>returns truev if updated, else false</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task<bool> UpdateActors(ActorsUpdateRequest actorsUpdateRequest, CancellationToken cancellationToken)
        {
            if (actorsUpdateRequest == null)
                throw new ArgumentNullException(nameof(actorsUpdateRequest));

            //validation
            ValidationHelper.ModelValidation(actorsUpdateRequest);

            //if (actorsUpdateRequest.ActorID == new Guid())
            //    throw new ArgumentException(nameof(personUpdateRequest.PersonID));

            bool isUpdated = await moviesRepository.UpdateActorsDetails(
                actorsUpdateRequest.ActorID,
                actorsUpdateRequest.FirstName,
                actorsUpdateRequest.FamilyName,
                actorsUpdateRequest.DOB,
                actorsUpdateRequest.DOD,
                actorsUpdateRequest.Gender,
                cancellationToken);

            if (isUpdated == false)
            {
                //throw new InvalidPersonIDException("Failed to update the actors details");
                return false;
            }


            return true;
			}


		public string GetTokenFromCookies()
		{
			var Request = _httpContextAccessor.HttpContext.Request;
			return Request.Cookies["jwtToken"];
		}
	}
}
