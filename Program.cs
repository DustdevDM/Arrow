using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot;
using System.Diagnostics;

public class Program
{
    private DiscordSocketClient _client;

    public static Task Main(string[] args) => new Program().MainAsync(args);

    public async Task MainAsync(string[] args)
    {
        if (args.Count() == 0) throw new ArgumentNullException("Missing .env Path Argument");

        EnvLoader.Load(args[0]);

        _client = new DiscordSocketClient();

        _client.Log += Log;

        var token = Environment.GetEnvironmentVariable("DISCORDTOKEN");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}