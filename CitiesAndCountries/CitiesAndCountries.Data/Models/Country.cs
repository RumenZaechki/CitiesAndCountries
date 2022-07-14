namespace CitiesAndCountries.Data.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public string ImageUrl { get; set; }
        public List<City> Cities { get; set; } = new List<City>();
    }
}
