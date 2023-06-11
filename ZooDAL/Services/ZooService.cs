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
    public class ZooService<T> : IZooService<T> where T : class
    {
        readonly myContext _dbContext;

        public ZooService(myContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            await dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var dbSet = _dbContext.Set<T>();

            var existingEntity = await dbSet.FindAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"Entity with ID {id} does not exist.");
            }
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
