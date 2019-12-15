using System.Collections.Generic;

namespace Blauhaus.Graphql.Abstractions
{
    public interface IGraphqlResult<TPayload>
    {
        TPayload Payload { get; set; }
        List<string> UserErrors { get; set; }
    }
}