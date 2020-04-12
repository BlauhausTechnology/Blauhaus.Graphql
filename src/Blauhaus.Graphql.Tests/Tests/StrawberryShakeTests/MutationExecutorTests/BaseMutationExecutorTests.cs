using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.TestHelpers;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.Graphql.Tests.Tests._Base;
using NUnit.Framework;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.Tests.StrawberryShakeTests.MutationExecutorTests
{
    public class BaseMutationExecutorTests 
    {
        [Test]
        public async Task IF_Mutation_executes_successfully_SHOULD_return_result()
        {
            //Arrange
            var sut = new TestMutationExecutor()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestPayload>()
                    .With(x => x.Data, new TestPayload
                    {
                        Name = "Fred!"
                    }).Object);

            //Act
            var result = await sut.ExecuteAsync(new TestOperation(), CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Fred!", result.Value.Name);
        }

        [Test]
        public void IF_Mutation_fails_with_Exception_SHOULD_throw_exception()
        {
            //Arrange
            var sut = new TestMutationExecutor()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestPayload>()
                    .With_Exception(new Exception("oops")).Object);

            //Act
            Assert.ThrowsAsync<Exception>(async () => await sut.ExecuteAsync(new TestOperation(), CancellationToken.None),
                "oops");

        }

        [Test]
        public async Task IF_Mutation_fails_without_Exception_SHOULD_return_Failure()
        {
            //Arrange
            var sut = new TestMutationExecutor()
                .Where_GetResultAsync_returns(new OperationResultMockBuilder<TestPayload>()
                    .With_Error("oops").Object);

            //Act
            var result = await sut.ExecuteAsync(new TestOperation(), CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("oops", result.Error);

        }

    }
}