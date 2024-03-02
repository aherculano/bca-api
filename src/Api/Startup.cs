using System.Text.Json.Serialization;
using Application;
using Infrastructure.Settings;
using Microsoft.OpenApi.Models;

namespace Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureSettings(services);
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        services.AddRouting();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BCA-Auction-API", Version = "v1" });
        });
        services.ConfigureApplication();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
    
    private void ConfigureSettings(IServiceCollection services)
    {
        var sqlSettings = new SqlSettings();
        Configuration.GetSection(SqlSettings.SettingsSection).Bind(sqlSettings);
        services.AddSingleton(sqlSettings);
    }
}