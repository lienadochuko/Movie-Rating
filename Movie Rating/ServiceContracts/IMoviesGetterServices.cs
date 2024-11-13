using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Movie_Rating.Models.DTO;
using Movie_Rating.Models;


namespace Movie_Rating.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating actor entity
    /// </summary>
    public interface IMoviesGetterServices
    {


        /// <summary>
        /// Returns all Films
        /// </summary>
        /// <returns>Returns a list of object of FilmDTO type</returns>
        Task<PaginatedFilmViewModel> GetAllFilms(CancellationToken cancellationToken, int pageNumber, int pageSize);

        /// <summary>
        /// Returns a list of movies based on searched title
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="title"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Returns a list of movies based on searched title</returns>
        Task<PaginatedFilmViewModel> GetFilmsByTitle(CancellationToken cancellationToken, string title, int pageNumber, int pageSize);

		/// <summary>
		/// Returns the FilmDTO object based on the given Film id
		/// </summary>
		/// <param name="GETFILMBYID"></param>
		/// <returns>Return matching FilmDTO object </returns>
		Task<FilmDTO2> GETFILMBYID(CancellationToken cancellationToken, int id);


        ///// <summary>
        ///// Returns the actor object based on the given actor id
        ///// </summary>
        ///// <param name="ActorID"></param>
        ///// <returns>Return matching actor object </returns>
        //Task<ActorResponse> GetActorsByID(string actorsID, CancellationToken cancellationToken);

        ///// <summary>
        ///// Returns the actor as CSV
        ///// </summary>
        ///// <returns>REturns the memory stream as CSV Data</returns>
        //Task<MemoryStream> GetActorCSV(CancellationToken cancellationToken);


        ///// <summary>
        ///// Returns the actor as Excel
        ///// </summary>
        ///// <returns>REturns the memory stream as Excel Data of Actors</returns>
        //Task<MemoryStream> GetActorExcel(CancellationToken cancellationToken);
    }
}
