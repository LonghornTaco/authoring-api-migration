using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace AuthoringApi.Webhook.Services
{
    public class SitecoreAuthoringApi : IAuthoringApi
    {
        private string graphqlEndpoint = "http://cm/sitecore/api/authoring/graphql/v1";

        public string ExecuteQuery(string query)
        {
            var token = GetToken();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var requestPayload = new 
                { 
                    query = query 
                };

                var jsonPayload = JObject.FromObject(requestPayload).ToString();
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = client.PostAsync(graphqlEndpoint, content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}\n{responseContent}");
                }
                return responseContent;
            }
        }
        private string GetToken()
        {
            var token = string.Empty;
            var identityServer = "http://id";
            var clientSecret = "sk2KUhr1vaGPYaLGzx9ozXVnQ7YUm8rCPlw2A3QAl9yER6s5GCxTvdKRgDHhcDQV";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(identityServer);

                var passwordRequest = new PasswordTokenRequest()
                {
                    Address = "/connect/token",
                    ClientId = "SitecorePassword",
                    ClientSecret = clientSecret,
                    GrantType = IdentityModel.OidcConstants.GrantTypes.Password,
                    Scope = "openid sitecore.profile sitecore.profile.api",
                    UserName = @"sitecore\admin",
                    Password = "b"
                };

                var tokenResult = httpClient.RequestPasswordTokenAsync(passwordRequest).Result;
                token = tokenResult?.AccessToken ?? string.Empty;
            }

            return token;
        }
    }
}
