using Hospital.Application.Patients.Queries.List;
using Hospital.Domain.Contracts;
using Hospital.Persistence.Repository;
using Microsoft.OpenApi.Models;

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
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ListPatientsQuery).Assembly));
        }
    }
}
