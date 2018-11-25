using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lup.Software.Engineering.Attributes;
using Lup.Software.Engineering.Exceptions;
using Lup.Software.Engineering.Repositories.Interface;

namespace Lup.Software.Engineering.Repositories
{
    public class TableStorageRepository<T> : ITableRepository<T> where T : TableEntity, new()
    {
        private CloudTableClient tableClient;
        private string tableName;

        public TableStorageRepository(IConfiguration configuration)
        {
            var storage = CloudStorageAccount.Parse(configuration["ConnectionStrings:DefaultConnection"]);
            this.tableClient = storage.CreateCloudTableClient();
            var tableNameAttributes = typeof(T).GetCustomAttributes(typeof(TableNameAttribute.TableName), false);
            this.tableName = (tableNameAttributes.Length > 0)
                             ? (tableNameAttributes[0] as TableNameAttribute.TableName).Value
                             : typeof(T).Name.ToLower();
        }

        private async Task<CloudTable> EnsureTableAsync()
        {
            try
            {
                var table = this.tableClient.GetTableReference(this.tableName);
                await table.CreateIfNotExistsAsync().ConfigureAwait(false);
                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            var table = await this.EnsureTableAsync().ConfigureAwait(false);
            var operation = TableOperation.Insert(entity);
            var executeResult = await table.ExecuteAsync(operation).ConfigureAwait(false);
            return executeResult.Result as T;
        }

        public async Task<T> AddOrUpdateAsync(T entity)
        {
            var table = await this.EnsureTableAsync().ConfigureAwait(false);
            var operation = TableOperation.InsertOrReplace(entity);
            var operationContext = new OperationContext
            {
                UserHeaders = new Dictionary<string, string> { { "If-Match", entity.ETag } }
            };

            var executeResult = await table.ExecuteAsync(operation, null, operationContext).ConfigureAwait(false);
            return executeResult.Result as T;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            var table = await this.EnsureTableAsync().ConfigureAwait(false);
            var operation = TableOperation.Delete(entity);
            var executeResult = await table.ExecuteAsync(operation).ConfigureAwait(false);
            return executeResult.Result as T;
        }

        public async Task<IList<T>> QueryAsync(string queryExpression = null)
        {
            var table = await this.EnsureTableAsync().ConfigureAwait(false);
            var query = (!string.IsNullOrEmpty(queryExpression))
                                ? new TableQuery<T>().Where(queryExpression)
                                : new TableQuery<T>();

            TableContinuationToken token = null;
            var result = new List<T>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                result.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return result;

        }

        public async Task<T> Get(string PartitionKey, string RowKey)
        {
            var table = await this.EnsureTableAsync().ConfigureAwait(false);
            var operation = TableOperation.Retrieve<T>(PartitionKey, RowKey);
            var executeResult = await table.ExecuteAsync(operation).ConfigureAwait(false);
            return executeResult.Result as T;
        }

        public async Task<T> Update(T entity)
        {
            var table = await this.EnsureTableAsync().ConfigureAwait(false);
            var existingEntity = await this.Get(entity.PartitionKey, entity.RowKey);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException("not found");
            }
            else
            {
                if (existingEntity.ETag != entity.ETag)
                {
                    throw new DataHadBeenModifiedException("data had been modified");
                }
                var operation = TableOperation.Replace(entity);
                var executeResult = await table.ExecuteAsync(operation).ConfigureAwait(false);
                return executeResult.Result as T;
            }

        }
    }
}
