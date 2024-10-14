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

namespace Movie_Rating.Services
{
    public class MoviesGetterService(IMoviesRepository moviesRepository) : IMoviesGetterServices
    {
		public async Task<IEnumerable<FilmDTO>> GetAllFilms(CancellationToken cancellationToken)
		{
			string apiUrl = "https://localhost:7291/api/Movies/GETFILMS";
			//string apiUrl = "https://localhost:7291/api/Films/GetFilms";
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(apiUrl);

				response.EnsureSuccessStatusCode();

				string responseData = await response.Content.ReadAsStringAsync(cancellationToken);

				// Deserialize JSON data into a list of Film objects
				IEnumerable<FilmDTO> film = JsonConvert.DeserializeObject<IEnumerable<FilmDTO>>(responseData);

				return film;
			}
		}
    }
}
