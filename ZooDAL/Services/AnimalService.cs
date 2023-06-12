using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;
using ZooDAL.Services.Intefaces;

namespace ZooDAL.Services
{
    public class AnimalService : ZooService<Animal>, IAnimalService
    {

        public AnimalService(myContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsWithCategories()
        {
            var animals = await GetAllAsync();
            var categories = await _dbContext.Categories.ToListAsync();

            foreach (var animal in animals)
            {
                animal.Category = categories.First(c => c.Id == animal.CategoryID);
            }

            return animals;
        }

        public async Task<Animal> GetAnimalWithCategory(Guid id)
        {
            var animal = await GetByIdAsync(id);
            var categories = await _dbContext.Categories.ToListAsync();

            animal.Category = categories.First(x => animal.CategoryID == x.Id);

            return animal;
        }

        public async Task<IEnumerable<Animal>> GetAnimalsByCategory(string categoryName)
        {
            //var allAnimals = await GetAllAsync();
            //var categorizedAnimals = allAnimals.Where(animal => animal.Category.Name == categoryName);
            //return categorizedAnimals;

            var animals = await GetAllAsync();
            var categories = await _dbContext.Categories.ToListAsync();

            var categoryMatch = categories.FirstOrDefault(c => c.Name == categoryName);
            if (categoryMatch == null)
            {
                throw new ArgumentException("The specified category does not exist.");
            }

            var animalsByCategory = animals.Where(x => x.CategoryID == categoryMatch.Id).ToList();

            if (animalsByCategory.Count == 0)
            {
                throw new ArgumentException("There are no animals in the specified category.");
            }

            return animalsByCategory;

        }

        public async Task<IEnumerable<Animal>> GetTopTwoAnimals()
        {
            var allAnimals = await GetAllAsync();
            var topTwoAnimals = allAnimals.OrderByDescending(animal => animal.Comments.Count()).Take(2);

            return topTwoAnimals;
        }
    }
}
