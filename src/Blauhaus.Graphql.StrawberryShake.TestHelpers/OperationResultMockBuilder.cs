using System;
using System.Collections.Generic;
using Blauhaus.TestHelpers.MockBuilders;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.TestHelpers
{
    public class OperationResultMockBuilder<TPayload> : BaseMockBuilder<OperationResultMockBuilder<TPayload>, IOperationResult<TPayload>> where TPayload : class
    {
        public OperationResultMockBuilder()
        {
            With(x => x.Errors, new List<IError>());
        }

        public OperationResultMockBuilder<TPayload> With_Error(string errorMessage)
        {
            With(x => x.Errors, new List<IError>
            {
                new ErrorBuilder()
                    .SetMessage(errorMessage).Build()
            });
            return this;
        }

        public OperationResultMockBuilder<TPayload> With_Exception(Exception exception)
        {
            With(x => x.Errors, new List<IError>
            {
                new ErrorBuilder()
                    .SetMessage("Exception")
                    .SetException(exception).Build()
            });
            return this;
        }
    }
}