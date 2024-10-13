using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Movie_Rating.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating actor entity
    /// </summary>
    public interface IMoviesDeleterServices
    {
        Task<bool> DeleteActors(string actorID, CancellationToken cancellationToken);
    }
}
