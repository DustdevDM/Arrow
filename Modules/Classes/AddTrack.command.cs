using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Databasemodels;
using DND_DC_Music_Bot.Modules.Interfaces;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;

namespace DND_DC_Music_Bot.Modules.Classes.Commands
{
    public class AddTrackCommand : ISlashCommand
    {
        private IAudioService audioService;

        public AddTrackCommand(IAudioService audioService)
        {
            this.audioService = audioService;
        }

        public string Name => "track";

        public string Description => "Manage your tracks";

        public SlashCommandOptionBuilder[] Options => new SlashCommandOptionBuilder[]
        {
            new SlashCommandOptionBuilder().WithName("add").WithDescription("Add a Track").WithType(ApplicationCommandOptionType.SubCommand)
                .AddOption("serach_query", ApplicationCommandOptionType.String, "The Search Query used to find your Song. Can be a Uri or a Song- or Videoname", true),
            new SlashCommandOptionBuilder().WithName("remove").WithDescription("Remove a Track").WithType(ApplicationCommandOptionType.SubCommand),
        };

        public Task Execute(SocketSlashCommand socketSlashCommand)
        {
            switch (socketSlashCommand.Data.Options.First().Name)
            {
                case "add":
                    this.AddTrackAsync(socketSlashCommand);
                    break;
                case "remove":
                    this.RemoveTrack(socketSlashCommand);
                    break;
            }

            return Task.CompletedTask;
        }

        public Task<bool> Validate(SocketSlashCommand socketSlashCommand)
        {
            return Task.FromResult(true);
        }

        private async Task AddTrackAsync(SocketSlashCommand socketSlashCommand)
        {
            //get the search query
            string searchQuery = socketSlashCommand.Data.Options.First().Options.First().Value.ToString();

            LavalinkTrack myTrack = await this.audioService.GetTrackAsync(searchQuery, SearchMode.YouTube);

            if (myTrack == null)
            {
                await socketSlashCommand.FollowupAsync("Track not found");
                return;
            }

            Track track = new Track()
            {
                Name = myTrack.Title,
                Uri = myTrack.Uri.ToString(),
                UserId = socketSlashCommand.User.Id.ToString(),
            };

        }

        private void RemoveTrack(SocketSlashCommand socketSlashCommand)
        {
            throw new NotImplementedException();
        }

    }
}
