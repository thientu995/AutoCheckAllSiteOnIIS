using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.Administration;
using System.Text.Json;
using System.Text.Json.Serialization;

class RegisterServices
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices(Services);
    }

    static void Services(HostBuilderContext context, IServiceCollection services)
    {
        var jsonOption = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        jsonOption.Converters.Add(new DateTimeConverter());
        services
            // Transient
            .AddTransient<Startup>()
            .AddTransient<ServerManager>()
            // Singleton
            .AddSingleton((x) =>
            {
                return jsonOption;
            })
            .AddSingleton<IPhysicalPath, PhysicalPath>()
            .AddSingleton<IInfoSite, InfoSite>()
            .AddSingleton<IGetInfoSiteJSON, GetInfoSiteJSON>()
            .AddSingleton<INotification, Notification>()
            .AddSingleton<IProcessor, Processor>();
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToLocalTime().ToString(Settings.FormatDate));
        }
    }
}