﻿namespace CitiesAndCountries.Data.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public string ImageUrl { get; set; }
        public Country? Country { get; set; }
    }
}