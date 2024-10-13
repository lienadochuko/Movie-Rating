using System;


namespace Movie_Rating.Models.DTO
{
    /// <summary>
    /// DTO class for adding new country
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
