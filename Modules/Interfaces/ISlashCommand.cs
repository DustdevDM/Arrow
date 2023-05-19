using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_DC_Music_Bot.Modules.Interfaces
{
    public interface ISlashCommand
    {
        /// <summary>
        /// The Name of the Slash Command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Description of the Slash Command
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// A Function to Validate that the Command can be Executed
        /// </summary>
        /// <returns></returns>
        public Task<bool> Validate();

        /// <summary>
        /// A Function to Execute the Command
        /// </summary>
        /// <returns></returns>
        public Task Execute();
    }
}
