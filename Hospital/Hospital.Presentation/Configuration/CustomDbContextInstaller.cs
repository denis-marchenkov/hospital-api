using Hospital.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Presentation.Configuration
{
    public class CustomDbContextInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["DbType:Type"] == "sql")
            {
                var connectionString = configuration.GetConnectionString("Default");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryDb"));
            }
        }
    }
}
