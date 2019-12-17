using System.Collections.Generic;

namespace Blauhaus.Graphql.Abstractions.GraphqlResults
{
    public interface IGraphqlResult<TPayload>
    {
        TPayload Payload { get; set; }
        List<GraphqlError> Errors { get; set; }
    }
}