using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Interfaces;

namespace DND_DC_Music_Bot.Modules.Classes.Commands
{
    internal class AddTrackCommand : ISlashCommand
    {
        public string Name => "track";

        public string Description => "Manage your tracks";

        public SlashCommandOptionBuilder[] Options => new SlashCommandOptionBuilder[]
        {
            new SlashCommandOptionBuilder().WithName("add").WithDescription("Add a Track").WithType(ApplicationCommandOptionType.SubCommand),
            new SlashCommandOptionBuilder().WithName("remove").WithDescription("Remove a Track").WithType(ApplicationCommandOptionType.SubCommand),
        };

        public Task Execute(SocketSlashCommand socketSlashCommand)
        {
            socketSlashCommand.FollowupAsync("You used the " + socketSlashCommand.Data.Options.First().Name + " option");
            return Task.CompletedTask;
        }

        public Task<bool> Validate(SocketSlashCommand socketSlashCommand)
        {
            return Task.FromResult(true);
        }
    }
}
