

using System;
using System.ComponentModel.DataAnnotations;

namespace Movie_Rating.Models.DTO
{
    public class PersonUpdateRequest
    {
        /// <summary>
        /// Acts as a DTO for updating person
        /// </summary>

        [Required(ErrorMessage = "Person ID can't be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be left blank")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date of birth can't be left blank")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Select your gender please")]
        public GenderOptions? Gender { get; set; }
        public string? NIN { get; set; }

        [Required(ErrorMessage = "Please provide a valid Home/Office address")]
        public string? Address { get; set; }
        public Guid? CountryID { get; set; }
        public bool RecieveNewsLetter { get; set; }

        /// <summary>
        /// converts the current object of PersonUpdateRequest 
        /// into a new object of Person type
        /// </summary>
        /// <returns>Returns Person object</returns>

        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DOB = DOB,
                Gender = Gender.ToString(),
                Address = Address,
                NIN = NIN,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter
            };
        }
    }
}
