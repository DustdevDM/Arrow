using Discord;
using Discord.WebSocket;
using Ninject;

/// <summary>
/// Programm Entry.
/// </summary>
public class Program
{
    private DiscordSocketClient? client;

    /// <summary>
    /// Programm Entry.
    /// </summary>
    /// <param name="args">Arguments of Programm startup.</param>
    /// <returns>Async Tasks.</returns>
    public static Task Main(string[] args) => new Program().MainAsync(args);

    /// <summary>
    /// Main Async Task.
    /// </summary>
    /// <param name="args">Arguments of Programm startup.</param>
    /// <returns>Async Task.</returns>
    /// <exception cref="ArgumentNullException">Missing Arguments.</exception>
    public async Task MainAsync(string[] args)
    {
        // Initialize Ninject Dependecy Injection
        var kernel = new StandardKernel();
        this.client = kernel.Get<DiscordSocketClient>();

        // Check for .env file
        if (args.Count() == 0)
        {
            throw new ArgumentNullException("Missing .env Path Argument");
        }

        this.client.Log += this.Log;

        var token = Environment.GetEnvironmentVariable("DISCORDTOKEN");

        // Login and connect to Discord.
        await this.client.LoginAsync(TokenType.Bot, token);
        await this.client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}