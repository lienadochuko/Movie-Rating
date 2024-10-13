using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Add a new country object to the data store
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>Returns the country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);


        /// <summary>
        /// Return all Countries in the data store
        /// </summary>
        /// <returns>Returns all countries from the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Return all countrys based on the given Country ID, otherwise it returns null
        /// </summary>
        /// <param name="countryID">CountryID to search for</param>
        /// <returns>Reutrns matching country or null</returns>
        Task<Country?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Return the country based on the countryName otherwise, it returns null
        /// </summary>
        /// <param name="countryName">CountryName to search to search for</param>
        /// <returns>Returns matching country or Null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);
    }
}
