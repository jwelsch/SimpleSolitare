using Microsoft.Extensions.DependencyInjection;

namespace SimpleSolitare.DependencyInjection
{
    public class ApplicationModule : Module
    {
        protected override void Load(IServiceCollection services, object[]? parameters = null)
        {
            services.AddTransient<IRngFactory, RngFactory>();
            services.AddTransient<IDeckShuffler, DeckShuffler>();
        }
    }
}
