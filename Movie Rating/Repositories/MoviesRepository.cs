using Movie_Rating.Helpers;
using Microsoft.Extensions.Configuration;
using Movie_Rating.Models.DTO;
using Movie_Rating.Repositories.DataAccess;
using Movie_Rating.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Rating.Repositories
{
    public class MoviesRepository(IDataRepository dataRepository, IConfiguration configuration) : IMoviesRepository
    {
        public async Task<IEnumerable<ActorsDto>> GetAllActors(CancellationToken cancellationToken)
        {
            return await dataRepository.GetListOfActors("dbo.GetAllActors", CustomHelpers.GetConnectionString(configuration, "SecondConnection"), null, reader =>
            {
                return new ActorsDto
                {
                    ActorID = CustomHelpers.GetSafeInt32(reader, 0),
                    FirstName = CustomHelpers.GetSafeString(reader, 1),
                    FamilyName = CustomHelpers.GetSafeString(reader, 2),
                    FullName = CustomHelpers.GetSafeString(reader, 3),
                    DoB = CustomHelpers.GetDateTime(reader, 4),
                    DoD = CustomHelpers.GetDateTime(reader, 5),
                    Gender = CustomHelpers.GetSafeString(reader, 6)
                };
            }, cancellationToken);
        }
        public async Task<ActorsDto> GetActors(string id, CancellationToken cancellationToken)
        {
            return await dataRepository.GetSearchedActors(id, "dbo.GetSearchedActor", CustomHelpers.GetConnectionString(configuration, "SecondConnection"), cancellationToken);
        }

        public async Task<bool> UpdateActorsDetails(string id, string FirstName, string FamilyName, DateTime? DoB, DateTime? DoD, string Gender, CancellationToken cancellationToken)
        {
            return await dataRepository.UpdateActorFieldsAsync(id, FirstName, FamilyName, DoB, DoD, Gender, "dbo.UPDATEFIELDS",
                CustomHelpers.GetConnectionString(configuration, "SecondConnection"), cancellationToken);
        }

        public async Task<bool> DeleteActor(string id, CancellationToken cancellationToken)
        {
            return await dataRepository.DeleteActorAsync(id, "dbo.DELETEFIELD",
                CustomHelpers.GetConnectionString(configuration, "SecondConnection"), cancellationToken);
        }
    }

}
