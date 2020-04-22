using System;
using System.Linq;
using HotChocolate.Execution;
using NUnit.Framework;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.Extensions
{
    public static class ConnectionResultExtensions
    {
        
        public static OrderedDictionary FindEdgeNodeByEntityId(this ConnectionResult connectionResult, Guid entityId)
        {
            foreach (var edge in connectionResult.Edges)
            {
                var keyValuePair = edge.Node.FirstOrDefault(x => x.Key.ToLowerInvariant() == "entityid");
                if (!string.IsNullOrEmpty(keyValuePair.Key) && Guid.Parse(keyValuePair.Value.ToString()) == entityId)
                    return edge.Node;
            }
            Assert.Fail("EntityId was not found on this object");
            return null;
        }

    }
}