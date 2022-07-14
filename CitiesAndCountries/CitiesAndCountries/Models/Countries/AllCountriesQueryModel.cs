namespace CitiesAndCountries.Models.Countries
{
    public class AllCountriesQueryModel
    {
        public const int CountriesPerPage = 3;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }
        public List<CountryViewModel> AllCountries { get; set; }
        public int CountryCount { get; set; }
    }
}