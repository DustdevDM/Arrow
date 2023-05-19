using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DND_DC_Music_Bot.Modules.Services
{
    public class SlashcommandService
    {
        public async void ImportAndRegisterCommands(DiscordSocketClient discordSocketClient)
        {
            string nspace = "DND_DC_Music_Bot.Modules.Classes.Commands";

            var slashCommands = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(t => t.Namespace == nspace && t.GetInterfaces().Contains(typeof(ISlashCommand)));

            foreach (var slashCommand in slashCommands)
            {
                ISlashCommand? slashCommandInstance = Activator.CreateInstance(slashCommand) as ISlashCommand;

                if (slashCommandInstance == null)
                {
                    Console.WriteLine("Failed to create Instance of Slashcommand");
                    continue;
                }

                var slashCommandAPIInstance = new SlashCommandBuilder();
                slashCommandAPIInstance.WithName(slashCommandInstance.Name);
                slashCommandAPIInstance.WithDescription(slashCommandInstance.Description);

                await discordSocketClient.CreateGlobalApplicationCommandAsync(slashCommandAPIInstance.Build());

            }
        }
    }
}
