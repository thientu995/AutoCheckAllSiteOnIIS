using Microsoft.Extensions.DependencyInjection;

var host = RegisterServices.CreateHostBuilder(args).Build();
host.Services.GetRequiredService<Startup>().Run();

if (!Settings.AutoClose)
{
    Console.ReadLine();
}
Environment.Exit(1);