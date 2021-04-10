using System;
using System.Linq.Expressions;
using System.Threading;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders
{
    public abstract class BaseAuthenticatedCommandHandlerMockBuilder<TBuilder, TMock, TPayload, TCommand, TUser> : BaseMockBuilder<TBuilder, TMock>
        where TMock : class, IAuthenticatedCommandHandler<TPayload, TCommand, TUser>
        where TBuilder : BaseAuthenticatedCommandHandlerMockBuilder<TBuilder, TMock, TPayload, TCommand, TUser> 
    {
        public TBuilder Where_HandleAsync_returns(Response<TPayload> value)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>()))
                .ReturnsAsync(value);
            return this as TBuilder;
        }
        public TBuilder Where_HandleAsync_returns(TPayload payload)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>()))
                .ReturnsAsync(Response.Success(payload));
            return this as TBuilder;
        }
        public TBuilder Where_HandleAsync_returns_error(string errorMessage)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>()))
                .ReturnsAsync(Response.Failure<TPayload>(errorMessage));
            return this as TBuilder;
        }
        public TBuilder Where_HandleAsync_returns_error(Error error)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>()))
                .ReturnsAsync(Response.Failure<TPayload>(error.ToString()));
            return this as TBuilder;
        }
        
        public TBuilder Where_HandleAsync_throws(Exception exception)
        {
            Mock.Setup(x => x.HandleAsync(It.IsAny<TCommand>(), It.IsAny<TUser>()))
                .ThrowsAsync(exception);
            return this as TBuilder;
        }

        public void VerifyHandleCalledWithCommandProperty(Expression<Func<TCommand, bool>> expression)
        {
            Mock.Verify(x => x.HandleAsync(It.Is<TCommand>(expression), It.IsAny<TUser>()));
        }
        public void VerifyHandleCalledWithUserProperty(Expression<Func<TUser, bool>> expression)
        {
            Mock.Verify(x => x.HandleAsync(It.IsAny<TCommand>(), It.Is<TUser>(expression)));
        }
    }
}