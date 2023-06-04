using CommandLine;
using CommandLine.Text;
using DND_DC_Music_Bot;
using Newtonsoft.Json;
using Ninject;

/// <summary>
/// Programm Entry.
/// </summary>
public class Program
{
    /// <summary>
    /// Programm Entry.
    /// </summary>
    /// <param name="args">Arguments of Programm startup.</param>
    /// <returns>Async Tasks.</returns>
    public static Task Main(string[] args) => MainAsync(args);

    /// <summary>
    /// Main Async Task.
    /// </summary>
    /// <param name="args">Arguments of Programm startup.</param>
    /// <returns>Async Task.</returns>
    public static async Task MainAsync(string[] args)
    {
        // Initialize Ninject Dependecy Injection
        var kernel = new StandardKernel();

        var bot = kernel.Get<Bot>();

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(async o =>
            {
                bot.LoadConfig(o.ConfigFilePath ?? throw new ArgumentNullException(nameof(o.ConfigFilePath)));
                await bot.ExecuteBotAsync(o.EnableDiscordNETLogs);
            }).WithNotParsed<Options>(e =>
            {
                Environment.Exit(0);
            });

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    /// <summary>
    /// Class to define Commandline Input Options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Required Commandline option to set the Config File Path.
        /// </summary>
        [Option('c', "configpath", Required = true, HelpText = "Set filepath to configurationfile")]
        public string? ConfigFilePath { get; set; }

        /// <summary>
        /// Optional Commandline option to enable Discord.Net output.
        /// </summary>
        [Option("enabledclog", Required = false, Default = true, HelpText = "Set option to enable the Discord.Net output")]
        public bool EnableDiscordNETLogs { get; set; }
    }
}