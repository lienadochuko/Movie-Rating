using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Movie_Rating.Models
{
    public class FilmDTO
    {
		public int FilmID { get; set; }
		public string Title { get; set; }
		public string? Poster { get; set; }
        public string? Rating { get; set; }
        public int LikeCount { get; set; }
    }

}
