using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_DC_Music_Bot.Modules.Databasemodels
{
    internal class Track
    {
        public string Id { get; set; }

        public string? Name { get; set; }

        public string? Uri { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Track"/> class.
        /// </summary>
        public Track()
        {

        }

        /// <summary>
        /// Gets Track by its Id from the Database.
        /// </summary>
        /// <param name="Id"></param>
        public Track(string Id)
        {

        }
    }
}
