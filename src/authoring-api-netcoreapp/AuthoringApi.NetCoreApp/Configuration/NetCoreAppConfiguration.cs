namespace AuthoringApi.NetCoreApp.Configuration
{
    public abstract class NetCoreAppConfiguration
    {
        protected virtual string SectionName { get; }

        private readonly IConfiguration _configuration;
        private IConfigurationSection ConfigurationSection => _configuration.GetSection(SectionName);

        protected NetCoreAppConfiguration(IConfiguration configuration) => _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        protected string GetConfigurationValue(string key) => ConfigurationSection[key] ?? string.Empty;
    }
}
