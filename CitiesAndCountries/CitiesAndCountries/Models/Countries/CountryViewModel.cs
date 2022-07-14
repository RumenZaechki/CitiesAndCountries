using CitiesAndCountries.Models.Cities;

namespace CitiesAndCountries.Models.Countries
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public string ImageUrl { get; set; }
        public List<CityViewModel> Cities { get; set; }
    }
}