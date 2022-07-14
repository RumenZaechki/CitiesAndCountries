using CitiesAndCountries.Models.Cities;
using CitiesAndCountries.Services.Cities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitiesAndCountries.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ICityService cityService;
        public CitiesController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        public async Task<IActionResult> AllCities([FromQuery] AllCitiesQueryModel query)
        {
            var cities = await this.cityService.GetCities(query.CurrentPage, AllCitiesQueryModel.CitiesPerPage, query.SearchTerm);
            var models = cities
                .Select(c => new CityViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Population = c.Population
                }).ToList();
            return View(new AllCitiesQueryModel
            {
                CurrentPage = query.CurrentPage,
                SearchTerm = query.SearchTerm,
                AllCities = models,
                CityCount = await this.cityService.GetCitiesCount()
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var city = await this.cityService.GetCity(id);
            if (city == null)
            {
                return NotFound();
            }
            var model = new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                Population = city.Population,
                CountryId = city.CountryId,
                CountryName = city.CountryName
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
        public async Task<IActionResult> Add(CityFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.cityService.AddCity(model.Name, model.Population, model.ImageUrl);
            return RedirectToAction("AllCities", "Cities");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var city = await this.cityService.GetCity(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(new CityFormModel
            {
                Name = city.Name,
                Population = city.Population,
                ImageUrl = city.ImageUrl
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CityFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.cityService.EditCity(id, model.Name, model.Population, model.ImageUrl);
            return RedirectToAction("AllCities", "Cities");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var city = await this.cityService.GetCity(id);
            if (city == null)
            {
                return NotFound();
            }
            await this.cityService.DeleteCity(id);
            return RedirectToAction("AllCities", "Cities");
        }
    }
}
