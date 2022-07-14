using CitiesAndCountries.Services.Countries.Models;

namespace CitiesAndCountries.Services.Countries
{
    public interface ICountryService
    {
        public Task<int> GetCountriesCount();
        public Task<List<CountryServiceModel>> GetCountries(int currentPage, int countriesPerPage, string searchTerm);
        public Task<CountryServiceModel> GetCountry(int id);
        public Task AddCountry(string name, int population, string imageUrl);
        public Task EditCountry(int id, string name, int population, string imageUrl);
        public Task AssignCityToCountry(int countryId, string cityName);
        public Task DeleteCountry(int id);
    }
}
