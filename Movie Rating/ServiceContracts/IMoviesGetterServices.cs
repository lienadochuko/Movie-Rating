using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Movie_Rating.Models.DTO;


namespace Movie_Rating.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating actor entity
    /// </summary>
    public interface IMoviesGetterServices
    {


        /// <summary>
        /// Returns all actor
        /// </summary>
        /// <returns>Returns a list of object of ActorResponse type</returns>
        Task<IEnumerable<ActorResponse>> GetAllActors(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the actor object based on the given actor id
        /// </summary>
        /// <param name="ActorID"></param>
        /// <returns>Return matching actor object </returns>
        Task<ActorResponse> GetActorsByID(string actorsID, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the actor as CSV
        /// </summary>
        /// <returns>REturns the memory stream as CSV Data</returns>
        Task<MemoryStream> GetActorCSV(CancellationToken cancellationToken);


        /// <summary>
        /// Returns the actor as Excel
        /// </summary>
        /// <returns>REturns the memory stream as Excel Data of Actors</returns>
        Task<MemoryStream> GetActorExcel(CancellationToken cancellationToken);
    }
}
