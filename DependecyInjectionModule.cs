using DND_DC_Music_Bot.Modules.Services;
using Ninject.Modules;

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
        }
    }
}
