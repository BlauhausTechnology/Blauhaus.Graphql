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
        private readonly IGraphqlClientConfig _config;

        public HttpClientHolder(
            IAuthenticatedAccessToken authenticatedAccessToken,
            IAnalyticsService analyticsService,
            IGraphqlClientConfig config)
        {
            _authenticatedAccessToken = authenticatedAccessToken;
            _analyticsService = analyticsService;
            _config = config;
        }

        public HttpClient CreateClient(string name)
        {
            //todo we have to make new clients all the time because we cannot change default request headers after a client has been used.
            //todo a better approach for analytics might be to add the properties to the dto itself

            var httpClient = new HttpClient {BaseAddress = new Uri(_config.GraphqlEndpoint)};
            httpClient.DefaultRequestHeaders.Clear();

            foreach (var appInsightsAnalyticsOperationHeader in _analyticsService.AnalyticsOperationHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(appInsightsAnalyticsOperationHeader.Key, appInsightsAnalyticsOperationHeader.Value);
            }

            foreach (var accessTokenAdditionalHeader in _authenticatedAccessToken.AdditionalHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(accessTokenAdditionalHeader.Key, accessTokenAdditionalHeader.Value);
            }

            if (!string.IsNullOrEmpty(_authenticatedAccessToken.Scheme))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authenticatedAccessToken.Scheme, _authenticatedAccessToken.Token);
            }

            return httpClient;
        }
    }
}