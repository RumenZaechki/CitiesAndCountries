using CitiesAndCountries.Data;
using CitiesAndCountries.Data.Models;
using CitiesAndCountries.Services.Cities.Models;
using CitiesAndCountries.Services.Countries.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesAndCountries.Services.Countries
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext data;
        public CountryService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<int> GetCountriesCount()
        {
            return await this.data.Countries.CountAsync();
        }

        public async Task<List<CountryServiceModel>> GetCountries(int currentPage, int countriesPerPage, string searchTerm)
        {
            if (currentPage <= 0)
            {
                currentPage = 1;
            }
            if (countriesPerPage <= 0)
            {
                countriesPerPage = 3;
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                return await this.data.Countries
                    .Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Skip((currentPage - 1) * countriesPerPage)
                    .Take(countriesPerPage)
                    .Select(c => new CountryServiceModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Population = c.Population,
                        ImageUrl = c.ImageUrl
                    })
                    .ToListAsync();
            }
            return await this.data.Countries
                   .Skip((currentPage - 1) * countriesPerPage)
                   .Take(countriesPerPage)
                   .Select(c => new CountryServiceModel
                   {
                       Id = c.Id,
                       Name = c.Name,
                       Population = c.Population,
                       ImageUrl = c.ImageUrl
                   })
                   .ToListAsync();
        }

        public async Task<CountryServiceModel> GetCountry(int id)
        {
            var country = await this.data.Countries
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return null;
            }
            return new CountryServiceModel
            {
                Id = country.Id,
                Name = country.Name,
                Population = country.Population,
                ImageUrl = country.ImageUrl,
                Cities = country.Cities
                    .Select(c => new CityServiceModel
                    {
                        Id = c.Id,
                        Population = c.Population,
                        Name = c.Name,
                        CountryId = country.Id
                    })
                    .ToList()
            };
        }

        public async Task AddCountry(string name, int population, string imageUrl)
        {
            bool citiesContainName = await IsThereAnyCityWithThisName(name);
            if (!string.IsNullOrEmpty(name) && population > 0 && citiesContainName == false)
            {
                await this.data.Countries.AddAsync(new Country
                {
                    Name = name,
                    Population = population,
                    ImageUrl = imageUrl
                });
                await this.data.SaveChangesAsync();
            }
        }

        public async Task EditCountry(int id, string name, int population, string imageUrl)
        {
            bool citiesContainName = await IsThereAnyCityWithThisName(name);
            if (!string.IsNullOrEmpty(name) && population > 0 && citiesContainName == false)
            {
                var countryToUpdate = await this.data.Countries.FirstOrDefaultAsync(c => c.Id == id);
                if (countryToUpdate != null)
                {
                    countryToUpdate.Name = name;
                    countryToUpdate.Population = population;
                    countryToUpdate.ImageUrl = imageUrl;
                    await this.data.SaveChangesAsync();
                }
            }
        }

        public async Task AssignCityToCountry(int countryId, string cityName)
        {
            var country = await this.data.Countries.FirstOrDefaultAsync(c => c.Id == countryId);
            var city = await this.data.Cities.FirstOrDefaultAsync(c => c.Name == cityName);

            if (city != null && country != null)
            {
                city.Country = country;
                await this.data.SaveChangesAsync();
            }
        }

        public async Task DeleteCountry(int id)
        {
            var countryToDelete = await this.data.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (countryToDelete != null)
            {
                this.data.Countries.Remove(countryToDelete);
                await this.data.SaveChangesAsync();
            }
        }

        private async Task<bool> IsThereAnyCityWithThisName(string name)
        {
            return await this.data.Cities.AnyAsync(c => c.Name == name);
        }
    }
}
