using DND_DC_Music_Bot.Modules.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DND_DC_Music_Bot.Modules.Databasemodels
{
    /// <summary>
    /// A User that stands for a Discord User.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : IUser
    {
        /// <inheritdoc />
        [BsonId]
        public ObjectId InternalId { get; set; }

        /// <inheritdoc />
        [BsonElement("Id")]
        public string? DiscordId { get; set; }
    }
}
