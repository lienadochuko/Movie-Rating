﻿using System;
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
        public async Task<IEnumerable<FilmDTO>> GetAllFilms(CancellationToken cancellationToken)
		{
			string apiUrl = "http://localhost:5119/Movies/GETFILMS";
            string bearerToken = GetTokenFromCookies();

            using (HttpClient client = new HttpClient())
            {
                // Add the Authorization header with the bearer token
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(apiUrl, cancellationToken);

                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

                // Deserialize JSON data into a list of FilmDTO objects
                IEnumerable<FilmDTO> films = JsonConvert.DeserializeObject<IEnumerable<FilmDTO>>(responseData);

                return films;
            }
        }


        public string GetTokenFromCookies()
        {
            var Request = _httpContextAccessor.HttpContext.Request;
            return Request.Cookies["jwtToken"];
        }
    }
}
