using CitiesAndCountries.Models.Cities;
using CitiesAndCountries.Models.Countries;
using CitiesAndCountries.Services.Countries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitiesAndCountries.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ICountryService countryService;
        public CountriesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        public async Task<IActionResult> AllCountries([FromQuery] AllCountriesQueryModel query)
        {
            var countries = await this.countryService.GetCountries(query.CurrentPage, AllCountriesQueryModel.CountriesPerPage, query.SearchTerm);
            var models = countries
                .Select(c => new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Population = c.Population
                })
                .ToList();
            return View(new AllCountriesQueryModel
            {
                CurrentPage = query.CurrentPage,
                SearchTerm = query.SearchTerm,
                AllCountries = models,
                CountryCount = await this.countryService.GetCountriesCount()
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var country = await this.countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }
            var model = new CountryViewModel
            {
                Id = country.Id,
                Name = country.Name,
                Population = country.Population,
                Cities = country.Cities == null 
                    ? new List<CityViewModel>() 
                    : country.Cities.Select(cc => new CityViewModel
                {
                    Id = cc.Id,
                    Name = cc.Name,
                    Population = cc.Population,
                    CountryId = country.Id,
                    CountryName = country.Name
                }).ToList()
            };
            return View(model);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CountryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.countryService.AddCountry(model.Name, model.Population, model.ImageUrl);
            return RedirectToAction("AllCountries", "Countries");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var country = await this.countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(new CountryFormModel
            {
                Name = country.Name,
                Population = country.Population,
                ImageUrl = country.ImageUrl,
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CountryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.countryService.EditCountry(id, model.Name, model.Population, model.ImageUrl);
            return RedirectToAction("AllCountries", "Countries");
        }

        [Authorize]
        public async Task<IActionResult> AssignCity(int id)
        {
            var country = await this.countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AssignCity(int id, AssignCityFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.countryService.AssignCityToCountry(id, model.Name);
            return RedirectToAction("AllCountries", "Countries");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await this.countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }
            await this.countryService.DeleteCountry(id);
            return RedirectToAction("AllCountries", "Countries");
        }
    }
}
