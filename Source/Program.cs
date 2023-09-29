using Microsoft.Extensions.DependencyInjection;

var host = RegisterServices.CreateHostBuilder(args).Build();
var program = host.Services.GetRequiredService<Startup>();
program.Run();