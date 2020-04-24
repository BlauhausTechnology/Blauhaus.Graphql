using System;
using System.Linq.Expressions;
using System.Threading;
using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Server;
using Blauhaus.Common.ValueObjects.Errors;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using Moq;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders
{
    public abstract class BaseAuthenticatedCommandHandlerMockBuilder<TBuilder, TMock, TPayload, TCommand, TUser> : BaseMockBuilder<TBuilder, TMock>
        where TMock : class, ICommandServerHandler<TPayload, TCommand, TUser>
        where TBuilder : BaseAuthenticatedCommandHandlerMockBuilder<TBuilder, TMock, TPayload, TCommand, TUser> 
    {
        public TBuilder Where_HandleAsync_returns(Result<TPayload> value)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(value);
            return this as TBuilder;
        }
        public TBuilder Where_HandleAsync_returns(TPayload payload)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(payload));
            return this as TBuilder;
        }
        public TBuilder Where_HandleAsync_returns_error(string errorMessage)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<TPayload>(errorMessage));
            return this as TBuilder;
        }
        public TBuilder Where_HandleAsync_returns_error(Error error)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<TPayload>(error.ToString()));
            return this as TBuilder;
        }
        
        public TBuilder Where_HandleAsync_throws(Exception exception)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
            return this as TBuilder;
        }

        public void VerifyHandleCalledWithCommandProperty(Expression<Func<TCommand, bool>> expression)
        {
            Mock.Verify(x => x.HandleAsync(It.Is<TCommand>(expression), It.IsAny<TUser>(), It.IsAny<CancellationToken>()));
        }
        public void VerifyHandleCalledWithUserProperty(Expression<Func<TUser, bool>> expression)
        {
            Mock.Verify(x => x.HandleAsync(It.IsAny<TCommand>(), It.Is<TUser>(expression), It.IsAny<CancellationToken>()));
        }
    }
}