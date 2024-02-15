using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductAPI.Data;

namespace ProductAPI
{
    /// <summary>
    /// Class responsible for configuring the application services and middleware.
    /// </summary>
    /// <param name="configuration">
    /// Represents a set of key/value application configuration properties.
    /// </param>
    public class Startup(IConfiguration configuration)
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        private IConfiguration Configuration { get; } = configuration;

        /// <summary>
        /// Method that configures the application services.
        /// </summary>
        /// <param name="services">
        /// Represents a collection of service descriptors.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<DataSeeder>();

            services.AddControllers();
        }
        
        /// <summary>
        /// Method that configures the application middleware.
        /// </summary>
        /// <param name="app">
        /// Represents a class that provides the mechanisms to configure an application's request pipeline.
        /// </param>
        /// <param name="env">
        /// Represents a class that provides information about the web hosting environment an application is running in.
        /// </param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Testing")){
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            dbContext.Database.EnsureCreated();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
                c.RoutePrefix = string.Empty; 
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}