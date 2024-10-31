using System.Reflection;

namespace Hospital.Presentation.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(
                    type => typeof(IServiceInstaller).IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    !type.IsInterface
                    )
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach (var installer in installers)
            {
                installer.Install(services, configuration);
            }

            return services;
        }
    }
}
