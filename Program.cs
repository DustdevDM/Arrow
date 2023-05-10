using Discord;
using Discord.WebSocket;
using DND_DC_Music_Bot;
using Ninject;
using System.Diagnostics;

public class Program
{
    private DiscordSocketClient? client;

    public static Task Main(string[] args) => new Program().MainAsync(args);

    public async Task MainAsync(string[] args)
    {
        //Initialize Ninject Dependecy Injection
        var kernel = new StandardKernel();
        client = kernel.Get<DiscordSocketClient>();

        //Check for .env file
        if (args.Count() == 0) throw new ArgumentNullException("Missing .env Path Argument");

        //Load .env file
        EnvLoader.Load(args[0]);


        client.Log += Log;

        var token = Environment.GetEnvironmentVariable("DISCORDTOKEN");

        //Login and connect to Discord.
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}