using MongoDB.Driver;
using Sample.Common.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sample.Common.Service.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);
        }
        // using read-only so that consumer should not need to modify.
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> prdicate)
        {
            return await dbCollection.Find(prdicate).ToListAsync();
        }
        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> prdicate)
        {
            return await dbCollection.Find(prdicate).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(T entity)
        {
            await dbCollection.InsertOneAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(x => x.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        public async Task DeleteAsync(Guid guid)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, guid);
            await dbCollection.DeleteOneAsync(filter);
        }

    }
}
