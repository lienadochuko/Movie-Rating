using ContactsManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represent data access logic for managing Person entity
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Adds the person object to the data store
        /// </summary>
        /// <param name="person">Person object to be added</param>
        /// <returns>Returns the person object after adding</returns>
        Task<Person> AddPerson(Person person);


        /// <summary>
        /// Returns all Persons in the data store
        /// </summary>
        /// <returns>List of all person object from table</returns>
        Task<List<Person>> GetAllPerson();

        /// <summary>
        /// Return Person object from the data store based on the PersonID
        /// </summary>
        /// <param name="personID">PersonId to search for</param>
        /// <returns>Returns matching Person Object or null</returns>
        Task<Person> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns Person object from the data store base on the PersonName
        /// </summary>
        /// <param name="personName">PersonName to search for</param>
        /// <returns>Returns matching Person Object or null</returns>
        Task<Person> GetPersonByPersonName(string? personName);

        /// <summary>
        /// Returns all person object based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Delete a person object based on the person ID
        /// </summary>
        /// <param name="personID">Person ID to search for</param>
        /// <returns>Returns true , if the deletion was successful otherwise, false</returns>
        Task<bool> DeletePersonByPersonID(Guid personID);

        /// <summary>
        /// Updates a person object based on the given person id
        /// </summary>
        /// <param name="person">Person object to update</param>
        /// <returns>Returns the updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
