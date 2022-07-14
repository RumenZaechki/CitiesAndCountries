using CitiesAndCountries.Services.Cities.Models;

namespace CitiesAndCountries.Services.Countries.Models
{
    public class CountryServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public string ImageUrl { get; set; }
        public List<CityServiceModel> Cities { get; set; } 
    }
}
