using Discord;
using Discord.WebSocket;

namespace DND_DC_Music_Bot
{
    /// <summary>
    /// Main Code inside of dependecy injection container.
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="config">Instance of <see cref="Config"/>.</param>
        /// <param name="discordSocketClient">Instance of <see cref="DiscordSocketClient"/>.</param>
        public Bot(Config config, DiscordSocketClient discordSocketClient)
        {
            this.Config = config;
            this.DiscordSocketClient = discordSocketClient;
        }

        private Config Config { get; set; }

        private DiscordSocketClient DiscordSocketClient { get; set; }

        /// <summary>
        /// Executes the bot. Loads the config, logs in and connects to Discord.
        /// </summary>
        internal async Task ExecuteBotAsync()
        {
            this.DiscordSocketClient.Log += this.Log;

            var token = this.Config.DiscordToken;

            // Login and connect to Discord.
            await this.DiscordSocketClient.LoginAsync(TokenType.Bot, token);
            await this.DiscordSocketClient.StartAsync();

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
                throw new ArgumentNullException("Missing .json FilePath Argument");
            }

            this.Config.Loader(args[0]);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
