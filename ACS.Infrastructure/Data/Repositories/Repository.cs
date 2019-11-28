using ACS.Core.Entities.Base;
using ACS.Core.Interfaces.Filters;
using ACS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T>
        where T:BaseEntity
    {
        private readonly ACSDbContext _context;

        public Repository(ACSDbContext context)
        {
            _context = context;
        }

        #region Select

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> ListByQueryAsync(IQueryFilter<T> filter)
        {
            return await FlattenQueryFilter(filter).ToListAsync();
        }

        public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultByQueryAsync(IQueryFilter<T> filter)
        {
            return await FlattenQueryFilter(filter).FirstOrDefaultAsync();
        }

        #endregion

        #region Insert

        public async Task<T> InsertAsync(T entity)
        {
            _context.Add<T>(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        #endregion

        #region Update

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Attach<T>(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _context.Find<T>(id);

            await DeleteAsync(entity);
        }

        #endregion

        #region Private

        private IQueryable<T> FlattenQueryFilter(IQueryFilter<T> filter)
        {
            return filter.Includes.Aggregate(_context.Set<T>().AsQueryable(), (acc, x) => acc.Include(x)).Where(filter.Predicate);
        }

        #endregion
    }
}