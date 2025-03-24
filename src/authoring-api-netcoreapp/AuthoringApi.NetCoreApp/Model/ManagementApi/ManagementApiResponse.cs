using AuthoringApi.NetCoreApp.Model.AuthoringApi;
using AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Indexes;

namespace AuthoringApi.NetCoreApp.Model.ManagementApi
{
    public class ManagementApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
