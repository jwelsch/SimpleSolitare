using Microsoft.Extensions.DependencyInjection;
using SimpleSolitare.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = RegisterAppServices();
    }

    private static IServiceProvider RegisterAppServices()
    {
        var services = new ServiceCollection();

        ConfigureServices(services);
        RegisterModules(services);

        return services.BuildServiceProvider();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        //services.AddLogging(configure =>
        //{
        //    configure.AddConsole(o => o.FormatterName = nameof(CustomConsoleFormatter))
        //             .AddConsoleFormatter<CustomConsoleFormatter, CustomConsoleOptions>();
        //    configure.AddDebug();
        //});
    }

    private static void RegisterModules(IServiceCollection services)
    {
        services.RegisterModule<ApplicationModule>();
    }
}