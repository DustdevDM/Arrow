using System.Reflection;
using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Interfaces;

namespace DND_DC_Music_Bot.Modules.Services
{
    /// <summary>
    /// Service to Import and Register Slashcommands to the Discord API.
    /// </summary>
    public class SlashcommandService
    {
        /// <summary>
        /// Import and Register all Slashcommands that are within the ".Modules.Classes.Commands" Namespace.
        /// </summary>
        public static async void ImportAndRegisterCommands(DiscordSocketClient discordSocketClient)
        {
            string nspace = "DND_DC_Music_Bot.Modules.Classes.Commands";

            IEnumerable<Type> slashCommands = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(t => t.Namespace == nspace && t.GetInterfaces().Contains(typeof(ISlashCommand)));

            foreach (Type slashCommand in slashCommands)
            {
                ISlashCommand? slashCommandInstance = Activator.CreateInstance(slashCommand) as ISlashCommand;

                if (slashCommandInstance == null)
                {
                    Console.WriteLine("Failed to create Instance of Slashcommand");
                    continue;
                }

                SlashCommandBuilder slashCommandAPIInstance = new();
                slashCommandAPIInstance.WithName(slashCommandInstance.Name);
                slashCommandAPIInstance.WithDescription(slashCommandInstance.Description);

                await discordSocketClient.CreateGlobalApplicationCommandAsync(slashCommandAPIInstance.Build());
            }
        }

        /// <summary>
        /// Import and Register all Slashcommands that are within the ".Modules.Classes.Commands" Namespace.
        /// </summary>
        public static async void RegisterCommand(DiscordSocketClient discordSocketClient, ISlashCommand slashCommand)
        {
            SlashCommandBuilder slashCommandAPIInstance = new();
            slashCommandAPIInstance.WithName(slashCommand.Name);
            slashCommandAPIInstance.WithDescription(slashCommand.Description);

            await discordSocketClient.CreateGlobalApplicationCommandAsync(slashCommandAPIInstance.Build());
        }
    }
}
