using DND_DC_Music_Bot.Modules.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DND_DC_Music_Bot.Modules.Databasemodels
{
    /// <summary>
    /// A Track that can be played by the bot.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Track : ITrack
    {
        /// <inheritdoc />
        [BsonId]
        public ObjectId Id { get; set; }

        /// <inheritdoc />
        [BsonElement("Name")]
        public string? Name { get; set; }

        /// <inheritdoc />
        [BsonElement("Uri")]
        public string? Uri { get; set; }

        /// <inheritdoc />
        [BsonElement("UserId")]
        public string? UserId { get; set; }
    }
}
