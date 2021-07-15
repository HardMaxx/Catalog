using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string databaseName ="catalog";
        private const string collectionName = "items";

        private readonly IMongoCollection<Item> itemsCollestions;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollestions = database.GetCollection<Item>(collectionName);
        }
        public async Task CreateItemAsync(Item item)
        {
            await itemsCollestions.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
           var filter = filterBuilder.Eq(item => item.Id ,id);
           await itemsCollestions.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id ,id);
            return await  itemsCollestions.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollestions.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id ,item.Id);
            await itemsCollestions.ReplaceOneAsync(filter,item);
        }
    }
}