using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Classes;
using DND_DC_Music_Bot.Modules.Services;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;

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
        private IAudioService audioService;
        private readonly Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="config">Instance of <see cref="ConfigService"/>.</param>
        /// <param name="slashcommandService">Instance of <see cref="SlashCommandService"/>.</param>
        /// <param name="discordSocketClient">Instance of <see cref="DiscordSocketClient"/>.</param>
        public Bot(ConfigService config, SlashCommandService slashcommandService, DiscordSocketClient discordSocketClient, IAudioService audioService, Logger logger)
        {
            this.config = config;
            this.slashcommandService = slashcommandService;
            this.discordSocketClient = discordSocketClient;
            this.audioService = audioService;
            this.logger = logger;
        }

        /// <summary>
        /// Executes the bot. Loads the config. Login and connects to Discord.
        /// </summary>
        internal async Task ExecuteBotAsync(bool enableDCLogs, bool rebuildSlashcommands)
        {
            if (enableDCLogs)
            {
                this.discordSocketClient.Log += this.Log;
            }

            var token = this.config.DiscordToken;

            // Login and connect to Discord.
            await this.discordSocketClient.LoginAsync(TokenType.Bot, token);
            await this.discordSocketClient.StartAsync();

            //Register Slashcommands after the bot is connected.
            this.discordSocketClient.Ready += async () =>
            {
                try
                {
                    if (rebuildSlashcommands)
                    {
                        await this.slashcommandService.ClearCommands(this.discordSocketClient);
                    }

                    await this.slashcommandService.ImportAndRegisterCommands(this.discordSocketClient);

                    await this.audioService.InitializeAsync();

                    this.discordSocketClient.SlashCommandExecuted += this.slashcommandService.HandleSlashCommand;
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };
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
            this.logger.Log(nameof(Discord), msg.Message);
            return Task.CompletedTask;
        }
    }
}
