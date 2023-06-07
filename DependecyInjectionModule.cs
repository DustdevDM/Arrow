using Discord.WebSocket;
using Discord;
using DND_DC_Music_Bot.Modules.Services;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Logging;
using Ninject.Modules;
using DND_DC_Music_Bot.Modules.Classes;
using DND_DC_Music_Bot.Modules.Classes.Commands;

namespace DND_DC_Music_Bot
{
    /// <summary>
    /// Dependency Injection Module.
    /// </summary>
    internal class DependecyInjectionModule : NinjectModule
    {
        /// <summary>
        /// Loads the Dependency Injection Module.
        /// </summary>
        public override void Load()
        {
            this.Bind<ConfigService>().ToSelf().InSingletonScope();
            this.Bind<SlashCommandService>().ToSelf();
            this.Bind<ILogger>().To<Logger>();

            this.Bind<DiscordSocketClient>().ToSelf().InSingletonScope().WithConstructorArgument(new DiscordSocketConfig() { UseInteractionSnowflakeDate = false, GatewayIntents = GatewayIntents.GuildVoiceStates });

            this.Bind<AddTrackCommand>().ToSelf();

            this.Bind<IDiscordClientWrapper>().To<DiscordClientWrapper>().InSingletonScope();
            ILavalinkCache cache = null;
            this.Bind<IAudioService>().To<LavalinkNode>().InSingletonScope().WithConstructorArgument("options", new LavalinkNodeOptions() { RestUri = "fdskfjdkls", WebSocketUri = "fdsfdsf", Password = "fsdfdsf" }).WithConstructorArgument("cache", cache);
        }
    }
}
