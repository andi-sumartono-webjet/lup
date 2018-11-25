namespace Lup.Software.Engineering.Repositories.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITableRepository<T> 
        where T : class
    {
        Task<T> Get(string partitionKey, string rowKey);

        Task<IList<T>> QueryAsync(string queryExpression = null);

        Task<T> AddOrUpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);
    }
}
