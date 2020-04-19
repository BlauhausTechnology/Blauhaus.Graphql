using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using Blauhaus.Graphql.StrawberryShake.TestHelpers;
using Blauhaus.Graphql.Tests.Suts;
using Blauhaus.Graphql.Tests.TestObjects;
using Moq;
using NUnit.Framework;
using Error = Blauhaus.Common.ValueObjects.Errors.Error;

namespace Blauhaus.Graphql.Tests.Tests.StrawberryShakeTests.MutationClientHandlerTests
{
    public class BaseMutationClientHandlerTests 
    {
        [Test]
        public async Task IF_Mutation_executes_successfully_SHOULD_return_dto()
        {
            //Arrange
            var sut = new TestMutationClientHandler()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestResponse>()
                    .With(x => x.Data, new TestResponse
                    {
                        Dto = new TestDto
                        {
                            Name = "Fred!"
                        }
                    }).Object);

            //Act
            var result = await sut.ExecuteAsync(new TestCommandInput(), CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Fred!", result.Value.Name);
        }

        [Test]
        public void IF_Mutation_fails_with_Exception_SHOULD_throw_exception()
        {
            //Arrange
            var sut = new TestMutationClientHandler()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestResponse>()
                    .With_Exception(new Exception("oops")).Object);

            //Act
            Assert.ThrowsAsync<Exception>(async () => await sut.ExecuteAsync(new TestCommandInput(), CancellationToken.None), "oops");

        }

        [Test]
        public void IF_Mutation_fails_with_non_Error_error_SHOULD_throw()
        {
            //Arrange
            var sut = new TestMutationClientHandler()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestResponse>()
                    .With_Error("oops").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await sut.ExecuteAsync(new TestCommandInput(), CancellationToken.None), "oops");
        }

        [Test]
        public void IF_Mutation_fails_with_IError_with_message_extension_SHOULD_use_it_as_message()
        {
            //Arrange
            var sut = new TestMutationClientHandler()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestResponse>()
                    .WithExtension("message", "underlying errsor message").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await sut.ExecuteAsync(new TestCommandInput(), CancellationToken.None));
        }

        [Test]
        public void IF_Mutation_fails_with_multiple_non_Error_error_SHOULD_throw()
        {
            //Arrange
            var sut = new TestMutationClientHandler()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestResponse>()
                    .With_Error("oops").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await sut.ExecuteAsync(new TestCommandInput(), CancellationToken.None));
        }

        [Test]
        public async Task IF_Mutation_fails_with_Error_SHOULD_return_failure()
        {
            //Arrange
            var error = Error.Create("Bad Thing");
            var sut = new TestMutationClientHandler()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestResponse>()
                    .With_Error(error.ToString()).Object);

            //Act
            var result = await sut.ExecuteAsync(new TestCommandInput(), CancellationToken.None);

            //Assert
            Assert.AreEqual(error.ToString(), result.Error);
        }

    }
}