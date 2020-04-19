using System;
using System.Collections.Generic;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.Exceptions
{
    public class GraphqlException : Exception
    {
        public GraphqlException(IError error) 
            : base(error.Extensions != null && error.Extensions.ContainsKey("message") ? error.Extensions["message"].ToString() : error.Message)
        {
            Extensions = error.Extensions;
        }

        public IReadOnlyDictionary<string, object?>? Extensions { get; }
    }
}