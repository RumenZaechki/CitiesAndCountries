namespace CitiesAndCountries.Services.Cities.Models
{
    public class CityServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public string ImageUrl { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
