using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Classes;
using DND_DC_Music_Bot.Modules.Classes.Commands;
using DND_DC_Music_Bot.Modules.Interfaces;
using Newtonsoft.Json;

namespace DND_DC_Music_Bot.Modules.Services
{
    /// <summary>
    /// Service to Import and Register Slashcommands to the Discord API.
    /// </summary>
    public class SlashCommandService
    {
        private Logger logger;
        private AddTrackCommand trackCommand;

        public SlashCommandService(Logger logger, AddTrackCommand trackCommand)
        {
            this.logger = logger;
            this.trackCommand = trackCommand;
        }

        /// <summary>
        /// Handles any triggered Slashcommand.
        /// </summary>
        /// <param name="socketSlashCommand">Instance of <see cref="SocketSlashCommand"/></param>
        /// <returns></returns>
        public async Task HandleSlashCommand(SocketSlashCommand socketSlashCommand)
        {
            await socketSlashCommand.DeferAsync();

            // Get Slashcommand from Namespace
            string nspace = "DND_DC_Music_Bot.Modules.Classes.Commands";
            IEnumerable<Type> slashCommands = Assembly.GetExecutingAssembly().GetTypes()
                                            .Where(t => t.Namespace == nspace && t.GetInterfaces().Contains(typeof(ISlashCommand)));

            ISlashCommand slashCommandInstance = null;

            switch (socketSlashCommand.Data.Name)
            {
                case "track":
                    slashCommandInstance = this.trackCommand;
                    break;
                default:
                    socketSlashCommand.RespondAsync("Command not found");
                    return;
            }

            try
            {
                this.logger.Log(nameof(SlashCommandService), $"Start execution of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the {socketSlashCommand.ChannelId} channel.");
                bool validationResukt = await slashCommandInstance.Validate(socketSlashCommand);
                if (validationResukt)
                {
                await slashCommandInstance.Execute(socketSlashCommand);
                this.logger.Log(nameof(SlashCommandService), $"End execution of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the {socketSlashCommand.ChannelId} channel.");
                }
                else
                {
                    this.logger.Log(nameof(SlashCommandService), $"Cancel execution due to negative validation result of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the \"{socketSlashCommand.Channel.Name}\"({socketSlashCommand.ChannelId}) channel.");
                }
            }
            catch(Exception e)
            {
                this.logger.Log(nameof(SlashCommandService), $"Cought Exception while executing of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the {socketSlashCommand.ChannelId} channel.\n{e}");
                await socketSlashCommand.FollowupAsync(e.Message ?? "Something went wrong");
            }
        }

        /// <summary>
        /// Import and Register all Slashcommands that are within the ".Modules.Classes.Commands" Namespace.
        /// </summary>
        public async Task ImportAndRegisterCommands(DiscordSocketClient discordSocketClient)
        {
            List<ISlashCommand> slashCommands = new List<ISlashCommand>();

            slashCommands.Add(this.trackCommand);

            foreach (ISlashCommand slashCommand in slashCommands)
            {
                SlashCommandBuilder slashCommandAPIInstance = new();
                slashCommandAPIInstance.WithName(slashCommand.Name);
                slashCommandAPIInstance.WithDescription(slashCommand.Description);
                slashCommandAPIInstance.AddOptions(slashCommand.Options);

                await this.PushCommandtoAPI(discordSocketClient, slashCommandAPIInstance);
            }

            return;
        }

        /// <summary>
        /// Clear all Slashcommands from the Discord API.
        /// </summary>
        public async Task ClearCommands(DiscordSocketClient discordSocketClient)
        {
            IReadOnlyCollection<SocketApplicationCommand> socketApplicationCommands = await discordSocketClient.GetGlobalApplicationCommandsAsync();

            foreach (SocketApplicationCommand socketApplicationCommand in socketApplicationCommands)
            {
                try
                {
                    this.logger.Log(nameof(SlashCommandService), $"Removing \"{socketApplicationCommand.Name}\" from Discord API");
                    await socketApplicationCommand.DeleteAsync();
                    return;
                }
                catch (HttpException exception)
                {
                    var json = JsonConvert.SerializeObject(exception.Errors.First(), Formatting.Indented);
                    this.logger.Log(nameof(SlashCommandService), $"Caught Exception while removing SlashCommand from Discord:\n{json}");
                }
            }
        }

        private async Task PushCommandtoAPI(DiscordSocketClient discordSocketClient, SlashCommandBuilder slashCommand)
        {
            try
            {
                this.logger.Log(nameof(SlashCommandService), $"Pushing \"{slashCommand.Name}\" to Discord API");
                await discordSocketClient.CreateGlobalApplicationCommandAsync(slashCommand.Build());
                return;
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors.First(), Formatting.Indented);
                this.logger.Log(nameof(SlashCommandService), $"Caught Exception while pushing SlashCommand to Discord:\n{json}");
            }
        }
    }
}
