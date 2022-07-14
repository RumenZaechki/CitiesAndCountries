namespace CitiesAndCountries.Models.Cities
{
    public class AllCitiesQueryModel
    {
        public const int CitiesPerPage = 3;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }
        public List<CityViewModel> AllCities { get; set; }
        public int CityCount { get; set; }
    }
}
