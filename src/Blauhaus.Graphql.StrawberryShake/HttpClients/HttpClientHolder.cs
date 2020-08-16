using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.AccessToken;
using Blauhaus.Graphql.StrawberryShake.Config;

namespace Blauhaus.Graphql.StrawberryShake.HttpClients
{
    public class HttpClientHolder : IHttpClientFactory
    {
        private readonly IAuthenticatedAccessToken _authenticatedAccessToken;
        private readonly IAnalyticsService _analyticsService;
        private readonly HttpClient _httpClient;

        public HttpClientHolder(
            IAuthenticatedAccessToken authenticatedAccessToken,
            IAnalyticsService analyticsService,
            IGraphqlClientConfig config)
        {
            _authenticatedAccessToken = authenticatedAccessToken;
            _analyticsService = analyticsService;
            _httpClient = new HttpClient {BaseAddress = new Uri(config.GraphqlEndpoint)};
        }

        public HttpClient CreateClient(string name)
        {
            
            _httpClient.DefaultRequestHeaders.Clear();

            foreach (var appInsightsAnalyticsOperationHeader in _analyticsService.AnalyticsOperationHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(appInsightsAnalyticsOperationHeader.Key, appInsightsAnalyticsOperationHeader.Value);
            }

            foreach (var accessTokenAdditionalHeader in _authenticatedAccessToken.AdditionalHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(accessTokenAdditionalHeader.Key, accessTokenAdditionalHeader.Value);
            }

            if (!string.IsNullOrEmpty(_authenticatedAccessToken.Scheme))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authenticatedAccessToken.Scheme, _authenticatedAccessToken.Token);
            }

            return _httpClient;
        }
    }
}