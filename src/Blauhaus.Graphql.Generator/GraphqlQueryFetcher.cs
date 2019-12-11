using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GraphQlClientGenerator;
using Newtonsoft.Json;

namespace Blauhaus.Graphql.Generator
{
    public static class GraphqlQueryFetcher
    {
        public static async Task<GraphQlSchema> RetrieveSchema(string url, HttpClient client)
        {
            using var response =
                await client.PostAsync(url,
                    new StringContent(JsonConvert.SerializeObject(new { query = IntrospectionQuery.Text }), Encoding.UTF8, "application/json"));

            var content =
                response.Content == null
                    ? "(no content)"
                    : await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Status code: {(int)response.StatusCode} ({response.StatusCode}); content: {content}");

            return GraphQlGenerator.DeserializeGraphQlSchema(content);
        }
    }
}