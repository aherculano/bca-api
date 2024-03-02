using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Settings;

[ExcludeFromCodeCoverage]
public class SqlSettings
{
    public const string SettingsSection = "SqlSettings";
    
    public string ConnectionString { get; set; }
}