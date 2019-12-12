using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Blauhaus.Graphql.Generator.Runner
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var config = new GraphqlGeneratorConfig
            {
                DestinationPath = "C:\\Code\\",
                Namespace = "Blauhaus.Graphql.Generator.Runner",
                SchemaUrl = "https://reveye-development-graphapi.azurewebsites.net/api/graphql",
                AuthenticationHeander = new KeyValuePair<string, string>("Bearer", "token"),
                HttpHeaders = new Dictionary<string, string> {{"X-Auth-AdminKey", "1f57e85c-7ac6-4923-9bbd-2d4fc4d1fd00"}},
                FieldTypeMappings = new Dictionary<string, string> {{"DateTime", "DateTime?"}}
            };

            await config.GeneratAsync();
        }

     


    }
}
