using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie_Rating.Models.DTO;

namespace Movie_Rating.Repositories.DataAccess
{
	public interface IDataRepository
	{
		Task<IEnumerable<T>> GetListOfActors<T>(string procedureName, string ConnectionString, SqlParameter[] sqlParameter, Func<SqlDataReader, T> mapFunction, CancellationToken cancellationToken = default);
        Task<ActorsDto> GetSearchedActors(string id, string procedureName, string connectionString,CancellationToken cancellationToken = default);
        Task<bool> UpdateActorFieldsAsync(string actorId, string firstName, string familyName, DateTime? dob, DateTime? dod, string gender,
          string procedureName, string connectionString, CancellationToken cancellationToken = default);
        Task<bool> DeleteActorAsync(string actorId, string procedureName, string connectionString, CancellationToken cancellationToken = default);

    }

    public class DataRepository : IDataRepository
    {
        public async Task<IEnumerable<T>> GetListOfActors<T>(string procedureName, string connectionString, SqlParameter[] sqlParameter, Func<SqlDataReader, T> mapFunction, CancellationToken cancellationToken = default)
        {
            await using SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new()
            {
                CommandText = procedureName,
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 300
            };

            if (sqlParameter != null)
            {
                command.Parameters.AddRange(sqlParameter);
            }

            await connection.OpenAsync(cancellationToken);

            List<T> result = new List<T>();

            await using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    T record = mapFunction(reader);
                    result.Add(record);
                }
            }

            return result;
        }
        
        public async Task<ActorsDto> GetSearchedActors(string id,string procedureName, string connectionString, CancellationToken cancellationToken = default)
        {
            ActorsDto ActorDto = null;
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding the parameter to the command
                    command.Parameters.Add(new SqlParameter("@VariableId", SqlDbType.VarChar, 10) { Value = id });

                    await connection.OpenAsync();

                    // Executing the command and reading the result
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                             ActorDto = new ActorsDto
                            {
                                ActorID = reader.GetInt32(reader.GetOrdinal("ActorID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                FamilyName = reader.GetString(reader.GetOrdinal("FamilyName")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                DoB = reader.GetDateTime(reader.GetOrdinal("DoB")),
                                DoD = reader.IsDBNull(reader.GetOrdinal("DoD")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DoD")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender"))
                            };
                        }
                    }
                }
            }

            return ActorDto;
        }

        public async Task<bool> UpdateActorFieldsAsync(string actorId, string firstName, string familyName, DateTime? dob, DateTime? dod, string gender,
            string procedureName, string connectionString, CancellationToken cancellationToken = default)
        {
            bool isUpdated = false;

            // Using ADO.NET connection and command
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command
                    command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 100) { Value = firstName });
                    command.Parameters.Add(new SqlParameter("@FamilyName", SqlDbType.VarChar, 100) { Value = familyName });
                    command.Parameters.Add(new SqlParameter("@DoB", SqlDbType.DateTime) { Value = dob });

                    if (dod.HasValue)
                    {
                        command.Parameters.Add(new SqlParameter("@DoD", SqlDbType.DateTime) { Value = dod });
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@DoD", SqlDbType.DateTime) { Value = DBNull.Value });
                    }

                    command.Parameters.Add(new SqlParameter("@Gender", SqlDbType.VarChar, 50) { Value = gender });
                    command.Parameters.Add(new SqlParameter("@ActorId", SqlDbType.VarChar, 10) { Value = actorId });

                    await connection.OpenAsync();

                    // Execute the update command
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    isUpdated = rowsAffected > 0;
                }
            }

            return isUpdated;
        }

        public async Task<bool> DeleteActorAsync(string actorId, string procedureName, string connectionString, CancellationToken cancellationToken = default)
        {
            bool isUpdated = false;

            // Using ADO.NET connection and command
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ActorId", SqlDbType.VarChar, 10) { Value = actorId });

                    await connection.OpenAsync();

                    // Execute the update command
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    isUpdated = rowsAffected > 0;
                }
            }

            return isUpdated;
        }
    }
}
