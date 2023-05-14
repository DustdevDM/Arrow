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
    public class Track
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Uri")]
        public string Uri { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }
    }

    public class TrackRepository
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Track> _collection;

        public TrackRepository(string connectionString)
        {
            this._client = new MongoClient(connectionString);
            this._database = this._client.GetDatabase("ArrowDnD");
            this._collection = this._database.GetCollection<Track>("Tracks");
        }

        public async Task IntertTrack(Track track)
        {
            await this._collection.InsertOneAsync(track);
        }

        public async Task<List<Track>> GetAllTracks()
        {
            return await this._collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Track> GetTrackByID(string id)
        {
            return await this._collection.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<List<Track>> GetTracksByUserId(string userId)
        {
            return await this._collection.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateTrack(Track track)
        {
            var filter = Builders<Track>.Filter.Eq(x => x.Id, track.Id);
            var update = Builders<Track>.Update
                .Set(x => x.Name, track.Name)
                .Set(x => x.Uri, track.Uri)
                .Set(x => x.UserId, track.UserId);
            var result = await this._collection.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTrack(string id)
        {
            var filter = Builders<Track>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var result = await this._collection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteTrack(Track track)
        {
            var filter = Builders<Track>.Filter.Eq(x => x.Id, track.Id);
            var result = await this._collection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
