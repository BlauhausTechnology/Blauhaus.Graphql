using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; 
using Blauhaus.Domain.Common.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Void;
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
    public class VoidAnonymousServerQueryHandlerTests : BaseGraphqlTest<VoidAnonymousServerQueryHandler>
    {
        private TestCommand _command;

        private MockBuilder<IVoidCommandHandler<TestCommand>> MockCommandHandler => AddMock<IVoidCommandHandler<TestCommand>>().Invoke();

        public override void Setup()
        {
            base.Setup();

            _command = new TestCommand {Name = "Piet"};
            MockCommandHandler.Mock.Setup(x => x.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());
            MockResolverContext.With_Service(MockCommandHandler.Object);
            MockResolverContext.With_Command_Argument(_command); 
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
            await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockAnalyticsService.VerifyStartRequestOperation("TestCommand");
            MockAnalyticsService.VerifyStartRequestOperationProperty(x => x["HeaderOne"] == "HeaderOneValue");
            MockAnalyticsService.VerifyStartRequestOperationProperty(x => x["HeaderTwo"] == "HeaderTwoValue");
        }

        [Test]
        public async Task SHOULD_extract_command_and_invoke_on_handler()
        {
            //Act
            await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockCommandHandler.Mock.Verify(x => x.HandleAsync(It.Is<TestCommand>(y => y.Name == "Piet"), It.IsAny<CancellationToken>()));
        }

        
        [Test]
        public async Task SHOULD_extract_command_and_log_with_analytics()
        {
            //Act
            await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert 
            MockAnalyticsService.VerifyTrace("Command received");
            MockAnalyticsService.VerifyTraceProperty("TestCommand", _command);
        }


        [Test]
        public async Task IF_command_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Command_Argument<TestCommand>(null);;

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Service<IVoidCommandHandler<TestCommand>>(null);

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_succeeds_SHOULD_return_true()
        {

            //Act
            var result = await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None);
        
            //Asserrt
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task IF_command_handler_fails_SHOULD_report_error()
        {
            //Arrange
            MockCommandHandler.Mock.Setup(x => x.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("Oops"));

            //Act
            var result = await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None);
        
            //Asserrt
            Assert.IsFalse(result);
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
                await Sut.HandleAsync<TestCommand>(MockResolverContext.Object, CancellationToken.None));

            //Asserrt
            MockAnalyticsService.VerifyLogException<Exception>("Oops");
        }
    }
}