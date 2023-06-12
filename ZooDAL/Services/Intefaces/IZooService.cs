using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooDAL.Services.Intefaces
{
    public interface IZooService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        //Task<T> GetByNameAsync(string name);

        Task CreateAsync(T entity);

        Task UpdateAsync(Guid id, T entity);

        Task DeleteAsync(Guid id);
    }
}
