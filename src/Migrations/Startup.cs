using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Migrations;

public class Startup
{
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var sqlSettings = new SqlSettings();
        Configuration.GetSection(SqlSettings.SettingsSection).Bind(sqlSettings);
        
        services.AddDbContext<MigrationsDbContext>((options) =>
        {
            options.UseSqlServer(sqlSettings.ConnectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });
    }
}