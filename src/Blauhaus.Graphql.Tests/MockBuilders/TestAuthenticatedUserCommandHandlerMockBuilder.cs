﻿using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Graphql.Tests.MockBuilders
{
    public class TestAuthenticatedUserCommandHandlerMockBuilder : BaseAuthenticatedCommandHandlerMockBuilder<TestAuthenticatedUserCommandHandlerMockBuilder, 
        IAuthenticatedCommandHandler<TestServerPayload, TestCommand, IAuthenticatedUser>, TestServerPayload, TestCommand, IAuthenticatedUser>
    {
        
    }
}