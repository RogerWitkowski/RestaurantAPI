using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Models;

namespace Restaurant.DataAccess.Seeder
{
    public class DataGenerator
    {
        private readonly RestaurantDbContext _dbContext;

        public DataGenerator(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedDb()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Restaurants.Any())
                {
                    GetRestaurants();
                }
            }
        }

        private IEnumerable<Models.Models.Restaurant> GetRestaurants()
        {
            var addressGenerator = new Faker<Address>()
                .RuleFor(c => c.City, fc => fc.Address.City())
                .RuleFor(s => s.Street, fs => fs.Address.StreetName())
                .RuleFor(p => p.PostalCode, fp => fp.Address.ZipCode());

            var dishGenerator = new Faker<Dish>()
                .RuleFor(n => n.Name, fn => fn.Name.FirstName())
                .RuleFor(d => d.Description, fd => fd.Commerce.ProductDescription())
                .RuleFor(p => p.Price, fp => fp.Random.Decimal(1, 100));

            var restaurantGenerator = new Faker<Models.Models.Restaurant>()
                .RuleFor(n => n.Name, fn => fn.Company.CompanyName())
                .RuleFor(d => d.Description, fd => fd.Lorem.Sentence(10))
                .RuleFor(c => c.Category, fc => fc.Lorem.Word())
                .RuleFor(hd => hd.HasDelivery, fhd => fhd.Random.Bool())
                .RuleFor(e => e.ContactEmail, fe => fe.Person.Email)
                .RuleFor(n => n.ContactNumber, fn => fn.Phone.PhoneNumber())
                .RuleFor(a => a.Address, fa => addressGenerator.Generate())
                .RuleFor(dishes => dishes.Dishes, fdishes => dishGenerator.Generate(50));

            var restaurant = restaurantGenerator.Generate(100);
            _dbContext.Restaurants.AddRange(restaurant);
            _dbContext.SaveChanges();

            return restaurant;
        }
    }
}