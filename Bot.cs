using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Services;
using MongoDB.Driver;

namespace DND_DC_Music_Bot
{
    /// <summary>
    /// Main Code inside of dependecy injection container.
    /// </summary>
    public class Bot
    {
        private ConfigService config;
        private SlashCommandService slashcommandService;
        private DiscordSocketClient discordSocketClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="config">Instance of <see cref="ConfigService"/>.</param>
        /// <param name="slashcommandService">Instance of <see cref="SlashCommandService"/>.</param>
        /// <param name="discordSocketClient">Instance of <see cref="DiscordSocketClient"/>.</param>
        public Bot(ConfigService config, SlashCommandService slashcommandService, DiscordSocketClient discordSocketClient)
        {
            this.config = config;
            this.slashcommandService = slashcommandService;
            this.discordSocketClient = discordSocketClient;
        }

        /// <summary>
        /// Executes the bot. Loads the config. Login and connects to Discord.
        /// </summary>
        internal async Task ExecuteBotAsync(bool enableDCLogs)
        {
            this.discordSocketClient = new DiscordSocketClient(new DiscordSocketConfig() { UseInteractionSnowflakeDate = false});

            if (enableDCLogs)
            {
                this.discordSocketClient.Log += this.Log;
            }

            var token = this.config.DiscordToken;

            // Login and connect to Discord.
            await this.discordSocketClient.LoginAsync(TokenType.Bot, token);
            await this.discordSocketClient.StartAsync();

            //Register Slashcommands after the bot is connected.
            this.discordSocketClient.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                SlashCommandService.ImportAndRegisterCommands(this.discordSocketClient);
                return Task.CompletedTask;
            };

            // Handle Slashcommands.
            this.discordSocketClient.SlashCommandExecuted += SlashCommandService.HandleSlashCommand;
        }

        /// <summary>
        /// Loads the config from the given file path.
        /// </summary>
        /// <param name="configFilePath">Filepath to configuration File.</param>
        /// <exception cref="ArgumentNullException">Thrown if no argument was passed at programm startup.</exception>
        internal void LoadConfig(string configFilePath)
        {
            this.config.Loader(configFilePath);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[{nameof(Discord)}] {msg.Message}");
            return Task.CompletedTask;
        }
    }
}
