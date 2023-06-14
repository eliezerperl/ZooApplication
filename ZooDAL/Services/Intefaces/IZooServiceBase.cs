using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities.Interface;

namespace ZooDAL.Services.Intefaces
{
    public interface IZooServiceBase<T> where T : IEntity, new()
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task CreateAsync(T entity);

        Task UpdateAsync(Guid id, T entity);

        Task DeleteAsync(Guid id);
    }
}
