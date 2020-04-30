using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.Graphql.Tests.Tests._Base;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Graphql.Tests.Tests.HotChocolateTests
{
    public class AnonymousServerQueryHandlerTests : BaseGraphqlTest<AnonymousServerQueryHandler>
    {

        private MockBuilder<ICommandHandler<TestServerPayload, TestCommand>> MockCommandHandler => AddMock<ICommandHandler<TestServerPayload, TestCommand>>().Invoke();

        public override void Setup()
        {
            base.Setup();
            MockCommandHandler.Mock.Setup(x => x.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(new TestServerPayload{Name = "Freddie"}));
            MockResolverContext.With_Service(MockCommandHandler.Object);
            MockResolverContext.With_Command_Argument(new TestCommand
            {
                Name = "Piet"
            }); 
        }


        [Test]
        public async Task SHOULD_extract_headers_and_start_request_operation()
        {
            //Arrange
            MockResolverContext.With_ContextData("HttpContext", new HttpContextMockBuilder()
                .WithHeaders(new HeaderDictionary(new Dictionary<string, StringValues>
                {
                    {"HeaderOne", "HeaderOneValue" },
                    {"HeaderTwo", "HeaderTwoValue" },
                })).Object);

            //Act
            await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockAnalyticsService.VerifyStartRequestOperation("TestCommand");
            MockAnalyticsService.VerifyStartRequestOperationProperty(x => x["HeaderOne"] == "HeaderOneValue");
            MockAnalyticsService.VerifyStartRequestOperationProperty(x => x["HeaderTwo"] == "HeaderTwoValue");
        }

        [Test]
        public async Task SHOULD_extract_command_and_invoke_on_handler()
        {
            //Act
            await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockCommandHandler.Mock.Verify(x => x.HandleAsync(It.Is<TestCommand>(y => y.Name == "Piet"), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task IF_command_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Command_Argument<TestCommand>(null);;

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Service<ICommandHandler<TestServerPayload, TestCommand>>(null);

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_succeeds_SHOULD_return_value()
        {

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);
        
            //Asserrt
            Assert.AreEqual("Freddie", result.Name);
        }

        [Test]
        public async Task IF_command_handler_fails_SHOULD_report_error()
        {
            //Arrange
            MockCommandHandler.Mock.Setup(x => x.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<TestServerPayload>("Oops"));

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);
        
            //Asserrt
            Assert.IsNull(result);
            MockResolverContext.Mock.Verify(x => x.ReportError(It.Is<IError>(y => 
                y.Message == "Oops")));
        }

        [Test]
        public async Task IF_command_handler_throws_SHOULD_log_exception_and_rethrow()
        {
            //Arrange
            MockCommandHandler.Mock.Setup(x => x.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Oops"));

            //Act
            Assert.ThrowsAsync<Exception>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None));

            //Asserrt
            MockAnalyticsService.VerifyLogException<Exception>("Oops");
        }
    }
}