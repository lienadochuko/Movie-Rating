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

namespace Movie_Rating.Services
{
    public class MoviesGetterService(IMoviesRepository moviesRepository,
        ISignInService signInService, IConfiguration configuration,
        IHttpContextAccessor _httpContextAccessor) : IMoviesGetterServices
    {
        public async Task<PaginatedFilmViewModel> GetAllFilms(CancellationToken cancellationToken, int pageNumber, int pageSize)
        {
            string apiUrl = $"https://localhost:7291/Movies/GETFILMS?pageNumber={pageNumber}&pageSize={pageSize}";
            string bearerToken = GetTokenFromCookies();

            using (HttpClient client = new HttpClient())
            {
                // Add the Authorization header with the bearer token
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl, cancellationToken);

                    // Check for successful response
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

                        // Deserialize JSON data into PaginatedFilmViewModel
                        PaginatedFilmViewModel paginatedFilmViewModel = JsonConvert.DeserializeObject<PaginatedFilmViewModel>(responseData);

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

        public async Task<PaginatedFilmViewModel> GetFilmsByTitle(CancellationToken cancellationToken, string title, int pageNumber, int pageSize)
        {
            string apiUrl = $"https://localhost:7291/Movies/GETFILMBYTITLE?title={Uri.EscapeDataString(title)}&pageNumber={pageNumber}&pageSize={pageSize}";
            string bearerToken = GetTokenFromCookies();

            using (HttpClient client = new HttpClient())
            {
                // Add the Authorization header with the bearer token
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl, cancellationToken);

                    // Check for successful response
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

                        // Deserialize JSON data into PaginatedFilmViewModel
                        PaginatedFilmViewModel paginatedFilmViewModel = JsonConvert.DeserializeObject<PaginatedFilmViewModel>(responseData);

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

        public string GetTokenFromCookies()
        {
            var Request = _httpContextAccessor.HttpContext.Request;
            return Request.Cookies["jwtToken"];
        }
    }
}
