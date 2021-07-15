using System;
using System.Collections.Generic;
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
        public void CreateItem(Item item)
        {
            itemsCollestions.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
           var filter = filterBuilder.Eq(item => item.Id ,id);
           itemsCollestions.DeleteOne(filter);
        }

        public Item GetItem(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id ,id);
            return itemsCollestions.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollestions.Find(new BsonDocument()).ToList();
        }

        public void UpdateItem(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id ,item.Id);
            itemsCollestions.ReplaceOne(filter,item);
        }
    }
}