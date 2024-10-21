namespace Movie_Rating.Models.DTO
{
    public class PaginatedFilmViewModel
    {
        public List<FilmDTO> Films { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
