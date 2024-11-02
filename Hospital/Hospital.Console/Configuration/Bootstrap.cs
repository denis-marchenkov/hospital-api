using Hospital.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hospital.Console.Configuration
{
    internal static class Bootstrap
    {
        public static IConfiguration GetConfiguration()
        {

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static IHost GetHost(IConfiguration configuration, string[] args)
        {
            var envConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
            var connectionString = string.IsNullOrEmpty(envConnectionString) ?
                                        configuration.GetConnectionString("Default") :
                                        envConnectionString;

            var host = Host.CreateDefaultBuilder(args)
                           .ConfigureServices((context, services) =>
                           {
                               services.AddDbContext<AppDbContext>(options =>
                                   options.UseSqlServer(connectionString));
                           })
                           .Build();

            return host;
        }

        public static IServiceProvider GetServices()
        {
            var serviceCollection = new ServiceCollection();

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Ignore SSL errors
            };

            serviceCollection.AddHttpClient("UnsafeHttpClient")
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        public static async Task Run(IConfiguration configuration, IHost host, IServiceProvider serviceProvider)
        {
            bool isRunning = true;

            try
            {
                while (isRunning)
                {
                    System.Console.WriteLine(
                        "List of available commands\n" +
                        "\n\n-s [amount] to seed database. Example: -s 100" +
                        "\n\n-u [url] to specify api url for creating patient. Example -u https://localhost:5000/patients" +
                        "\n\n-c to clear database." +
                        "\n\n-x to exit." +
                        "\n\nCommand:"
                    );

                    var userInput = System.Console.ReadLine();

                    var options = CmdOptions.Parse(userInput);

                    if (options.Exit)
                    {
                        isRunning = false;
                        continue;
                    }

                    if (options.Seed > 0)
                    {
                        Seed.ClearDb(host);
                        if (!string.IsNullOrEmpty(options.Url))
                        {
                            await Seed.SeedDbApi(serviceProvider, options);
                        }
                        else
                        {
                            Seed.SeedDbLocal(host, configuration, options);
                        }
                    }

                    if (options.Clear)
                    {
                        Seed.ClearDb(host);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
