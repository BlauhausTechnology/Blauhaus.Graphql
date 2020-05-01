using System;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.TestHelpers;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.BaseTests;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;
using StrawberryShake;
using Error = Blauhaus.Common.ValueObjects.Errors.Error;

namespace Blauhaus.Graphql.Tests.Tests.StrawberryShakeTests
{
    public class ClientQueryHandlerTests : BaseServiceTest<ClientQueryHandler<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>>
    {
        private TestCommandDto _commandDto;
        private IOperationResult<TestGraphqlResponse> _operationResult;
        private TestModelDto _dto;


        private MockBuilder<IGraphqlClient<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>> MockGraphqlClient 
            => AddMock<IGraphqlClient<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>>().Invoke();

        [SetUp]
        public void Setup()
        {
            Cleanup();

            _commandDto = new TestCommandDto{Name = "Command"};
            _dto = new TestModelDto{Name = "Dto"};
            _operationResult = new OperationResultMockBuilder<TestGraphqlResponse>()
                .With(x => x.Data, new TestGraphqlResponse
                {
                    Dto = _dto
                }).Object;

            MockGraphqlClient.Mock.Setup(x => x.GetDtoFromResult(_operationResult)).Returns(_dto);
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken)).ReturnsAsync(_operationResult);

            AddService(x => MockGraphqlClient.Object);
        }

        [Test]
        public async Task IF_Mutation_executes_successfully_SHOULD_convert_result_to_dto_and_returns()
        {
            //Act
            var result = await Sut.HandleAsync(_commandDto, CancellationToken);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(_dto, result.Value);
        }

        [Test]
        public void IF_Mutation_fails_with_Exception_SHOULD_throw_exception()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ThrowsAsync(new Exception("oops"));

            //Act
            Assert.ThrowsAsync<Exception>(async () => await Sut.HandleAsync(_commandDto, CancellationToken), "oops");

        }

        [Test]
        public void IF_Mutation_fails_with_non_Error_error_SHOULD_throw()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync(new OperationResultMockBuilder<TestGraphqlResponse>()
                    .With_Error("oops").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await Sut.HandleAsync(_commandDto, CancellationToken), "oops");
        }

        [Test]
        public void IF_Mutation_fails_with_IError_with_message_extension_SHOULD_use_it_as_message()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync(new OperationResultMockBuilder<TestGraphqlResponse>()
                    .WithExtension("message", "underlying errsor message").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await Sut.HandleAsync(_commandDto, CancellationToken));
        }

        [Test]
        public void IF_Mutation_fails_with_multiple_non_Error_error_SHOULD_throw()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync((new OperationResultMockBuilder<TestGraphqlResponse>()
                    .With_Error("oops").Object));

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await Sut.HandleAsync(_commandDto, CancellationToken));
        }

        [Test]
        public async Task IF_Mutation_fails_with_Error_SHOULD_return_failure()
        {
            //Arrange
            var error = Error.Create("Bad Thing");
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync(new OperationResultMockBuilder<TestGraphqlResponse>()
                    .With_Error(error.ToString()).Object);

            //Act
            var result = await Sut.HandleAsync(_commandDto, CancellationToken);

            //Assert
            Assert.AreEqual(error.ToString(), result.Error);
        }
         
    }
}