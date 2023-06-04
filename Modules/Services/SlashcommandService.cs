using System;
using System.Reflection;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Interfaces;
using Newtonsoft.Json;

namespace DND_DC_Music_Bot.Modules.Services
{
    /// <summary>
    /// Service to Import and Register Slashcommands to the Discord API.
    /// </summary>
    public class SlashCommandService
    {
        /// <summary>
        /// Handles any triggered Slashcommand.
        /// </summary>
        /// <param name="socketSlashCommand">Instance of <see cref="SocketSlashCommand"/></param>
        /// <returns></returns>
        public static async Task HandleSlashCommand(SocketSlashCommand socketSlashCommand)
        {
            await socketSlashCommand.DeferAsync();

            // Get Slashcommand from Namespace
            string nspace = "DND_DC_Music_Bot.Modules.Classes.Commands";
            IEnumerable<Type> slashCommands = Assembly.GetExecutingAssembly().GetTypes()
                                            .Where(t => t.Namespace == nspace && t.GetInterfaces().Contains(typeof(ISlashCommand)));

            Type? slashCommand = slashCommands.FirstOrDefault(x =>
            {
                ISlashCommand? slashCommandInstance = Activator.CreateInstance(x) as ISlashCommand;
                if (slashCommandInstance?.Name == socketSlashCommand.Data.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            if (slashCommand == null)
            {
                await socketSlashCommand.FollowupAsync("Failed to find Slashcommand");
                return;
            }

            ISlashCommand? slashCommandInstance = Activator.CreateInstance(slashCommand) as ISlashCommand;
            if (slashCommandInstance == null)
            {
                await socketSlashCommand.FollowupAsync("Failed to create Instance of Slashcommand");
                return;
            }

            try
            {
                Console.WriteLine($"[{nameof(SlashCommandService)}] Start execution of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the \"{socketSlashCommand.Channel.Name}\"({socketSlashCommand.ChannelId}) channel.");
                bool validationResukt = await slashCommandInstance.Validate(socketSlashCommand);
                if (validationResukt)
                {
                await slashCommandInstance.Execute(socketSlashCommand);
                Console.WriteLine($"[{nameof(SlashCommandService)}] End execution of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the \"{socketSlashCommand.Channel.Name}\"({socketSlashCommand.ChannelId}) channel.");
                } 
                else
                {
                    Console.WriteLine($"[{nameof(SlashCommandService)}] Cancel execution due to negative validation result of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the \"{socketSlashCommand.Channel.Name}\"({socketSlashCommand.ChannelId}) channel.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[{nameof(SlashCommandService)}] Cought Exception while executing of \"{slashCommandInstance.Name}\" which was ordered by user \"{socketSlashCommand.User.Username}\"({socketSlashCommand.User.Id}) on the \"{socketSlashCommand.Channel.Name}\"({socketSlashCommand.ChannelId}) channel.\n{exception.Message}");
                await socketSlashCommand.FollowupAsync(exception.Message);
            }
        }

        /// <summary>
        /// Import and Register all Slashcommands that are within the ".Modules.Classes.Commands" Namespace.
        /// </summary>
        public static void ImportAndRegisterCommands(DiscordSocketClient discordSocketClient)
        {
            string nspace = "DND_DC_Music_Bot.Modules.Classes.Commands";

            IEnumerable<Type> slashCommands = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(t => t.Namespace == nspace && t.GetInterfaces().Contains(typeof(ISlashCommand)));

            foreach (Type slashCommand in slashCommands)
            {
                ISlashCommand? slashCommandInstance = Activator.CreateInstance(slashCommand) as ISlashCommand;

                if (slashCommandInstance == null)
                {
                    Console.WriteLine($"[{nameof(SlashCommandService)}] Failed to create Instance of Slashcommand");
                    continue;
                }

                SlashCommandBuilder slashCommandAPIInstance = new();
                slashCommandAPIInstance.WithName(slashCommandInstance.Name);
                slashCommandAPIInstance.WithDescription(slashCommandInstance.Description);

                PushCommandtoAPI(discordSocketClient, slashCommandAPIInstance);
            }
        }

        private static async void PushCommandtoAPI(DiscordSocketClient discordSocketClient, SlashCommandBuilder slashCommand)
        {
            try
            {
                Console.WriteLine($"[{nameof(SlashCommandService)}] Pushing \"{slashCommand.Name}\" to Discord API");
                await discordSocketClient.CreateGlobalApplicationCommandAsync(slashCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors.First(), Formatting.Indented);
                Console.WriteLine($"[{nameof(SlashCommandService)}] Caught Exception while pushing SlashCommand to Discord:\n{json}");
            }
        }
    }
}
