using CitiesAndCountries.Data;
using CitiesAndCountries.Data.Models;
using CitiesAndCountries.Services.Cities.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesAndCountries.Services.Cities
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext data;
        public CityService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<int> GetCitiesCount()
        {
            return await this.data.Cities.CountAsync();
        }

        public async Task<List<CityServiceModel>> GetCities(int currentPage, int citiesPerPage, string searchTerm)
        {
            if (currentPage <= 0)
            {
                currentPage = 1;
            }
            if (citiesPerPage <= 0)
            {
                citiesPerPage = 3;
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                return await this.data.Cities
                    .Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Skip((currentPage - 1) * citiesPerPage)
                    .Take(citiesPerPage)
                    .Select(c => new CityServiceModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Population = c.Population,
                        ImageUrl = c.ImageUrl
                    })
                    .ToListAsync();
            }
            return await this.data.Cities
                   .Skip((currentPage - 1) * citiesPerPage)
                   .Take(citiesPerPage)
                   .Select(c => new CityServiceModel
                   {
                       Id = c.Id,
                       Name = c.Name,
                       Population = c.Population,
                       ImageUrl = c.ImageUrl
                   })
                   .ToListAsync();
        }

        public async Task<CityServiceModel> GetCity(int id)
        {
            var city = await this.data.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return null;
            }
            return new CityServiceModel
            {
                Id = city.Id,
                Name = city.Name,
                Population = city.Population,
                ImageUrl = city.ImageUrl,
                CountryId = city.Country == null ? -1 : city.Country.Id,//a bit dumb, I know, but I'll implement a better idea if I have the time
                CountryName = city.Country == null ? "None as of yet." : city.Country.Name 
            };
        }

        public async Task AddCity(string name, int population, string imageUrl)
        {
            bool citiesContainName = await IsThereAnyCountryWithThisName(name);
            bool cityAlreadyExists = await this.data.Cities.AnyAsync(c => c.Name == name);
            if (!string.IsNullOrEmpty(name) && population > 0
                && citiesContainName == false && cityAlreadyExists == false)
            {
                await this.data.Cities.AddAsync(new City
                {
                    Name = name,
                    Population = population,
                    ImageUrl = imageUrl
                });
                await this.data.SaveChangesAsync();
            }
        }

        public async Task EditCity(int id, string name, int population, string imageUrl)
        {
            bool citiesContainName = await IsThereAnyCountryWithThisName(name);
            if (!string.IsNullOrEmpty(name) && population > 0 && citiesContainName == false)
            {
                var cityToUpdate = await this.data.Cities.FirstOrDefaultAsync(c => c.Id == id);
                if (cityToUpdate != null)
                {
                    cityToUpdate.Name = name;
                    cityToUpdate.Population = population;
                    cityToUpdate.ImageUrl = imageUrl;
                    await this.data.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteCity(int id)
        {
            var cityToDelete = await this.data.Cities.FirstOrDefaultAsync(c => c.Id == id);
            if (cityToDelete != null)
            {
                this.data.Cities.Remove(cityToDelete);
                await this.data.SaveChangesAsync();
            }
        }

        private async Task<bool> IsThereAnyCountryWithThisName(string name)
        {
            return await this.data.Countries.AnyAsync(c => c.Name == name);
        }
    }
}
