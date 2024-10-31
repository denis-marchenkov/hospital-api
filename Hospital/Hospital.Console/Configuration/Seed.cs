using Bogus;
using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using Hospital.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace Hospital.Console.Configuration
{
    internal static class Seed
    {
        public static void SeedDb(IHost host, IConfiguration configuration, int amount = 100)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var patients = new ConcurrentBag<Patient>();
                    var names = new ConcurrentBag<Name>();
                    var givenNames = new ConcurrentBag<GivenName>();

                    Parallel.For(0, amount, i =>
                    {
                        var patient = new Faker<Patient>()
                            .RuleFor(p => p.Id, f => PatientId.New())
                            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                            .RuleFor(p => p.BirthDate, f => f.Date.Past(30))
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

                    System.Console.WriteLine("\n\nDatabase seeded.");
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

        public static void ClearDb(IHost host)
        {
            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    context.Patients.RemoveRange(context.Patients);

                    context.SaveChanges();

                    System.Console.WriteLine("\n\nDatabase cleared.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
