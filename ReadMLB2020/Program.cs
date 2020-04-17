using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ReadMLB2020
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            // Create service collection and configure our services
            var services = ConfigureServices();
            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();

            // Kick off our actual code
            serviceProvider.GetService<ReadMLBApp>().RunAsync(args).Wait();

           
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            //services.AddTransient<ITestService, TestService>();

            // Set up the objects we need to get to configuration settings
            var config = LoadConfiguration();
            // Add the config to our DI container for later user
            services.AddSingleton(config);


            ReadMLB.Services.Startup.ConfigureServices(services, config);

            // IMPORTANT! Register our application entry point
            services.AddSingleton<FindPlayer>();
            services.AddSingleton<ReadMLBApp>();
            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true,
                             reloadOnChange: true);
            return builder.Build();
        }
    }
   
}
