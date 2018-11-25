using System.Threading.Tasks;
using System.Collections.Generic;

namespace Lup.Software.Engineering.Repositories.Interface
{
    public interface ITableRepository<T> 
        where T : class
    {
        Task<T> Get(string partitionKey, string rowKey);

        Task<IList<T>> QueryAsync(string queryExpression = null);

        Task<T> AddOrUpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);

        Task<T> AddAsync(T entity);

        Task<T> Update(T entity);
    }
}
