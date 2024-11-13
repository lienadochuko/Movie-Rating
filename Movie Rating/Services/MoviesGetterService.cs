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
using System.Threading;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Models.DTO;
using Movie_Rating.Models;
using Newtonsoft.Json;
using Movie_Rating.RepositoryContracts;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Newtonsoft.Json.Linq;

namespace Movie_Rating.Services
{
    public class MoviesGetterService(IMoviesRepository moviesRepository,
        ISignInService signInService, IConfiguration configuration,
        IHttpContextAccessor _httpContextAccessor) : IMoviesGetterServices
    {
        public async Task<PaginatedFilmViewModel> GetAllFilms(CancellationToken cancellationToken, int pageNumber, int pageSize)
        {
            string apiUrl = $"https://localhost:7291/Movies/GETFILMS?pageNumber={pageNumber}&pageSize={pageSize}";
            string  bearerToken = GetTokenFromCookies();

            using (
                HttpClient client = new HttpClient())
            {
                
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                var aes = new AES(configuration["AesGcm:Key"]);

                var (cipherText, tag, nonce) = aes.Encrypt(bearerToken);

                // Convert encrypted parts to Base64 for safe storage/transmission
                string encryptedToken = Convert.ToBase64String(cipherText);
                string tagBase64 = Convert.ToBase64String(tag);
                string nonceBase64 = Convert.ToBase64String(nonce);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {encryptedToken}");
                client.DefaultRequestHeaders.Add("Tag", tagBase64);
                client.DefaultRequestHeaders.Add("Nonce", nonceBase64);

                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl, cancellationToken);

                    // Check for successful response
                    if (response.IsSuccessStatusCode)
                    {
                         string responseData = await response.Content.ReadAsStringAsync(cancellationToken);


                        // Deserialize and decrypt the token
                        var Data = JsonConvert.DeserializeObject<encrypt>(Convert.ToString(responseData));
                        var decryptedToken = aes.Decrypt(
                            Convert.FromBase64String(Data.EncryptedToken),
                            Convert.FromBase64String(Data.TagBase64),
                            Convert.FromBase64String(Data.NonceBase64)
                        );


                        // Deserialize JSON data into PaginatedFilmViewModel
                        PaginatedFilmViewModel paginatedFilmViewModel = JsonConvert.DeserializeObject<PaginatedFilmViewModel>(decryptedToken);

                        return paginatedFilmViewModel;
                    }
                    else
                    {
                        // Handle non-success status codes (e.g., 404, 401, 403)
                        string errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                        return new PaginatedFilmViewModel
                        {
                            Films = new List<FilmDTO>(), // Assuming FilmDTO represents individual films
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            TotalItems = 0,
                            TotalPages = 0
                        };
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

                // Return an empty PaginatedFilmViewModel in case of exceptions
                return new PaginatedFilmViewModel
                {
                    Films = new List<FilmDTO>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalItems = 0,
                    TotalPages = 0
                };
            }
        }

		public async Task<FilmDTO2> GETFILMBYID(CancellationToken cancellationToken, int id)
		{
			 string apiUrl = $"https://localhost:7291/Movies/GETFILMBYID?id={id}";
			 string bearerToken = GetTokenFromCookies();

			using (HttpClient client = new HttpClient())
			{
                
                var aes = new AES(configuration["AesGcm:Key"]);
                var (cipherText, tag, nonce) = aes.Encrypt(bearerToken);

                // Convert encrypted parts to Base64 for safe storage/transmission
                string encryptedToken = Convert.ToBase64String(cipherText);
                string tagBase64 = Convert.ToBase64String(tag);
                string nonceBase64 = Convert.ToBase64String(nonce);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {encryptedToken}");
                client.DefaultRequestHeaders.Add("Tag", tagBase64);
                client.DefaultRequestHeaders.Add("Nonce", nonceBase64);

                try
				{
					// Make the GET request
					HttpResponseMessage response = await client.GetAsync(apiUrl, cancellationToken);

					// Check for successful response
					if (response.IsSuccessStatusCode)
					{
						string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

                        // Deserialize JSON data into PaginatedFilmViewModel
                        var Data = JsonConvert.DeserializeObject<encrypt>(Convert.ToString(responseData));
                        var decryptedToken = aes.Decrypt(
                            Convert.FromBase64String(Data.EncryptedToken),
                            Convert.FromBase64String(Data.TagBase64),
                            Convert.FromBase64String(Data.NonceBase64)
                        );

                        FilmDTO2 filmDTO = JsonConvert.DeserializeObject<FilmDTO2>(decryptedToken);

						return filmDTO;
					}
					else
					{
						// Handle non-success status codes (e.g., 404, 401, 403)
						string errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                        return new FilmDTO2()
                        {
                            FilmID = 0,
                            Title = "",
                            ReleaseDate = "",
                            DirectorID = 0,
                            Director = "",
                            StudioID = 0,
                            Studio = "",
                            Review = "",
                            CountryID = 0,
                            Country = "",
                            LanguageID = 0,
                            Language = "",
                            GenreID = 0,
                            Genre = "",
                            RunTimeMinutes = 0,
                            CertificateID = 0,
                            Certificate = "",
                            BudgetDollars = 0,
                            BoxOfficeDollars = 0,
                            OscarNominations = 0,
                            OscarWins = 0,
                            Poster = "",
                            Rating = "",
                        };
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

                // Return an empty PaginatedFilmViewModel in case of exceptions
                return new FilmDTO2()
				{
					FilmID = 0,
					Title = "",
					ReleaseDate = "",
					DirectorID = 0,
					Director = "",
					StudioID = 0,
					Studio = "",
					Review = "",
					CountryID = 0,
					Country = "",
					LanguageID = 0,
					Language = "",
					GenreID = 0,
					Genre = "",
					RunTimeMinutes = 0,
					CertificateID = 0,
					Certificate = "",
					BudgetDollars = 0,
					BoxOfficeDollars = 0,
					OscarNominations = 0,
					OscarWins = 0,
					Poster = "",
					Rating = "",
				};
			}
		}

		public async Task<PaginatedFilmViewModel> GetFilmsByTitle(CancellationToken cancellationToken, string  title, int pageNumber, int pageSize)
        {
            string apiUrl = $"https://localhost:7291/Movies/GETFILMBYTITLE?title={Uri.EscapeDataString(title)}&pageNumber={pageNumber}&pageSize={pageSize}";
            string bearerToken = GetTokenFromCookies();

            using (HttpClient client = new HttpClient())
            {                
                var aes = new AES(configuration["AesGcm:Key"]);
                var (cipherText, tag, nonce) = aes.Encrypt(bearerToken);

                // Convert encrypted parts to Base64 for safe storage/transmission
                string encryptedToken = Convert.ToBase64String(cipherText);
                string tagBase64 = Convert.ToBase64String(tag);
                string nonceBase64 = Convert.ToBase64String(nonce);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {encryptedToken}");
                client.DefaultRequestHeaders.Add("Tag", tagBase64);
                client.DefaultRequestHeaders.Add("Nonce", nonceBase64);

                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl, cancellationToken);

                    // Check for successful response
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

                        var Data = JsonConvert.DeserializeObject<encrypt>(Convert.ToString(responseData));
                        var decryptedToken = aes.Decrypt(
                            Convert.FromBase64String(Data.EncryptedToken),
                            Convert.FromBase64String(Data.TagBase64),
                            Convert.FromBase64String(Data.NonceBase64)
                        );
                        // Deserialize JSON data into PaginatedFilmViewModel
                        PaginatedFilmViewModel paginatedFilmViewModel = JsonConvert.DeserializeObject<PaginatedFilmViewModel>(decryptedToken);

                        return paginatedFilmViewModel;
                    }
                    else
                    {
                        // Handle non-success status codes (e.g., 404, 401, 403)
                        // Log or throw an exception based on your needs
                        string errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                        return new PaginatedFilmViewModel
                        {
                            Films = new List<FilmDTO>(), // Assuming FilmDTO represents individual films
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            TotalItems = 0,
                            TotalPages = 0
                        };
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

                // Return an empty PaginatedFilmViewModel in case of exceptions
                return new PaginatedFilmViewModel
                {
                    Films = new List<FilmDTO>(),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalItems = 0,
                    TotalPages = 0
                };
            }
        }

        public string  GetTokenFromCookies()
        {
            var Request = _httpContextAccessor.HttpContext.Request;
            return Request.Cookies["jwtToken"];
        }
    }
}
