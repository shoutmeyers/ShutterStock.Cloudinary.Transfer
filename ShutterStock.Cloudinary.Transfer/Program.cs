using Cloudinary.ApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShutterStock.ApiClient;
using System;
using System.IO;
using System.Linq;

namespace ShutterStock.Cloudinary.Transfer
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddLogging(cfg => cfg.AddConsole())
                    .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Debug)
                    .AddScoped<ICloudinaryApiClient, CloudinaryApiClient>()
                    .AddScoped<IShutterStockApiClient, ShutterStockApiClient>()
                    .BuildServiceProvider();

                var logger = serviceProvider.GetService<ILogger<Program>>();

                logger.LogDebug("Starting application");

                //do the actual work here
                var client = serviceProvider.GetService<IShutterStockApiClient>();

                var folderPath = "H:\\Test";

                var user = client.GetUser();
                logger.LogInformation(user.Username);

                var subscriptions = client.GetSubscriptions();
                logger.LogInformation(subscriptions.Data.FirstOrDefault()?.Id);

                var startDir = new DirectoryInfo(folderPath);

                var recurseFileStructure = new RecurseFileStructure();
                recurseFileStructure.TraverseDirectory(startDir, client);

                Console.WriteLine("Press ANY key to exit");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
