using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Auth.Abstractions.Builders;
using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Server;
using Blauhaus.Graphql.HotChocolate.MutationHandlers;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.MockBuilders;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.Graphql.Tests.Tests._Base;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Graphql.Tests.Tests.HotChocolateTests.AuthenticatedUserMutationServerHandlerTests
{
    public class HandleAsyncTests : BaseGraphqlTest<AuthenticatedUserMutationServerHandler>
    {

        private TestCommandHandlerMockBuilder _mockTestCommandHandler;

        public override void Setup()
        {
            base.Setup();
            _mockTestCommandHandler = new TestCommandHandlerMockBuilder()
                .Where_HandleAsync_returns(new TestServerPayload{Name = "Freddie"});
            MockResolverContext.With_Service(_mockTestCommandHandler.Object);
            MockResolverContext.With_Command_Argument(new TestServerCommand
            {
                Name = "Piet"
            });
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
            await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockAnalyticsService.VerifyStartRequestOperation("TestServerCommand");
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
            await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            MockAzureAuthenticationServerService.Mock.Verify(x => x.ExtractUserFromClaimsPrincipal(It.Is<ClaimsPrincipal>(y => 
                y.HasClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "Fred"))));
        }

        [Test]
        public async Task IF_ClaimsPrincipal_is_not_authenticated_SHOULD_return_error()
        {
            //Arrange
            MockResolverContext.With_ContextData("ClaimsPrincipal", new ClaimsPrincipalBuilder()
                .WithIsAuthenticatedFalse().Build()); 

            //Act
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task SHOULD_extract_command_and_invoke_on_handler()
        {
            //Act
            await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None);

            //Assert
            _mockTestCommandHandler.VerifyHandleCalledWithCommandProperty(x => x.Name == "Piet");
        }

        [Test]
        public async Task IF_command_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Command_Argument<TestServerCommand>(null);;

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_cannot_be_found_SHOULD_throw()
        {
            //Arrange
            MockResolverContext.With_Service<ICommandServerHandler<TestServerPayload, TestServerCommand, IAuthenticatedUser>>(null);;

            //Act
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None));
        }

        [Test]
        public async Task IF_command_handler_succeeds_SHOULD_return_value()
        {

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None);
        
            //Asserrt
            Assert.AreEqual("Freddie", result.Name);
        }

        [Test]
        public async Task IF_command_handler_fails_SHOULD_report_error()
        {
            //Arrange
            _mockTestCommandHandler.Where_HandleAsync_returns_error("Oops");

            //Act
            var result = await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None);
        
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
                await Sut.HandleAsync<TestServerPayload, TestServerCommand>(MockResolverContext.Object, CancellationToken.None));

            //Asserrt
            MockAnalyticsService.VerifyLogException<Exception>("Oops");
        }
    }
}