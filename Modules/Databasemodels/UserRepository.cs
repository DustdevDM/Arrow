using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DND_DC_Music_Bot.Modules.Databasemodels
{
    /// <summary>
    /// Database Repository for Users.
    /// </summary>
    public class UserRepository
    {
        private IMongoClient mDBClient;
        private IMongoDatabase database;
        private IMongoCollection<User> collection;

        /// <summary>
        /// Initializes a new instance of <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="connectionString">A connection string to a mongoDB Instance.</param>
        public UserRepository(string connectionString)
        {
            this.mDBClient = new MongoClient(connectionString);
            this.database = this.mDBClient.GetDatabase("ArrowDnD");
            this.collection = this.database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Insterts a new User into the database.
        /// </summary>
        /// <param name="user">Instance of <see cref="User"/>.</param>
        /// <returns>Async <see cref="Task"/>.</returns>
        public async Task InsertUser(User user)
        {
            await this.collection.InsertOneAsync(user);
        }

        /// <summary>
        /// Gets all Users from the database.
        /// </summary>
        /// <returns>Async <see cref="Task"/> that carries <see cref="List{Track}"/>of <see cref="User"/>s.</returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await this.collection.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// Gets User by its internal ID.
        /// </summary>
        /// <param name="id">Internal User ID.</param>
        /// <returns>Async <see cref="Task"/> that carries <see cref="User"/>.</returns>
        public async Task<User> GetUserByInternalID(string id)
        {
            return await this.collection.Find(x => x.InternalId == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets User by its internal ID.
        /// </summary>
        /// <param name="id">Internal User ID.</param>
        /// <returns>Async <see cref="Task"/> that carries <see cref="User"/>.</returns>
        public async Task<User> GetUserByInternalID(ObjectId id)
        {
            return await this.collection.Find(x => x.InternalId == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets User by its Discord ID.
        /// </summary>
        /// <param name="id">Discord User ID.</param>
        /// <returns>Async <see cref="Task"/> that carries <see cref="User"/>.</returns>
        public async Task<User> GetUserByDiscordID(string id)
        {
            return await this.collection.Find(x => x.DiscordId == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Deletes a User from the database.
        /// </summary>
        /// <param name="user">Instance of <see cref="User"/>.</param>
        /// <returns>Async <see cref="Task"/> that carries a <see cref="bool"/> which indicates if the operation was successful.</returns>
        public async Task<bool> DeleteUser(User user)
        {
            var result = await this.collection.DeleteOneAsync(x => x.InternalId == user.InternalId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Deletes a User from the database.
        /// </summary>
        /// <param name="id">Internal User ID.</param>
        /// <returns>Async <see cref="Task"/> that carries a <see cref="bool"/> which indicates if the operation was successful.</returns>
        public async Task<bool> DeleteUser(ObjectId id)
        {
            var result = await this.collection.DeleteOneAsync(x => x.InternalId == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Deletes a User from the database.
        /// </summary>
        /// <param name="id">Discord User ID.</param>
        /// <returns>Async <see cref="Task"/> that carries a <see cref="bool"/> which indicates if the operation was successful.</returns>
        public async Task<bool> DeleteUser(string id)
        {
            var result = await this.collection.DeleteOneAsync(x => x.InternalId == ObjectId.Parse(id));
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
