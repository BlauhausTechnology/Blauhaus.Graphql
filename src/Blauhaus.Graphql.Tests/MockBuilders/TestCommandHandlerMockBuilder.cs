using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Graphql.Tests.MockBuilders
{
    public class TestCommandHandlerMockBuilder : BaseAuthenticatedUserCommandHandlerMockBuilder<TestCommandHandlerMockBuilder, 
        IAuthenticatedUserCommandHandler<TestServerPayload, TestServerCommand>, TestServerPayload, TestServerCommand >
    {
        
    }
}