using Hospital.Application.Infrastructure;
using Hospital.Application.Patients.Queries.List;
using Hospital.Domain.Contracts;
using Hospital.Domain.Search;
using Hospital.Persistence.Repository;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Hospital.Presentation.Configuration
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hospital API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                else
                {
                    Console.WriteLine($"XML documentation file not found: {xmlPath}");
                }
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ISearchFilterProvider, SearchFilterProvider>();
            services.AddSingleton<IQueryStringParser, QueryStringParser>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ListPatientsQuery).Assembly));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }
    }
}
