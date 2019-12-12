using System.Collections.Generic;

namespace Blauhaus.Graphql.Generator
{
    public class GraphqlGeneratorConfig
    {
        public bool GenerateSchema { get; set; }
        public string SchemaUrl { get; set; }
        public string Namespace { get; set; }
        public string DestinationPath { get; set; }
        public KeyValuePair<string, string> AuthenticationHeander { get; set; } = new KeyValuePair<string, string>();
        public Dictionary<string, string>HttpHeaders { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> FieldTypeMappings { get; set; } = new Dictionary<string, string>();
    }
}