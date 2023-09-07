
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.FAA.Web;

public class Startup
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfigurationRoot _configuration;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;
        var config = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.Development.json", true)
#endif
            .AddEnvironmentVariables();

        if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            config.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["Environment"];
                    options.PreFixConfigurationKeys = false;
                }
            );
        }

        _configuration = config.Build();
    }
}