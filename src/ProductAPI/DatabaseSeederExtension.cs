using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductAPI.Data;

namespace ProductAPI
{
    /// <summary>
    /// A class that represents the extension for the IHost interface, used to seed the database with the initial data.
    /// </summary>
    public static class DatabaseSeederExtension
    {
        /// <summary>
        /// A method that seeds the database with the initial data.
        /// </summary>
        /// <param name="host">
        /// The instance of the host for the application.
        /// </param>
        /// <returns>
        /// The instance of the host for the application.
        /// </returns>
        public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
               
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");
                var context = services.GetRequiredService<ApplicationDbContext>();
                try
                {
                    var seeder = services.GetRequiredService<DataSeeder>();
                    seeder.SeedCategories(context);
                    seeder.SeedProducts(context);
                    logger.LogInformation("Database seeding completed.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            return host;
        }
    }
}