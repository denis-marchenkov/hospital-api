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

        public static void Run(IConfiguration configuration, IHost host)
        {
            bool isRunning = true;

            try
            {
                while (isRunning)
                {
                    System.Console.WriteLine($"\n\nType -s [amount] to seed database. Example: -s 100\n\nType -c to clear database.\n\nType -x to exit.\n\nCommand:");

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
                        Seed.SeedDb(host, configuration, options.Seed);
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
