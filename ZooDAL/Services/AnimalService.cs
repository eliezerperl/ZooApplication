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

        public async Task<IEnumerable<Animal>> GetAnimalsByCategory(string categoryName)
        {
            var allAnimals = await GetAllAsync();
            var categorizedAnimals = allAnimals.Where(animal => animal.Category.Name == categoryName);
            
            return categorizedAnimals;
        }

        public async Task<IEnumerable<Animal>> GetTopTwoAnimals()
        {
            var allAnimals = await GetAllAsync();
            var topTwoAnimals = allAnimals.OrderByDescending(animal => animal.Comments.Count());

            return topTwoAnimals;
        }
    }
}
