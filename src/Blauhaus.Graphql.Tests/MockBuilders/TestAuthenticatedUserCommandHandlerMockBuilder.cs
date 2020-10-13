using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.TestObjects;

namespace Blauhaus.Graphql.Tests.MockBuilders
{
    public class TestAuthenticatedUserCommandHandlerMockBuilder : BaseAuthenticatedCommandHandlerMockBuilder<TestAuthenticatedUserCommandHandlerMockBuilder, 
        IAuthenticatedCommandHandler<TestServerPayload, TestCommand, IAuthenticatedUser>, TestServerPayload, TestCommand, IAuthenticatedUser>
    {
        
    }
}