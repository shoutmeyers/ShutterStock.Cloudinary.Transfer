using Cloudinary.ApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ShutterStock.ApiClient;
using System;
using System.IO;
using System.Linq;

namespace ShutterStock.Cloudinary.Transfer
{
    internal class Program
    {
        public static IConfigurationRoot Configuration;

        private static void Main()
        {
            try
            {
                var serviceProvider = Initialize();

                var configSection = Configuration.GetSection("ShutterStock");

                var baseUrl = configSection["BaseUrl"];
                var bearer = configSection["Bearer"];

                Console.WriteLine(baseUrl);

                Console.WriteLine(bearer);

                var client = serviceProvider.GetService<IShutterStockApiClient>();

                var folderPath = Configuration.GetSection("ServerPath").Value;
                Log.Information($"De ROOT van de directory is {folderPath}.");

                var user = client.GetUser();
                Log.Information(user.Username);

                var subscriptions = client.GetSubscriptions();
                Log.Information(subscriptions.Data.FirstOrDefault()?.Id);

                var startDir = new DirectoryInfo(folderPath);

                var recurseFileStructure = serviceProvider.GetService<IRecurseFileStructure>();

                recurseFileStructure.TraverseDirectory(startDir, client);

                Console.WriteLine("Press ANY key to exit");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IServiceProvider Initialize()
        {
            try
            {
                // Initialize serilog logger
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                    .WriteTo.File("logs\\transfer-.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .CreateLogger();

                // Create service collection
                Log.Information("Creating service collection");
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                // Create service provider
                Log.Information("Building service provider");
                IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
                return serviceProvider;
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Initialize failed.");
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            try
            {
                // Add logging
                serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
                {
                    builder
                        .AddSerilog(dispose: true);
                }));

                serviceCollection.AddLogging();

                // Build configuration
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.json", false)
                    .Build();

                // Add access to generic IConfigurationRoot
                serviceCollection.AddSingleton(Configuration);

                // Add app
                serviceCollection.AddScoped<IRecurseFileStructure, RecurseFileStructure>();
                serviceCollection.AddScoped<IShutterStockApiClient, ShutterStockApiClient>();
                serviceCollection.AddScoped<ICloudinaryApiClient, CloudinaryApiClient>();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "ConfigureServices failed.");
                throw;
            }
        }
    }
}
