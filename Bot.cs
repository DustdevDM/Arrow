using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot.Modules.Databasemodels;
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
        private SlashcommandService slashcommandService;
        private DiscordSocketClient discordSocketClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="config">Instance of <see cref="ConfigService"/>.</param>
        /// <param name="slashcommandService">Instance of <see cref="SlashcommandService"/></param>
        /// <param name="discordSocketClient">Instance of <see cref="DiscordSocketClient"/>.</param>
        public Bot(ConfigService config, SlashcommandService slashcommandService, DiscordSocketClient discordSocketClient)
        {
            this.config = config;
            this.slashcommandService = slashcommandService;
            this.discordSocketClient = discordSocketClient;
        }

        /// <summary>
        /// Executes the bot. Loads the config, logs in and connects to Discord.
        /// </summary>
        internal async Task ExecuteBotAsync()
        {
            this.discordSocketClient.Log += this.Log;

            var token = this.config.DiscordToken;

            // Login and connect to Discord.
            await this.discordSocketClient.LoginAsync(TokenType.Bot, token);
            await this.discordSocketClient.StartAsync();

            this.discordSocketClient.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                SlashcommandService.ImportAndRegisterCommands(this.discordSocketClient);
                return Task.CompletedTask;
            };

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        /// <summary>
        /// Loads the config from the given file path.
        /// </summary>
        /// <param name="args">Programm startup Arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown if no argument was passed at programm startup.</exception>
        internal void LoadConfig(string[] args)
        {
            // Check if argument was passeda at programm startup.
            if (args.Count() == 0)
            {
                throw new ArgumentNullException(nameof(args), "Missing .json FilePath Argument");
            }

            this.config.Loader(args[0]);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
