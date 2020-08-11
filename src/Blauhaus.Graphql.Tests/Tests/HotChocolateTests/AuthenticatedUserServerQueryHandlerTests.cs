using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Builders;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Domain.Common.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.MockBuilders;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.Graphql.Tests.Tests._Base;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Graphql.Tests.Tests.HotChocolateTests
{
    public class AuthenticatedUserServerQueryHandlerTests : BaseGraphqlTest<AuthenticatedUserServerQueryHandler>
    {

        private TestAuthenticatedUserCommandHandlerMockBuilder _mockTestCommandHandler;
        private TestCommand _command;

        public override void Setup()
        {
            base.Setup();
            _command = new TestCommand  { Name = "Piet" };
            _mockTestCommandHandler = new TestAuthenticatedUserCommandHandlerMockBuilder()
                .Where_HandleAsync_returns(new TestServerPayload{Name = "Freddie"});
            MockResolverContext.With_Service(_mockTestCommandHandler.Object);
            MockResolverContext.With_Command_Argument(_command);
            Services.AddSingleton(_mockTestCommandHandler.Object);
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
        public async Task SHOULD_extract_claims_principal_and_get_user()
        {
            //Arrange
            MockResolverContext.With_ContextData("ClaimsPrincipal", new ClaimsPrincipalBuilder()
                    .With_NameIdentifier("Fred").Build()); 

            //Act
            await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockAuthenticatedUserFactory.Mock.Verify(x => x.Create(It.Is<ClaimsPrincipal>(y => 
                y.HasClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "Fred"))));
        }

        [Test]
        public async Task IF_ClaimsPrincipal_is_not_authenticated_SHOULD_return_error()
        {
            //Arrange
            MockResolverContext.With_ContextData("ClaimsPrincipal", new ClaimsPrincipalBuilder()
                .WithIsAuthenticatedFalse().Build()); 

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            Assert.IsNull(result);
            MockResolverContext.Mock.Verify(x => x.ReportError(It.Is<IError>(y => y.Message == AuthErrors.NotAuthenticated.ToString())));
            MockAnalyticsService.VerifyTrace(AuthErrors.NotAuthenticated.Code, LogSeverity.Error);
        }

        [Test]
        public async Task IF_ClaimsPrincipal_is_bogus_return_error()
        {
            //Arrange
            MockResolverContext.With_ContextData("ClaimsPrincipal", null); 

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            Assert.IsNull(result);
            MockResolverContext.Mock.Verify(x => x.ReportError(It.Is<IError>(y => y.Message == AuthErrors.NotAuthenticated.ToString())));
            MockAnalyticsService.VerifyTrace(AuthErrors.NotAuthenticated.Code, LogSeverity.Error);
        }
        
        [Test]
        public async Task IF_command_handler_throws_UnauthorizedAccessException_SHOULD_log_exception_and_report_error()
        {
            //Arrange
            _mockTestCommandHandler.Mock.Setup(x => x.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<IAuthenticatedUser>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Asserrt
            Assert.IsNull(result);
            MockResolverContext.Mock.Verify(x => x.ReportError(It.Is<IError>(y => y.Message == AuthErrors.NotAuthorized.ToString())));
            MockAnalyticsService.VerifyTrace(AuthErrors.NotAuthorized.Code, LogSeverity.Error);
        }

        [Test]
        public async Task SHOULD_extract_command_and_invoke_on_handler()
        {
            //Act
            await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            _mockTestCommandHandler.VerifyHandleCalledWithCommandProperty(x => x.Name == "Piet");
        }
        
        [Test]
        public async Task SHOULD_extract_command_and_log_with_analytics()
        {
            //Act
            await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None);

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
                await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Service<IAuthenticatedCommandHandler<TestServerPayload, TestCommand, IAuthenticatedUser>>(null);;

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
            _mockTestCommandHandler.Where_HandleAsync_returns_error("Oops");

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
            _mockTestCommandHandler.Where_HandleAsync_throws(new Exception("Oops"));

            //Act
            Assert.ThrowsAsync<Exception>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestCommand>(MockResolverContext.Object, CancellationToken.None));

            //Asserrt
            MockAnalyticsService.VerifyLogException<Exception>("Oops");
        }
    }
}