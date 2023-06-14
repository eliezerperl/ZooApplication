using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;
using ZooDAL.Entities.Interface;
using ZooDAL.Services.Intefaces;

namespace ZooDAL.Services
{
    public class ZooServiceBase<T> : IZooServiceBase<T> where T : class, IEntity
    {
        protected readonly myContext _dbContext;
        readonly DbSet<T> _dbSet;

        public ZooServiceBase(myContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var deletingEntity = await _dbSet.FindAsync(id);
            if (deletingEntity == null)
                throw new ArgumentException($"Entity with ID {id} does not exist.");

            _dbSet.Remove(deletingEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var listOfEntites = await _dbSet.ToListAsync();

            return listOfEntites;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var wantedEntity = await _dbSet.FindAsync(id);
            if (wantedEntity == null)
                throw new ArgumentException($"Entity with ID {id} does not exist.");

            return wantedEntity;
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
                throw new ArgumentException($"Entity with ID {id} does not exist.");

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            _dbContext.SaveChanges();
        }
    }
}
