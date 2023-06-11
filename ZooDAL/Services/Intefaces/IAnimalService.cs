using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;

namespace ZooDAL.Services.Intefaces
{
    public interface IAnimalService : IZooService<Animal>
    {
        public Task<IEnumerable<Animal>> GetTopTwoAnimals();

        public Task<IEnumerable<Animal>> GetAnimalsByCategory(string categoryName);
    }
}
