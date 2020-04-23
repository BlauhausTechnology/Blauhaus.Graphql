//using System;
//using System.Linq.Expressions;
//using System.Threading;
//using Blauhaus.Graphql.StrawberryShake.Executors;
//using Blauhaus.Graphql.StrawberryShake.MutationHandlers;
//using Blauhaus.TestHelpers.MockBuilders;
//using CSharpFunctionalExtensions;
//using Moq;
//using StrawberryShake;

//namespace Blauhaus.Graphql.StrawberryShake.TestHelpers
//{
//    public abstract class BaseMutationExecutorMockBuilder<TBuilder, TMock, TDto, TCommandInput> : BaseMockBuilder<TBuilder, TMock> 
//        where TMock : class, IMutationClientHandler<TDto, TCommandInput> 
//        where TBuilder : BaseMutationExecutorMockBuilder<TBuilder, TMock, TDto, TCommandInput>

//    {

//        public TBuilder Where_execute_returns_result(TDto result)
//        {
//            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TCommandInput>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(Result.Success(result));
//            return this as TBuilder;
//        }

//        public TBuilder Where_execute_returns_result(Result<TDto> result)
//        {
//            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TCommandInput>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(result);
//            return this as TBuilder;
//        }
        
//        public TBuilder Where_execute_returns_error(string errorMessage)
//        {
//            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TCommandInput>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(Result.Failure<TDto>(errorMessage));
//            return this as TBuilder;
//        }

//        public TBuilder Where_execute_throws_exception(Exception e)
//        {
//            Mock.Setup(x => x.ExecuteAsync(It.IsAny<TCommandInput>(), It.IsAny<CancellationToken>()))
//                .ThrowsAsync(e);
//            return this as TBuilder;
//        }

//        public TBuilder Verify_Execute_called_with<TProperty>(Expression<Func<TCommandInput, bool>> expression)
//        {
//            Mock.Verify(x => x.ExecuteAsync(It.Is<TCommandInput>(expression), It.IsAny<CancellationToken>()));
//            return this as TBuilder;
//        }
//    }
//}