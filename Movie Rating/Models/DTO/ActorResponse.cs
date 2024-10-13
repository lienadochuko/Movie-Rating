using System;

namespace Movie_Rating.Models.DTO
{
    /// <summary>
    /// Represents a DTO class that is used as
    /// return type of most methods of Person Service
    /// </summary>
    public class ActorResponse
    {
        public string ActorID { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string FullName { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOD { get; set; }
        public string Gender { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the
        /// parameter object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True or false, indicatin whether
        /// all actors details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(ActorResponse)) return false;

            ActorResponse Actors_to_comapare = (ActorResponse)obj;

            return ActorID == Actors_to_comapare.ActorID &&
                FirstName == Actors_to_comapare.FirstName &&
                FamilyName == Actors_to_comapare.FamilyName &&
                FullName == Actors_to_comapare.FullName &&
                DOB == Actors_to_comapare.DOB &&
                DOD == Actors_to_comapare.DOD &&
                Gender == Actors_to_comapare.Gender &&
                Age == Actors_to_comapare.Age;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string? ToString()
        {
            return $"ActorID: {ActorID}, \n" +
                $"FirstName: {FirstName},\n" +
                $"FamilyName: {FamilyName},\n" +
                $"FullName: {FullName}, \n" +
                $"Date of Birth: {DOB}, \n" +
                $"Date of Death: {DOD}, \n" +
                $"Gender: {Gender}, \n" +
                $"Age: {Age}, \n";
        }


        public ActorsUpdateRequest ToActorsUpdateRequest()
        {
            return new ActorsUpdateRequest()
            {
                ActorID = ActorID,
                FirstName = FirstName,
                FamilyName = FamilyName,
                FullName = FullName,
                DOB = DOB,
                DOD = DOD,
                Gender = Gender
            };
        }

    }
    public static class ActorsExtension
    {
        /// <summary>
        /// An extension method to convert an object of Person class into PersonResponse class
        /// </summary>
        /// <param name="actors">the actors object to convert</param>
        /// <returns>Returns the converted PersonResponse object</returns>
        public static ActorResponse ToActorResponse(this ActorsDto actors)
        {
            return new ActorResponse
            {
                ActorID = actors.ActorID.ToString(),
                FirstName = actors.FirstName,
                FamilyName = actors.FamilyName,
                FullName = actors.FullName,
                DOB = actors.DoB,
                DOD = actors.DoD,
                Gender = actors.Gender,
                Age = actors.DoB != null ? Math.Round((DateTime.Now - actors.DoB.Value).TotalDays / 365.25) : null
            };

        }
    }
}
