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

        public Task<IEnumerable<Animal>> GetAnimalsByCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetCommentsForAnimal(Animal animal)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Animal>> GetTopTwoAnimals()
        {
            throw new NotImplementedException();
        }
    }
}
