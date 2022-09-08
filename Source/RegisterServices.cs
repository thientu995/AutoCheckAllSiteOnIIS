using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.Administration;
using System.Text.Json;

class RegisterServices
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices(Services);
    }

    static void Services(HostBuilderContext context, IServiceCollection services)
    {
        services
            // Transient
            .AddTransient<Startup>()
            .AddTransient<ServerManager>()
            // Singleton
            .AddSingleton((x) =>
            {
                return new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
            })
            .AddSingleton<IPhysicalPath, PhysicalPath>()
            .AddSingleton<IInfoSite, InfoSite>()
            .AddSingleton<INotification, Notification>();
    }
}