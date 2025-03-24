using AuthoringApi.NetCoreApp.Model.ManagementApi;
using AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Indexes;
using AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Jobs;

namespace AuthoringApi.NetCoreApp.Services
{
    public interface IManagementApiService
    {
        ManagementApiResponse<QueryIndexResponse> GetIndexes();
        RebuildIndexResponse RebuildMasterIndexes();
        RebuildIndexResponse RebuildIndexes(string[] indexes);
        JobStatusResponse CheckJobStatus(string jobName);
    }
}
