using Microsoft.Extensions.DependencyInjection;

namespace SimpleSolitare.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterModule<TModule>(this IServiceCollection services, object[]? parameters = null) where TModule : Module
        {
            var module = Activator.CreateInstance<TModule>();
            module.LoadModule(services, parameters);
        }
    }
}
