namespace AuthoringApi.NetCoreApp.Configuration
{
    public interface IAuthoringApiConfiguration
    {
        string IdentityServerUrl { get; }
        string IdentityServerClientSecret { get; }
        string AuthoringApiUrl { get; }
        string ToBeRemovedUsername { get; }
        string ToBeRemovedPassword { get; }
        string MasterIndexName { get; }
        string SxaMasterIndexName { get; }
    }
}
