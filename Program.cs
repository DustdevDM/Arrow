using DND_DC_Music_Bot;
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
    public static Task Main(string[] args) => new Program().MainAsync(args);

    /// <summary>
    /// Main Async Task.
    /// </summary>
    /// <param name="args">Arguments of Programm startup.</param>
    /// <returns>Async Task.</returns>
    public async Task MainAsync(string[] args)
    {
        // Initialize Ninject Dependecy Injection
        var kernel = new StandardKernel();

        var bot = kernel.Get<Bot>();

        bot.LoadConfig(args);

        await bot.ExecuteBotAsync();
    }
}