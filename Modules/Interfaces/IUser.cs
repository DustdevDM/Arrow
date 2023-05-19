using MongoDB.Bson;

namespace DND_DC_Music_Bot.Modules.Interfaces
{
    internal interface IUser
    {
        /// <summary>
        /// Internal Identifier set by MongoDB.
        /// </summary>
        public ObjectId InternalId { get; set; }

        /// <summary>
        /// Identifier of the Discord User set by Discord.
        /// </summary>
        public string? DiscordId { get; set; }
    }
}
