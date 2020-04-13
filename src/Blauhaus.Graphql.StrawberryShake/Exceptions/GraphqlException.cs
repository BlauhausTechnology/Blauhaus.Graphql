using System;
using System.Collections.Generic;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.Exceptions
{
    public class GraphqlException : Exception
    {
        public GraphqlException(IError error) : base(error.Message)
        {

        }
    }
}