using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DND_DC_Music_Bot.Modules.Interfaces
{
    /// <summary>
    /// Interface for Slash Commands.
    /// </summary>
    public interface ISlashCommand
    {
        /// <summary>
        /// The Name of the Slash Command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Description of the Slash Command.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// A Function to Validate that the Command can be Executed.
        /// </summary>
        public Task<bool> Validate(SocketSlashCommand socketSlashCommand);

        /// <summary>
        /// A Function to Execute the Command.
        /// </summary>
        public Task Execute(SocketSlashCommand socketSlashCommand);
    }
}
