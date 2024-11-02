using Bogus;
using Hospital.Application.Patients.Commands.Create;
using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using Hospital.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace Hospital.Console.Configuration
{
    internal static class Seed
    {
        private static List<(DateTime, Guid)> _testData;

        static Seed()
        {
            // predetermined dates and ids for easier api testing
            _testData = ReadTestData();
        }

        static Func<Faker, int, DateTime> GetDate = (Faker fake, int i) =>
        {
            if (_testData != null && i < _testData.Count)
            {
                try
                {
                    return _testData[i].Item1;
                }
                catch { }
            }

            return fake.Date.Past(30);
        };

        static Func<int, PatientId> GetId = (int i) =>
        {
            if (_testData != null && i < _testData.Count)
            {
                return new PatientId(_testData[i].Item2);
            }

            return PatientId.New();
        };

        public static void SeedDbLocal(IHost host, IConfiguration configuration, CmdOptions options)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var patients = new ConcurrentBag<Patient>();
                    var names = new ConcurrentBag<Name>();
                    var givenNames = new ConcurrentBag<GivenName>();

                    Parallel.For(0, options.Seed, i =>
                    {
                        var patient = new Faker<Patient>()
                            .RuleFor(p => p.Id, f => GetId(i))
                            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                            .RuleFor(p => p.BirthDate, f => GetDate(f, i))
                            .RuleFor(p => p.Active, f => f.Random.Bool())
                            .Generate();

                        patients.Add(patient);


                        var name = new Faker<Name>()
                            .RuleFor(n => n.Id, f => Guid.NewGuid())
                            .RuleFor(n => n.PatientId, f => patient.Id)
                            .RuleFor(n => n.Use, f => f.Name.Prefix())
                            .RuleFor(n => n.Family, f => f.Name.LastName())
                            .Generate();

                        names.Add(name);


                        for (int j = 0; j < new Random().Next(1, 4); j++)
                        {
                            var givenName = new Faker<GivenName>()
                                .RuleFor(g => g.Id, f => Guid.NewGuid())
                                .RuleFor(g => g.NameId, f => name.Id)
                                .RuleFor(g => g.Value, f => f.Name.FirstName())
                            .Generate();

                            givenNames.Add(givenName);
                        }
                    });

                    context.Patients.AddRange(patients);
                    context.Names.AddRange(names);
                    context.GivenNames.AddRange(givenNames);

                    context.SaveChanges();

                    System.Console.WriteLine("\n\nDatabase seeded.\n\n");
                }
                catch (AggregateException ex)
                {
                    foreach (var inner in ex.InnerExceptions)
                    {
                        System.Console.WriteLine($"An error occurred: {inner.Message}");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static async Task SeedDbApi(IServiceProvider serviceProvider, CmdOptions options)
        {
            var commands = new ConcurrentBag<CreatePatientCommand>();

            Parallel.For(0, options.Seed, i =>
            {
                var command = new Faker<CreatePatientCommand>()
                            .RuleFor(p => p.Id, f => GetId(i).Value)
                            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                            .RuleFor(p => p.BirthDate, f => GetDate(f, i))
                            .RuleFor(p => p.Active, f => f.Random.Bool())
                            .RuleFor(n => n.Use, f => f.Name.Prefix())
                            .RuleFor(n => n.Family, f => f.Name.LastName())
                            .RuleFor(n => n.GivenNames, f => f.Lorem.Words(f.Random.Int(1, 3)))
                            .Generate();

                commands.Add(command);
            });


            using (var scope = serviceProvider.CreateScope())
            {
                var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("UnsafeHttpClient");

                var tasks = commands.Select(async patient =>
                {
                    var seedResponse = await httpClient.PostAsJsonAsync(options.Url, patient);
                    if (!seedResponse.IsSuccessStatusCode)
                    {
                        System.Console.WriteLine($"Error seeding patient: {seedResponse.StatusCode}");
                    }
                });

                await Task.WhenAll(tasks);

                System.Console.WriteLine("\n\nDatabase seeded.\n\n");
            }
        }

        public static void ClearDb(IHost host)
        {
            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    context.Patients.RemoveRange(context.Patients);

                    context.SaveChanges();

                    System.Console.WriteLine("\n\nDatabase cleared.\n\n");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static List<(DateTime, Guid)> ReadTestData()
        {
            var result = new List<(DateTime, Guid)>();
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "patients_date_guid.txt");
                var data = File.ReadAllLines(filePath).ToList();

                foreach (var line in data)
                {
                    var pair = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var date = DateTime.TryParse(pair[0], out var tempDate) ? tempDate : DateTime.Now;
                    var guid = Guid.TryParse(pair[1], out var tempGuid) ? tempGuid : Guid.NewGuid();

                    result.Add((date, guid));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }

            return result;
        }
    }
}
