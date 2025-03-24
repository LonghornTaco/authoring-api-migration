using AuthoringApi.NetCoreApp.Configuration;
using AuthoringApi.NetCoreApp.Model.AuthoringApi;
using IdentityModel.Client;

namespace AuthoringApi.NetCoreApp.Services
{
    public class IdServerTokenManager : ITokenManager
    {
        private readonly IAuthoringApiConfiguration _authoringApiConfiguration;
        private readonly ILogger<SitecoreAuthoringApiService> _logger;
        private IdServerTokenInfo _currentToken;

        public IdServerTokenManager(IAuthoringApiConfiguration authoringApiConfiguration, ILogger<SitecoreAuthoringApiService> logger)
        {
            _authoringApiConfiguration = authoringApiConfiguration ?? throw new ArgumentNullException(nameof(authoringApiConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetAccessToken()
        {
            if (_currentToken != null && DateTime.UtcNow < _currentToken.ExpiryTime)
            {
                return _currentToken.AccessToken;  // Token is still valid
            }

            if (_currentToken != null && !string.IsNullOrEmpty(_currentToken.RefreshToken))
            {
                _currentToken = RefreshToken(_currentToken.RefreshToken);  // Attempt to refresh the token
            }
            else
            {
                _currentToken = RequestNewToken();  // Request a new token
            }

            return _currentToken.AccessToken;
        }

        private IdServerTokenInfo RequestNewToken()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_authoringApiConfiguration.IdentityServerUrl);

                var passwordRequest = new PasswordTokenRequest
                {
                    Address = "/connect/token",
                    ClientId = "SitecorePassword",
                    ClientSecret = _authoringApiConfiguration.IdentityServerClientSecret,
                    Scope = "openid sitecore.profile sitecore.profile.api",
                    GrantType = IdentityModel.OidcConstants.GrantTypes.Password,
                    UserName = _authoringApiConfiguration.ToBeRemovedUsername,
                    Password = _authoringApiConfiguration.ToBeRemovedPassword
                };

                _logger.LogDebug($"SitecoreAuthoringApiService: Attempting to get a token from {_authoringApiConfiguration.IdentityServerUrl}");
                var tokenResult = httpClient.RequestPasswordTokenAsync(passwordRequest).Result;

                if (tokenResult.IsError)
                {
                    _logger.LogDebug($"ITokenManager: There was a problem getting a token from Identity Server");
                    _logger.LogDebug($"ITokenManager: {tokenResult.Error}");
                    throw new Exception($"ITokenManager: Error requesting token: {tokenResult.Error}");
                }

                _currentToken = new IdServerTokenInfo()
                {
                    AccessToken = tokenResult.AccessToken,
                    RefreshToken = tokenResult.RefreshToken,
                    ExpiryTime = DateTime.UtcNow.AddSeconds(tokenResult.ExpiresIn)
                };
            }

            return _currentToken;
        }

        private IdServerTokenInfo RefreshToken(string refreshToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_authoringApiConfiguration.IdentityServerUrl);

                var refreshRequest = new RefreshTokenRequest
                {
                    Address = "/connect/token",
                    ClientId = "SitecorePassword",
                    ClientSecret = _authoringApiConfiguration.IdentityServerClientSecret,
                    RefreshToken = refreshToken
                };

                var tokenResult = httpClient.RequestRefreshTokenAsync(refreshRequest).Result;

                if (tokenResult.IsError)
                {
                    // If refreshing fails, request a new token
                    return RequestNewToken();
                }

                _currentToken = new IdServerTokenInfo
                {
                    AccessToken = tokenResult.AccessToken,
                    RefreshToken = tokenResult.RefreshToken,
                    ExpiryTime = DateTime.UtcNow.AddSeconds(tokenResult.ExpiresIn)
                };
            }

            return _currentToken;
        }
    }
}
