using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_DC_Music_Bot.Modules.Databasemodels
{
    internal class User
    {
        /// <summary>
        /// Creates a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Gets User by its Id from the Database.
        /// </summary>
        /// <param name="id"></param>
        public User(string id)
        {
            this.Id = id;
        }

        string Id { get; set; }
    }
}
