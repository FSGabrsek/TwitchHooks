using Microsoft.Extensions.Options;

namespace TwitchHooksAPI.Options;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string CONFIGURATION_SECTION = "DatabaseOptions";
    
    private readonly IConfiguration _configuration;

    private readonly string _host;
    private readonly string _name;
    private readonly string _user;
    private readonly string _password;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
        _host = Environment.GetEnvironmentVariable("DB_HOST") ?? string.Empty;
        _name = Environment.GetEnvironmentVariable("DB_NAME") ?? string.Empty;
        _user = Environment.GetEnvironmentVariable("DB_USER") ?? string.Empty;
        _password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? string.Empty;
        
    }

    public void Configure(DatabaseOptions options)
    {
        var connectionString = $"Host={_host};Database={_name};Username={_user};Password={_password}";
        options.ConnectionString = connectionString;

        _configuration.GetSection(CONFIGURATION_SECTION).Bind(options);
    }
}