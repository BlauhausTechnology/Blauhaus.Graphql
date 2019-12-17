using System;
using System.Collections.Generic;
using System.Text;

namespace Blauhaus.Graphql.Abstractions.GraphqlResults
{
    public class GraphqlError
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
