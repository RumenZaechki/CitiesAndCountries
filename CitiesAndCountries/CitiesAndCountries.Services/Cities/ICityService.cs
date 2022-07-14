using CitiesAndCountries.Services.Cities.Models;

namespace CitiesAndCountries.Services.Cities
{
    public interface ICityService
    {
        public Task<int> GetCitiesCount();
        public Task<List<CityServiceModel>> GetCities(int currentPage, int countriesPerPage, string searchTerm);
        public Task<CityServiceModel> GetCity(int id);
        public Task AddCity(string name, int population, string imageUrl);
        public Task EditCity(int id, string name, int population, string imageUrl);
        public Task DeleteCity(int id);
    }
}
