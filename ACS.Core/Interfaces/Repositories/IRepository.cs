using ACS.Core.Entities.Base;
using ACS.Core.Interfaces.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        #region Select

        Task<T?> GetByIdAsync(int id);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> ListByQueryAsync(IQueryFilter<T> filter);

        Task<T?> FirstOrDefaultByQueryAsync(IQueryFilter<T> filter);
        Task<T?> FirstOrDefault(Expression<Func<T, bool>> predicate);

        #endregion

        #region Insert
        
        Task<T> InsertAsync(T entity);

        #endregion

        #region Update

        Task<T> UpdateAsync(T entity);

        #endregion

        #region Delete

        Task DeleteAsync(T entity);

        Task DeleteAsync(int id);

        #endregion
    }
}
