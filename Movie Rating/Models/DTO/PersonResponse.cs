using System;

namespace Movie_Rating.Models.DTO
{
    /// <summary>
    /// Represents a DTO class that is used as
    /// return type of most methods of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? NIN { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetter { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the
        /// parameter object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True or false, indicatin whether
        /// all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person_to_comapare = (PersonResponse)obj;

            return PersonID == person_to_comapare.PersonID &&
                PersonName == person_to_comapare.PersonName &&
                Email == person_to_comapare.Email &&
                DOB == person_to_comapare.DOB &&
                Gender == person_to_comapare.Gender &&
                NIN == person_to_comapare.NIN &&
                CountryID == person_to_comapare.CountryID &&
                Country == person_to_comapare.Country &&
                Address == person_to_comapare.Address &&
                RecieveNewsLetter == person_to_comapare.RecieveNewsLetter &&
                Age == person_to_comapare.Age;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string? ToString()
        {
            return $"PersonID: {PersonID}, \n" +
                $"Name: {PersonName},\n" +
                $"Email: {Email}, \n" +
                $"Date of Birth: {DOB}, \n" +
                $"Gender: {Gender}, \n" +
                $"NIN: {NIN}, \n" +
                $"Age: {Age}, \n" +
                $"Address: {Address}, \n" +
                $"CountryID: {CountryID}, \n" +
                $"Country: {Country}, \n" +
                $"RecieveNewsLetter: {RecieveNewsLetter} \n";
        }


        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DOB = DOB,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                NIN = NIN,
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter,
            };
        }

    }
    public static class PersonExtension
    {
        /// <summary>
        /// An extension method to convert an object of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">the person object to convert</param>
        /// <returns>Returns the converted PersonResponse object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DOB = person.DOB,
                Gender = person.Gender,
                NIN = person.NIN,
                Address = person.Address,
                CountryID = person.CountryID,
                Age = person.DOB != null ? Math.Round((DateTime.Now - person.DOB.Value).TotalDays / 365.25) : null,
                RecieveNewsLetter = person.RecieveNewsLetter,
                Country = person.country?.CountryName
            };

        }
    }
}
