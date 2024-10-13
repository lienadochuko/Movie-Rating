using System;
using System.ComponentModel.DataAnnotations;



namespace Movie_Rating.Models.DTO
{
    public class PersonAddRequest
    {
        /// <summary>
        /// Acts as a DTO for inserting new person
        /// </summary>

        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be left blank")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date of birth can't be left blank")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Select your gender please")]
        public GenderOptions? Gender { get; set; }
        public string? NIN { get; set; }

        [Required(ErrorMessage = "Please provide a valid Home/Office address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Select your Country please")]
        public Guid? CountryID { get; set; }

        public bool RecieveNewsLetter { get; set; }
        /// <summary>
        /// converts the current object of PersonAddRequest 
        /// into a new object of Person type
        /// </summary>
        /// <returns></returns>

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DOB = DOB,
                Gender = Gender.ToString(),
                NIN = NIN,
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter,
            };
        }
    }
}
