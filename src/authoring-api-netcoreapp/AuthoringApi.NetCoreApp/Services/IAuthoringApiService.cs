using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Create;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Search;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Update;

namespace AuthoringApi.NetCoreApp.Services
{
    public interface IAuthoringApiService
    {
        QueryItemResponse QueryItem(string itemPath);
        UpdateItemResponse UpdateItem(string input);
        CreateItemResponse CreateItem(string input);
    }
}
