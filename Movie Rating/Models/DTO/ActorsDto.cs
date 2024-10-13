using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Rating.Models.DTO
{
    public class ActorsDto
    {
        public int ActorID { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string FullName { get; set; }
        public DateTime? DoB { get; set; }
        public DateTime? DoD { get; set; }
        public string Gender { get; set; }
    }
}
