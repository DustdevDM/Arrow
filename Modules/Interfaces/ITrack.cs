using MongoDB.Bson;

namespace DND_DC_Music_Bot.Modules.Interfaces
{
    internal interface ITrack
    {
        public ObjectId Id { get; set; }

        /// <summary>
        /// The name of the track.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The URI of the track.
        /// </summary>
        public string? Uri { get; set; }

        /// <summary>
        /// The Discord User ID of the user who added the track.
        /// </summary>
        public string? UserId { get; set; }
    }
}
