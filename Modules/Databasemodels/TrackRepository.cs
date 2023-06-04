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
    /// Database Repository for Tracks.
    /// </summary>
    public class TrackRepository
    {
        private IMongoClient mDBClient;
        private IMongoDatabase database;
        private IMongoCollection<Track> collection;

        /// <summary>
        /// Initializes a new instance of <see cref="TrackRepository"/>.
        /// </summary>
        /// <param name="connectionString">A connection string to a mongoDB Instnace</param>
        public TrackRepository(string connectionString)
        {
            this.mDBClient = new MongoClient(connectionString);
            this.database = this.mDBClient.GetDatabase("ArrowDnD");
            this.collection = this.database.GetCollection<Track>("Tracks");
        }

        /// <summary>
        /// Inserts a new track into the database.
        /// </summary>
        /// <param name="track">Instance of <see cref="Track"/>.</param>
        /// <returns>Async <see cref="Task"/>.</returns>
        public async Task IntertTrack(Track track)
        {
            await this.collection.InsertOneAsync(track);
        }

        /// <summary>
        /// Gets all tracks from the database.
        /// </summary>
        /// <returns>Async <see cref="Task"/> that carries <see cref="List{Track}"/>of <see cref="Track"/>s.</returns>
        public async Task<List<Track>> GetAllTracks()
        {
            return await this.collection.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// Gets all tracks from the database that match the providet userid.
        /// </summary>
        /// <param name="userId">The DiscordID of a User.</param>
        /// <returns>Async <see cref="Task"/> that carries <see cref="List{Track}"/>of <see cref="Track"/>s.</returns>
        public async Task<List<Track>> GetTracksByUserId(string userId)
        {
            return await this.collection.Find(x => x.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Gets a track by its ID.
        /// </summary>
        /// <param name="id">The ID of a <see cref="Track"/>.</param>
        /// <returns>Async <see cref="Task"/> that carries <see cref="Track"/>.</returns>
        public async Task<Track> GetTrackByID(string id)
        {
            return await this.collection.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates a Track in the database.
        /// </summary>
        /// <param name="track">Instance of <see cref="Track"/>.</param>
        /// <returns>Async <see cref="Task"/> that carries a <see cref="bool"/> which indicates if the operation was successful.</returns>
        public async Task<bool> UpdateTrack(Track track)
        {
            var filter = Builders<Track>.Filter.Eq(x => x.Id, track.Id);
            var update = Builders<Track>.Update
                .Set(x => x.Name, track.Name)
                .Set(x => x.Uri, track.Uri)
                .Set(x => x.UserId, track.UserId);
            var result = await this.collection.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        /// <summary>
        /// Delete a Track from the database by its ID.
        /// </summary>
        /// <param name="id">The id of a <see cref="Track"/>.</param>
        /// <returns>Async <see cref="Task"/> that carries a <see cref="bool"/> which indicates if the operation was successful.</returns>
        public async Task<bool> DeleteTrack(string id)
        {
            var filter = Builders<Track>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var result = await this.collection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Delete a Track from the database.
        /// </summary>
        /// <param name="track">Instance of <see cref="Track"/>.</param>
        /// <returns>Async <see cref="Task"/> that carries a <see cref="bool"/> which indicates if the operation was successful.</returns>
        public async Task<bool> DeleteTrack(Track track)
        {
            var filter = Builders<Track>.Filter.Eq(x => x.Id, track.Id);
            var result = await this.collection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
