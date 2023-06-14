using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;

namespace ZooDAL.Services.Intefaces
{
    public interface IAnimalService : IZooServiceBase<Animal>
    {

        Task<Animal> GetAnimalWithCategory(Guid id);

        Task<IEnumerable<Animal>> GetAllAnimalsWithCategories();

        Task<IEnumerable<Animal>> GetTopTwoAnimalsWithCategories();

        Task<IEnumerable<Animal>> GetAnimalsByCategory(Guid categoryId);
    }
}
