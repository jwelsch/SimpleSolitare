using Microsoft.Extensions.DependencyInjection;
using SimpleSolitare.Wraps;

namespace SimpleSolitare.DependencyInjection
{
    public class ApplicationModule : Module
    {
        protected override void Load(IServiceCollection services, object[]? parameters = null)
        {
            services.AddTransient<ILoggerFactoryWrap, LoggerFactoryWrap>();
            services.AddTransient<IDeckProvider, DeckProvider>();
            services.AddTransient<IRngFactory, RngFactory>();
            services.AddTransient<IDeckShuffler, DeckShuffler>();
            services.AddTransient<IPlayer, Player>();
            services.AddTransient<IGameRunner, GameRunner>();
        }
    }
}
