namespace Movie_Rating.Models.DTO
{
    public class PaginatedFilmViewModel
    {
        public IEnumerable<FilmDTO> Films { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages { get; set;}
    }
}
