namespace AuthoringApi.NetCoreApp.Configuration
{
    public class SitecoreAuthoringApiConfiguration : NetCoreAppConfiguration, IAuthoringApiConfiguration
    {
        protected override string SectionName => "AuthoringApi";

        public string IdentityServerUrl => GetConfigurationValue("IdentityServerUrl");
        public string IdentityServerClientSecret => GetConfigurationValue("IdentityServerClientSecret");
        public string AuthoringApiUrl => GetConfigurationValue("AuthoringApiUrl");
        public string ToBeRemovedUsername => GetConfigurationValue("ToBeRemovedUsername");
        public string ToBeRemovedPassword => GetConfigurationValue("ToBeRemovedPassword");
        public string MasterIndexName => GetConfigurationValue("MasterIndexName");
        public string SxaMasterIndexName => GetConfigurationValue("SxaMasterIndexName");

        public SitecoreAuthoringApiConfiguration(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
