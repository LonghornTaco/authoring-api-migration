using AuthoringApi.NetCoreApp.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using AuthoringApi.NetCoreApp.Model.AuthoringApi;

namespace AuthoringApi.NetCoreApp.Services
{
    public abstract class ApiService
    {
        protected readonly IAuthoringApiConfiguration AuthoringApiConfiguration;
        private readonly ITokenManager _tokenManager;

        private const string GRAPHQL_ENDPOINT = "/sitecore/api/authoring/graphql/v1";

        protected ApiService(IAuthoringApiConfiguration authoringApiConfiguration, ITokenManager tokenManager)
        {
            AuthoringApiConfiguration = authoringApiConfiguration ?? throw new ArgumentNullException(nameof(authoringApiConfiguration));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        protected T ExecuteQuery<T>(string query) where T : IApiResponse
        {
            var token = _tokenManager.GetAccessToken();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AuthoringApiConfiguration.AuthoringApiUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var requestPayload = new
                {
                    query = query
                };

                var jsonPayload = JObject.FromObject(requestPayload).ToString();
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = client.PostAsync(GRAPHQL_ENDPOINT, content).Result;

                var responseContent = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(responseContent);

                if ((result.Errors?.Count() ?? 0) > 0)
                {
                    throw new Exception($"Error: {string.Join(" | ", result.Errors.Select(x => x.Message))} \n\n Query: {query}");
                }
                return result;
            }
        }
    }
}
