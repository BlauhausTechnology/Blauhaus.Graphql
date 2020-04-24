using System;
using System.Threading;
using Blauhaus.Graphql.HotChocolate.MutationHandlers;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base;
using Blauhaus.TestHelpers.MockBuilders;
using HotChocolate.Resolvers;
using Moq;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders
{
    public class MutationServerHandlerMockBuilder : BaseMockBuilder<MutationServerHandlerMockBuilder, IMutationServerHandler>
    {
        public MutationServerHandlerMockBuilder Where_HandleAsync_returns<TCommand, TPayload>(TPayload payload)
        {
            Mock.Setup(x => x.HandleAsync<TPayload, TCommand>(It.IsAny<IResolverContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(payload);
            return this;
        }
        public MutationServerHandlerMockBuilder Where_HandleAsync_throws<TCommand, TPayload>(Exception e)
        {
            Mock.Setup(x => x.HandleAsync<TPayload, TCommand>(It.IsAny<IResolverContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(e);
            return this;
        }
    }
}