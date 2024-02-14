using Gateway.Middleware;
using Microsoft.OpenApi.Models;

namespace Gateway;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient("ProductsAPI", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5095/"); 
        });
        services.AddReverseProxy()
            .LoadFromConfig(Configuration.GetSection("ReverseProxy"));
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway API", Version = "v1" });
        });
        services.AddControllers();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseRequestLogging();

        app.UseSwagger();
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API V1");
            c.RoutePrefix = string.Empty; 
        });

        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }


}