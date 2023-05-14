using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DND_DC_Music_Bot.Modules.Databasemodels
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        [BsonElement("Id")]
        public string DiscordId { get; set; }
    }

    public class UserRepository
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<User> _collection;

        public UserRepository(string connectionString)
        {
            this._client = new MongoClient(connectionString);
            this._database = this._client.GetDatabase("ArrowDnD");
            this._collection = this._database.GetCollection<User>("Users");
        }

        public async Task InsertUser(User user)
        {
            await this._collection.InsertOneAsync(user);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await this._collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<User> GetUserByInternalID(string id)
        {
            return await this._collection.Find(x => x.InternalId == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByInternalID(ObjectId id)
        {
            return await this._collection.Find(x => x.InternalId == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByDiscordID(string id)
        {
            return await this._collection.Find(x => x.DiscordId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteUser(User user)
        {
            var result = await this._collection.DeleteOneAsync(x => x.InternalId == user.InternalId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteUser(ObjectId id)
        {
            var result = await this._collection.DeleteOneAsync(x => x.InternalId == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var result = await this._collection.DeleteOneAsync(x => x.InternalId == ObjectId.Parse(id));
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
