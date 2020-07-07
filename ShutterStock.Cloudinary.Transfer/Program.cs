using Cloudinary.ApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ShutterStock.ApiClient;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ShutterStock.Cloudinary.Transfer.Util;

namespace ShutterStock.Cloudinary.Transfer
{
    internal class Program
    {
        public static IConfigurationRoot Configuration;
        private static string _shutterStockCode = "";

        private static void Main()
        {
            try
            {
                var serviceProvider = Initialize();

                Log.Information($"---- Start verificatie ----");

                var folderPath = Configuration.GetSection("ServerPath").Value;
                Log.Information($"De ROOT van de directory is '{folderPath}'.");

                CreateServer();

                var client = serviceProvider.GetService<IShutterStockApiClient>();
                Log.Information("ShutterStock gegevens worden opgehaald...");

                client.Authorize();

                while (string.IsNullOrEmpty(_shutterStockCode))
                {
                    Log.Information("Wachten op ShutterStock authorisatie...");
                    Thread.Sleep(5000);
                }

                client.Authenticate(_shutterStockCode);
                var user = client.GetUser();
                Log.Information($"Gebruiker: {user.Username}");

                var subscriptions = client.GetSubscriptions();
                Log.Information($"Subscriptions: {subscriptions.Data.FirstOrDefault()?.Id}");

                Log.Information($"---- Stop verificatie ----");

                var startDir = new DirectoryInfo(folderPath);

                var recurseFileStructure = serviceProvider.GetService<IRecurseFileStructure>();

                recurseFileStructure.TraverseDirectory(startDir, client);
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
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                // Create service provider
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

        private static void CreateServer()
        {
            var tcp = new TcpListener(IPAddress.Any, 3000);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        var stream = tcpClient.GetStream();
                        var bytes = new byte[1024];
                        var i = stream.Read(bytes, 0, bytes.Length);
                        var incoming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        var code = RegexUtil.GetStringBetweenCharacters(incoming, '?', '&');

                        if (!string.IsNullOrEmpty(code))
                        {
                            _shutterStockCode = code.Substring(code.LastIndexOf('=') + 1);
                        }

                        tcpClient.Close();
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
    }
}
