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
    public class ZooServiceBase<T> : IZooServiceBase<T> where T : class
    {
        protected readonly myContext _dbContext;

        public ZooServiceBase(myContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            await dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var dbSet = _dbContext.Set<T>();
            var deletingEntity = await dbSet.FindAsync(id);
            if (deletingEntity == null)
                throw new ArgumentException($"Entity with ID {id} does not exist.");

            dbSet.Remove(deletingEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var dbSet = _dbContext.Set<T>();
            var listOfEntites = await dbSet.ToListAsync();

            return listOfEntites;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var dbSet = _dbContext.Set<T>();
            var wantedEntity = await dbSet.FindAsync(id);
            if (wantedEntity == null)
                throw new ArgumentException($"Entity with ID {id} does not exist.");

            return wantedEntity;
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var dbSet = _dbContext.Set<T>();

            var existingEntity = await dbSet.FindAsync(id);
            if (existingEntity == null)
                throw new ArgumentException($"Entity with ID {id} does not exist.");
            /*
             add newEntityId = oldEntityId
            entity.Id = id;
             */
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            _dbContext.SaveChanges();
        }
    }
}
