using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollingStationsResolver.Domain;
using PollingStationsResolver.Migrator;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

Console.WriteLine(FiggleFonts.Ogre.Render("Domains Migrator"));
Console.WriteLine("Registering contexts.");

services.AddApplicationDomain(configuration);

Console.WriteLine("Done: Registering contexts.");

var serviceProvider = services.BuildServiceProvider();
var dbContext = serviceProvider.GetService<PollingStationsResolverContext>()!;

Console.WriteLine($"Migrating {dbContext.GetType().Name}.");
await dbContext.CreateAndMigrateAsync();
Console.WriteLine($"Done: Migrating {dbContext.GetType().Name}.");

//TODO: create hangfire db if it does not exists

Console.WriteLine("All good. Have a nice day!");
