using System;
using System.Linq.Expressions;
using System.Threading;
using Blauhaus.Graphql.StrawberryShake.Executors;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using Moq;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.TestHelpers
{
    public abstract class BaseGraphqlOperationExecutorMockBuilder<TBuilder, TMock, TOperation, TResult> : BaseMockBuilder<TBuilder, TMock> 
        where TMock : class, IGraphqlOperationExecutor<TOperation, TResult> 
        where TBuilder : BaseGraphqlOperationExecutorMockBuilder<TBuilder, TMock, TOperation, TResult>
        where TOperation  : IOperation<TResult>
        where TResult : class
    {

        public TBuilder Where_execute_returns_result(TResult result)
        {
            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TOperation>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(result));
            return this as TBuilder;
        }

        public TBuilder Where_execute_returns_result(Result<TResult> result)
        {
            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TOperation>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            return this as TBuilder;
        }
        
        public TBuilder Where_execute_returns_error(string errorMessage)
        {
            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TOperation>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<TResult>(errorMessage));
            return this as TBuilder;
        }

        public TBuilder Where_execute_throws_exception(Exception e)
        {
            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TOperation>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(e);
            return this as TBuilder;
        }

        public TBuilder Verify_Execute_called_with<TProperty>(Expression<Func<TOperation, bool>> expression)
        {
            Mock.Verify(x => x.ExecuteAsync(It.Is<TOperation>(expression), It.IsAny<CancellationToken>()));

            return this as TBuilder;
        }
    }
}