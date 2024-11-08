namespace Movie_Rating.Models.DTO
{
    public class FilmDTO2
    {
        public int FilmID { get; set; }
        public string Title { get; set; }
        public string ReleaseDate { get; set; }
        public int? DirectorID { get; set; }
        public string Director { get; set; }
        public int? StudioID { get; set; }
        public string Studio { get; set; }
        public string? Review { get; set; }
        public int? CountryID { get; set; }
        public string Country { get; set; }
        public int? LanguageID { get; set; }
        public string Language { get; set; }
        public int? GenreID { get; set; }
        public string Genre { get; set; }
        public int? RunTimeMinutes { get; set; }
        public int? CertificateID { get; set; }
        public string Certificate { get; set; }
        public long? BudgetDollars { get; set; }
        public long? BoxOfficeDollars { get; set; }
        public byte? OscarNominations { get; set; }
        public byte? OscarWins { get; set; }
        public string? Poster { get; set; }
        public string? Rating { get; set; }
    }
}
