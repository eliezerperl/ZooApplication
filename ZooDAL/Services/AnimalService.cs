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
    public class AnimalService : ZooServiceBase<Animal>, IAnimalService
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

        public async Task<IEnumerable<Animal>> GetAnimalsByCategory(Guid categoryId)
        {

            var animals = await GetAllAsync();
            var categories = await _dbContext.Categories.ToListAsync();

            var categoryMatch = categories.FirstOrDefault(c => c.Id == categoryId);
            if (categoryMatch == null)
            {
                throw new ArgumentException("The specified category does not exist.");
            }

            var animalsByCategory = animals.Where(x => x.CategoryID == categoryMatch.Id).ToList();

            return animalsByCategory;

        }

        public async Task<IEnumerable<Animal>> GetTopTwoAnimalsWithCategories()
        {
            var allAnimals = await GetAllAnimalsWithCategories();
            var comments = _dbContext.Comments.ToList();

            foreach (var animal in allAnimals)
            {
                animal.Comments = comments.Where(comment => comment.AnimalID == animal.Id);
            }

            var topTwoAnimals = allAnimals
                .OrderByDescending(animal => animal.Comments.Count())
                .Take(2);

            //var categories = _dbContext.Categories.ToList();
            //foreach (var animal in topTwoAnimals)
            //{
            //    animal.Category = categories.First(c => c.Id == animal.CategoryID);
            //}

            return topTwoAnimals;

        }
    }
}
