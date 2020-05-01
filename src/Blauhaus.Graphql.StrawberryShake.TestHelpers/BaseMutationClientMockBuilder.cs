using System.Threading;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.TestHelpers
{
    public class MutationClientMockBuilder<TBuilder, TMock, TModelDo, TMutationResult, TCommandDto, TCommand> : BaseMockBuilder<TBuilder, TMock>
        where TMock : class, IGraphqlClient<TModelDo, TMutationResult, TCommandDto, TCommand> 
        where TBuilder : BaseMockBuilder<TBuilder, TMock>
        where TMutationResult : class
        where TModelDo : class
    {
        public TBuilder Where_GetResultAsync_returns(TMutationResult value)
        {
            Mock.Setup(x => x.GetResultAsync(It.IsAny<TCommandDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new OperationResultMockBuilder<TMutationResult>()
                    .With(x => x.Data, value).Object);
            return this as TBuilder;
        }
    }
}